﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLCGen.Generators.CCOL.Settings;
using TLCGen.Models;
using TLCGen.Models.Enumerations;

namespace TLCGen.Generators.CCOL.CodeGeneration.Functionality
{
    [CCOLCodePieceGenerator]
    public class VasteAanvraagCodeGenerator : CCOLCodePieceGeneratorBase
    {
#pragma warning disable 0649
        private CCOLGeneratorCodeStringSettingModel _schca;
        private CCOLGeneratorCodeStringSettingModel _tuitgestca;
#pragma warning restore 0649

        public override void CollectCCOLElements(ControllerModel c)
        {
            _myElements = new List<CCOLElement>();

            foreach (FaseCyclusModel fcm in c.Fasen)
            {
                if (fcm.VasteAanvraag != Models.Enumerations.NooitAltijdAanUitEnum.Nooit &&
                    fcm.VasteAanvraag != Models.Enumerations.NooitAltijdAanUitEnum.Altijd)
                {
                    _myElements.Add(
                        CCOLGeneratorSettingsProvider.Default.CreateElement(
                            $"{_schca}{fcm.Naam}", fcm.VasteAanvraag == Models.Enumerations.NooitAltijdAanUitEnum.SchAan ? 1 : 0, CCOLElementTimeTypeEnum.SCH_type, _schca, fcm.Naam));
                }

                if (fcm.VasteAanvraag != Models.Enumerations.NooitAltijdAanUitEnum.Nooit &&
                    fcm.UitgesteldeVasteAanvraag)
                {
                    _myElements.Add(
                        CCOLGeneratorSettingsProvider.Default.CreateElement(
                            $"{_tuitgestca}{fcm.Naam}", fcm.UitgesteldeVasteAanvraagTijdsduur, CCOLElementTimeTypeEnum.TE_type, _tuitgestca, fcm.Naam));
                }
            }
        }

        public override bool HasCCOLElements()
        {
            return true;
        }

        public override IEnumerable<CCOLElement> GetCCOLElements(CCOLElementTypeEnum type)
        {
            return _myElements.Where(x => x.Type == type);
        }

        public override int HasCode(CCOLCodeTypeEnum type)
        {
            switch (type)
            {
                case CCOLCodeTypeEnum.RegCAanvragen:
                    return 50;
                default:
                    return 0;
            }
        }

        public override string GetCode(ControllerModel c, CCOLCodeTypeEnum type, string ts)
        {
            StringBuilder sb = new StringBuilder();

            switch (type)
            {
                case CCOLCodeTypeEnum.RegCAanvragen:
                    sb.AppendLine($"{ts}/* Vaste aanvragen */");
                    sb.AppendLine($"{ts}/* --------------- */");
                    foreach (FaseCyclusModel fcm in c.Fasen)
                    {
                        if (fcm.VasteAanvraag == NooitAltijdAanUitEnum.SchAan ||
                            fcm.VasteAanvraag == NooitAltijdAanUitEnum.SchUit)
                        {
                            if (fcm.UitgesteldeVasteAanvraag)
                            {
                                sb.AppendLine($"{ts}if (SCH[{_schpf}{_schca}{fcm.Naam}])");
                                sb.AppendLine($"{ts}{{");
                                sb.AppendLine($"{ts}{ts}RT[{_tpf}{_tuitgestca}{fcm.Naam}] = !T[{_tpf}{_tuitgestca}{fcm.Naam}] && !ET[{_tpf}{_tuitgestca}{fcm.Naam}] && R[{_fcpf}{fcm.Naam}] && !A[{_fcpf}{fcm.Naam}];");
                                sb.AppendLine($"{ts}{ts}if (ET[{_tpf}{_tuitgestca}{fcm.Naam}]) vaste_aanvraag({_fcpf}{fcm.Naam});");
                                sb.AppendLine($"{ts}}}");
                            }
                            else
                            {
                                sb.AppendLine($"{ts}if (SCH[{_schpf}{_schca}{fcm.Naam}]) vaste_aanvraag({_fcpf}{fcm.Naam});");
                            }
                        }
                        else if (fcm.VasteAanvraag == NooitAltijdAanUitEnum.Altijd)
                        {
                            if (fcm.UitgesteldeVasteAanvraag)
                            {
                                sb.AppendLine($"{ts}RT[{_tpf}{_tuitgestca}{fcm.Naam}] = !T[{_tpf}{_tuitgestca}{fcm.Naam}] && !ET[{_tpf}{_tuitgestca}{fcm.Naam}] && R[{_fcpf}{fcm.Naam}] && !A[{_fcpf}{fcm.Naam}];");
                                sb.AppendLine($"{ts}if (ET[{_tpf}{_tuitgestca}{fcm.Naam}]) vaste_aanvraag({_fcpf}{fcm.Naam});");
                            }
                            else
                            {
                                sb.AppendLine($"{ts}vaste_aanvraag({_fcpf}{fcm.Naam});");
                            }
                        }
                    }
                    sb.AppendLine();
                    return sb.ToString();
                default:
                    return null;
            }
        }
    }
}
