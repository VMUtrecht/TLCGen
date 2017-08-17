﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLCGen.Generators.CCOL.Extensions;
using TLCGen.Generators.CCOL.Settings;
using TLCGen.Models;
using TLCGen.Models.Enumerations;

namespace TLCGen.Generators.CCOL.CodeGeneration.Functionality
{
    [CCOLCodePieceGenerator]
    public class CCOLVeiligheidsGroenCodeGenerator : CCOLCodePieceGeneratorBase
    {
        private List<CCOLElement> _MyElements;

#pragma warning disable 0649
        private string _schvg; // schakelaar veiligheidsgroen
        private string _tvga; // veiligheidsgroen min. tijdsduur in MG
        private string _tvgb; // tijdsduur veiligheidsgroen
#pragma warning restore 0649
        private string _hfile;

        public override void CollectCCOLElements(ControllerModel c)
        {
            _MyElements = new List<CCOLElement>();

            foreach (var dm in c.Fasen.SelectMany(x => x.Detectoren))
            {
                if (dm.VeiligheidsGroen != Models.Enumerations.NooitAltijdAanUitEnum.Nooit)
                {
                    if (dm.VeiligheidsGroen != Models.Enumerations.NooitAltijdAanUitEnum.Altijd)
                    {
                        _MyElements.Add(
                            new CCOLElement(
                                $"{_schvg}{dm.Naam}",
                                (dm.VeiligheidsGroen == Models.Enumerations.NooitAltijdAanUitEnum.SchAan ? 1 : 0),
                                CCOLElementTimeTypeEnum.SCH_type,
                                CCOLElementTypeEnum.Schakelaar));
                    }
                    _MyElements.Add(
                        new CCOLElement(
                            $"{_tvga}{dm.Naam}",
                            dm.VeiligheidsGroenMinMG,
                            CCOLElementTimeTypeEnum.TE_type,
                            CCOLElementTypeEnum.Timer));
                    _MyElements.Add(
                        new CCOLElement(
                            $"{_tvgb}{dm.Naam}",
                            dm.VeiligheidsGroenTijdsduur,
                            CCOLElementTimeTypeEnum.TE_type,
                            CCOLElementTypeEnum.Timer));
                }
            }
        }

        public override bool HasCCOLElements()
        {
            return true;
        }

        public override IEnumerable<CCOLElement> GetCCOLElements(CCOLElementTypeEnum type)
        {
            return _MyElements.Where(x => x.Type == type);
        }

        public override int HasCode(CCOLRegCCodeTypeEnum type)
        {
            switch (type)
            {
                case CCOLRegCCodeTypeEnum.Meeverlengen:
                    return 2;
                default:
                    return 0;
            }
        }

        public override string GetCode(ControllerModel c, CCOLRegCCodeTypeEnum type, string ts)
        {
            StringBuilder sb = new StringBuilder();

            switch (type)
            {
                case CCOLRegCCodeTypeEnum.Meeverlengen:
                    if (c.Fasen.SelectMany(x => x.Detectoren).Any(x => x.VeiligheidsGroen != NooitAltijdAanUitEnum.Nooit))
                    {
                        sb.AppendLine($"{ts}/* Veiligheidsgroen */");
                        sb.AppendLine($"{ts}/* ---------------- */");
                        foreach (var fcm in c.Fasen)
                        {
                            foreach (var dm in fcm.Detectoren)
                            {
                                if (dm.VeiligheidsGroen != NooitAltijdAanUitEnum.Nooit)
                                {
                                    if (dm.VeiligheidsGroen == NooitAltijdAanUitEnum.Altijd)
                                    {
                                        sb.AppendLine($"{ts}veiligheidsgroen({_fcpf}{fcm.Naam}, {_tpf}{_tvga}{fcm.Naam}, {_tpf}{_tvgb}{fcm.Naam}, (bool)(TDH[{_dpf}{dm.Naam}]));");
                                    }
                                    else
                                    {
                                        sb.AppendLine($"{ts}veiligheidsgroen({_fcpf}, {_tpf}{_tvga}{fcm.Naam}, {_tpf}{_tvgb}{fcm.Naam}, (bool)(SCH[{_schpf}{_schvg}{dm.Naam}] && TDH[{_dpf}{dm.Naam}]));");
                                    }
                                }
                            }
                        }
                        sb.AppendLine();
                    }
                    return sb.ToString();
                default:
                    return null;
            }
        }

        public override bool SetSettings(CCOLGeneratorClassWithSettingsModel settings)
        {
            _hfile = CCOLGeneratorSettingsProvider.Default.GetElementName("hfile");

            return base.SetSettings(settings);
        }
    }
}
