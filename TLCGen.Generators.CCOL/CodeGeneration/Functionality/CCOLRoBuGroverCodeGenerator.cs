﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLCGen.Models;

namespace TLCGen.Generators.CCOL.CodeGeneration
{
    [CCOLCodePieceGenerator]
    public class CCOLRoBuGroverCodeGenerator : CCOLCodePieceGeneratorBase
    {
        private List<CCOLElement> _MyElements;
        private List<CCOLIOElement> _MyBitmapOutputs;

        public override void CollectCCOLElements(ControllerModel c)
        {
            _MyElements = new List<CCOLElement>();
            _MyBitmapOutputs = new List<CCOLIOElement>();

            _MyElements.Add(new CCOLElement("rgv", 2, CCOLElementTimeTypeEnum.None, CCOLElementTypeEnum.Parameter));
            _MyElements.Add(new CCOLElement("min_tcyclus", 900, CCOLElementTimeTypeEnum.TE_type, CCOLElementTypeEnum.Parameter));
            _MyElements.Add(new CCOLElement("max_tcyclus", 1500, CCOLElementTimeTypeEnum.TE_type, CCOLElementTypeEnum.Parameter));
            _MyElements.Add(new CCOLElement("tvg_omhoog", 50, CCOLElementTimeTypeEnum.TE_type, CCOLElementTypeEnum.Parameter));
            _MyElements.Add(new CCOLElement("tvg_omlaag", 20, CCOLElementTimeTypeEnum.TE_type, CCOLElementTypeEnum.Parameter));
            _MyElements.Add(new CCOLElement("tvg_verschil", 50, CCOLElementTimeTypeEnum.TE_type, CCOLElementTypeEnum.Parameter));
            _MyElements.Add(new CCOLElement("tvg_npr_omlaag", 50, CCOLElementTimeTypeEnum.TE_type, CCOLElementTypeEnum.Parameter));
            _MyElements.Add(new CCOLElement("rgv", 1, CCOLElementTimeTypeEnum.SCH_type, CCOLElementTypeEnum.Schakelaar));
            _MyElements.Add(new CCOLElement("rgvsnel", 1, CCOLElementTimeTypeEnum.SCH_type, CCOLElementTypeEnum.Schakelaar));
            _MyElements.Add(new CCOLElement("rgv", CCOLElementTypeEnum.Uitgang));

            foreach(var fc in c.RoBuGrover.SignaalGroepInstellingen)
            {
                if (fc.FileDetectoren?.Count == 0 || fc.HiaatDetectoren?.Count == 0)
                    continue;

                _MyElements.Add(new CCOLElement($"mintvg_{fc.FaseCyclus}", fc.MinGroenTijd, CCOLElementTimeTypeEnum.TE_type, CCOLElementTypeEnum.Parameter));
                _MyElements.Add(new CCOLElement($"maxtvg_{fc.FaseCyclus}", fc.MaxGroenTijd, CCOLElementTimeTypeEnum.TE_type, CCOLElementTypeEnum.Parameter));
                _MyElements.Add(new CCOLElement($"prreal{fc.FaseCyclus}", CCOLElementTypeEnum.HulpElement));
                foreach(var d in fc.FileDetectoren)
                {
                    _MyElements.Add(new CCOLElement($"fd{d.Detector}", d.FileTijd, CCOLElementTimeTypeEnum.TE_type, CCOLElementTypeEnum.Timer));
                }
                foreach (var d in fc.HiaatDetectoren)
                {
                    _MyElements.Add(new CCOLElement($"hd{d.Detector}", d.HiaatTijd, CCOLElementTimeTypeEnum.TE_type, CCOLElementTypeEnum.Timer));
                }
            }

            _MyBitmapOutputs.Add(new CCOLIOElement(c.RoBuGrover.BitmapData as IOElementModel, "usrgv"));
        }

        public override bool HasCCOLElements()
        {
            return true;
        }

        public override IEnumerable<CCOLElement> GetCCOLElements(CCOLElementTypeEnum type)
        {
            return _MyElements.Where(x => x.Type == type);
        }

        public override bool HasCCOLBitmapOutputs()
        {
            return true;
        }

        public override IEnumerable<CCOLIOElement> GetCCOLBitmapOutputs()
        {
            return _MyBitmapOutputs;
        }

        public override bool HasCode(CCOLRegCCodeTypeEnum type)
        {
            switch (type)
            {
                case CCOLRegCCodeTypeEnum.Top:
                case CCOLRegCCodeTypeEnum.Maxgroen:
                    return true;
                default:
                    return false;
            }
        }

        public override string GetCode(ControllerModel c, CCOLRegCCodeTypeEnum type, string tabspace)
        {
            if(c.RoBuGrover.ConflictGroepen?.Count == 0)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            switch(type)
            {
                case CCOLRegCCodeTypeEnum.Top:
                    sb.AppendLine($"{tabspace}/* Robuuste Groenverdeler */");
                    sb.AppendLine($"{tabspace}#include \"{c.Data.Naam}rgv.c\"");
                    return sb.ToString();

                case CCOLRegCCodeTypeEnum.Maxgroen:
                    sb.AppendLine($"{tabspace}/* AANROEP EN RAPPOTEREN ROBUGROVER */");
                    sb.AppendLine($"{tabspace}if (SCH[schrgv] != 0)");
                    sb.AppendLine($"{tabspace}{{");
                    sb.AppendLine($"{tabspace}{tabspace}int teller = 0;");
                    sb.AppendLine();
                    foreach(var cg in c.RoBuGrover.ConflictGroepen)
                    {
                        sb.Append($"{tabspace}{tabspace}TC[teller++] = berekencyclustijd_va_arg(");
                        foreach(var fc in cg.Fasen)
                        {
#warning Change so it uses prefix settings (also at the end!)
                            sb.Append($"fc{fc.FaseCyclus}, ");
                        }
                        sb.AppendLine($"END);");
                    }
                    sb.AppendLine();
                    sb.AppendLine($"{tabspace}{tabspace}TC_max = TC[0];");
                    sb.AppendLine();
                    sb.AppendLine($"{tabspace}{tabspace}for (teller = 1; teller < MAX_AANTAL_CONFLICTGROEPEN; ++teller)");
                    sb.AppendLine($"{tabspace}{tabspace}{{");
                    sb.AppendLine($"{tabspace}{tabspace}{tabspace}if (TC_max < TC[teller])");
                    sb.AppendLine($"{tabspace}{tabspace}{tabspace}{{");
                    sb.AppendLine($"{tabspace}{tabspace}{tabspace}{tabspace}TC_max = TC[teller];");
                    sb.AppendLine($"{tabspace}{tabspace}{tabspace}}}");
                    sb.AppendLine($"{tabspace}{tabspace}}}");
                    sb.AppendLine($"{tabspace}#if !defined AUTOMAAT");
                    sb.AppendLine($"{tabspace}{tabspace}for (teller = 0; teller < MAX_AANTAL_CONFLICTGROEPEN; ++teller)");
                    sb.AppendLine($"{tabspace}{tabspace}{{");
                    sb.AppendLine($"{tabspace}{tabspace}{tabspace}xyprintf(50, teller + 1, \"%4d\", TC[teller]);");
                    sb.AppendLine($"{tabspace}{tabspace}}}");
                    sb.AppendLine($"{tabspace}#endif");
                    sb.AppendLine();
                    sb.AppendLine($"{tabspace}{tabspace}/* AANROEP ROBUUSTE GROENTIJD VERDELER */");
                    sb.AppendLine($"{tabspace}{tabspace}/* ================================== */");
                    sb.AppendLine($"{tabspace}{tabspace}rgv_add();");
                    sb.AppendLine();
                    sb.AppendLine($"{tabspace}{tabspace}CIF_GUS[usrgv] = TRUE;");
                    sb.AppendLine($"{tabspace}}}");
                    sb.AppendLine($"{tabspace}else");
                    sb.AppendLine($"{tabspace}{{");
                    foreach (var fc in c.Fasen)
                    {
                        if(fc.Type == Models.Enumerations.FaseTypeEnum.Auto)
                        {
                            sb.AppendLine($"{tabspace}{tabspace}TVG_rgv[fc{fc.Naam}] = TVG_basis[fc{fc.Naam}];");
                        }
                    }
                    sb.AppendLine();
                    sb.AppendLine($"{tabspace}{tabspace}CIF_GUS[usrgv] = FALSE;");
                    sb.AppendLine($"{tabspace}}}");

                    return sb.ToString();

                default:
                    return null;
            }
        }
    }
}
