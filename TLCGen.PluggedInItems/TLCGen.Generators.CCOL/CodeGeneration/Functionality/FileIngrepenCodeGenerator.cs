﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLCGen.Generators.CCOL.Settings;
using TLCGen.Models;
using TLCGen.Models.Enumerations;

namespace TLCGen.Generators.CCOL.CodeGeneration.Functionality
{
    [CCOLCodePieceGenerator]
    public class FileIngrepenCodeGenerator : CCOLCodePieceGeneratorBase
    {
#pragma warning disable 0649
        private CCOLGeneratorCodeStringSettingModel _hfile;
        private CCOLGeneratorCodeStringSettingModel _schfile;
        private CCOLGeneratorCodeStringSettingModel _usfile;
        private CCOLGeneratorCodeStringSettingModel _tafv;
        private CCOLGeneratorCodeStringSettingModel _trij;
        private CCOLGeneratorCodeStringSettingModel _tbz;
        private CCOLGeneratorCodeStringSettingModel _prmfperc;
        private CCOLGeneratorCodeStringSettingModel _schparlus;
        private CCOLGeneratorCodeStringSettingModel _schparstrook;
        private CCOLGeneratorCodeStringSettingModel _scheerlijkdoseren;
        private CCOLGeneratorCodeStringSettingModel _hafk;
        private CCOLGeneratorCodeStringSettingModel _tafkmingroen;
        private CCOLGeneratorCodeStringSettingModel _tminrood;
#pragma warning restore 0649

        // read from other objects
        private string _mperiod;

        public override void CollectCCOLElements(ControllerModel c)
        {
            _myElements = new List<CCOLElement>();
            _myBitmapOutputs = new List<CCOLIOElement>();

            foreach (var fm in c.FileIngrepen)
            {
                _myBitmapOutputs.Add(new CCOLIOElement(fm.BitmapData, $"{_uspf}{_usfile}{fm.Naam}"));

                _myElements.Add(
                    CCOLGeneratorSettingsProvider.Default.CreateElement(
                        $"{_usfile}{fm.Naam}",
                        _usfile, fm.Naam));
                _myElements.Add(
                    CCOLGeneratorSettingsProvider.Default.CreateElement(
                        $"{_hfile}{fm.Naam}",
                        _hfile, fm.Naam));
                _myElements.Add(
                    CCOLGeneratorSettingsProvider.Default.CreateElement(
                        $"{_schfile}{fm.Naam}",
                        1, 
                        CCOLElementTimeTypeEnum.SCH_type,
                        _schfile, fm.Naam));
                _myElements.Add(
                    CCOLGeneratorSettingsProvider.Default.CreateElement(
                        $"{_tafv}{fm.Naam}",
                        fm.AfvalVertraging,
                        CCOLElementTimeTypeEnum.TE_type,
                        _tafv, fm.Naam));
                
                if(fm.EerlijkDoseren)
                {
                    _myElements.Add(
                        CCOLGeneratorSettingsProvider.Default.CreateElement(
                            $"{_scheerlijkdoseren}{fm.Naam}",
                            fm.EerlijkDoseren ? 1 : 0,
                            CCOLElementTimeTypeEnum.SCH_type,
                            _scheerlijkdoseren, fm.Naam));
                }

                var detectorDict = new Dictionary<int, List<string>>();
                foreach (var fmd in fm.FileDetectoren)
                {
                    var d = c.Fasen.SelectMany(x => x.Detectoren).FirstOrDefault(x => x.Naam == fmd.Detector) ??
                            c.Detectoren.FirstOrDefault(x => x.Naam == fmd.Detector);
                    if (d?.Rijstrook == null) continue;
                    if (!detectorDict.ContainsKey(d.Rijstrook.Value))
                    {
                        detectorDict.Add(d.Rijstrook.Value, new List<string> { fmd.Detector });
                    }
                    else
                    {
                        detectorDict[d.Rijstrook.Value].Add(fmd.Detector);
                    }
                }
                var multiStrook = detectorDict.Count > 1;
                var multiLusPerStrook = detectorDict.Any(x => x.Value.Count > 1) ||
                                        detectorDict.Count == 0 && fm.FileDetectoren.Count > 1;

                if (fm.FileDetectoren.Count > 1)
                {
                    if (multiLusPerStrook)
                    {
                        _myElements.Add(
                            CCOLGeneratorSettingsProvider.Default.CreateElement(
                                $"{_hfile}{fm.Naam}{_schparlus}",
                                fm.MetingPerLus ? 1 : 0,
                                CCOLElementTimeTypeEnum.SCH_type,
                                _schparlus, fm.Naam));
                    }
                    if(multiStrook)
                    {
                        _myElements.Add(
                            CCOLGeneratorSettingsProvider.Default.CreateElement(
                                $"{_hfile}{fm.Naam}{_schparstrook}",
                                fm.MetingPerStrook ? 1 : 0,
                                CCOLElementTimeTypeEnum.SCH_type,
                                _schparstrook, fm.Naam));
                    }
                }
                foreach (var fd in fm.FileDetectoren)
                {
                    _myElements.Add(
                        CCOLGeneratorSettingsProvider.Default.CreateElement(
                            $"{_tafv}{fd.Detector}",
                            fd.AfvalVertraging,
                            CCOLElementTimeTypeEnum.TE_type,
                            _tafv, fd.Detector));
                    _myElements.Add(
                        CCOLGeneratorSettingsProvider.Default.CreateElement(
                            $"{_tbz}{fd.Detector}",
                            fd.BezetTijd,
                            CCOLElementTimeTypeEnum.TE_type,
                            _tbz, fd.Detector));
                    _myElements.Add(
                        CCOLGeneratorSettingsProvider.Default.CreateElement(
                            $"{_trij}{fd.Detector}",
                            fd.RijTijd,
                            CCOLElementTimeTypeEnum.TE_type,
                            _trij, fd.Detector));
                    _myElements.Add(
                        CCOLGeneratorSettingsProvider.Default.CreateElement(
                            $"{_hfile}{fd.Detector}",
                            _hfile, fd.Detector));
                }
                if (fm.EerlijkDoseren && fm.TeDoserenSignaalGroepen.Count > 0)
                {
                    _myElements.Add(
                        CCOLGeneratorSettingsProvider.Default.CreateElement(
                            $"{_prmfperc}{fm.Naam}",
                            fm.TeDoserenSignaalGroepen[0].DoseerPercentage,
                            CCOLElementTimeTypeEnum.None,
                            _prmfperc, fm.Naam));
                }
                else
                {
                    foreach (var ff in fm.TeDoserenSignaalGroepen)
                    {
                        _myElements.Add(
                            CCOLGeneratorSettingsProvider.Default.CreateElement(
                                $"{_prmfperc}{fm.Naam}{ff.FaseCyclus}",
                                ff.DoseerPercentage,
                                CCOLElementTimeTypeEnum.None,
                                _prmfperc, ff.FaseCyclus));
                    }
                }
                foreach (var ff in fm.TeDoserenSignaalGroepen.Where(x => x.AfkappenOpStartFile))
                {
                    _myElements.Add(
                        CCOLGeneratorSettingsProvider.Default.CreateElement(
                            $"{_tafkmingroen}{ff.FaseCyclus}{_hfile}{fm.Naam}",
                            ff.AfkappenOpStartFileMinGroentijd,
                            CCOLElementTimeTypeEnum.TE_type,
                            _tafkmingroen, ff.FaseCyclus));
                    _myElements.Add(
                        CCOLGeneratorSettingsProvider.Default.CreateElement(
                            $"{_hafk}{ff.FaseCyclus}{_hfile}{fm.Naam}",
                            _hafk, ff.FaseCyclus));
                }
                foreach (var ff in fm.TeDoserenSignaalGroepen.Where(x => x.MinimaleRoodtijd))
                {
                    _myElements.Add(
                        CCOLGeneratorSettingsProvider.Default.CreateElement(
                            $"{_tminrood}{ff.FaseCyclus}{_hfile}{fm.Naam}",
                            ff.MinimaleRoodtijdTijd,
                            CCOLElementTimeTypeEnum.TE_type,
                            _tminrood, ff.FaseCyclus));
                }
            }
        }

        public override bool HasCCOLElements() => true;
        
        public override bool HasCCOLBitmapOutputs() => true;

        public override bool HasFunctionLocalVariables() => true;

        public override IEnumerable<Tuple<string, string, string>> GetFunctionLocalVariables(ControllerModel c, CCOLCodeTypeEnum type)
        {
            switch (type)
            {
                case CCOLCodeTypeEnum.RegCFileVerwerking:
                    if(!c.FileIngrepen.Any(x => x.TeDoserenSignaalGroepen.Any(x2 => x2.AfkappenOpStartFile || x2.MinimaleRoodtijd)))
                        return base.GetFunctionLocalVariables(c, type);
                    return new List<Tuple<string, string, string>> { new Tuple<string, string, string>("int", "fc", "") };
                default:
                    return base.GetFunctionLocalVariables(c, type);
            }
        }

        public override int HasCode(CCOLCodeTypeEnum type)
        {
            switch (type)
            {
                case CCOLCodeTypeEnum.RegCTop:
                    return 10;
                case CCOLCodeTypeEnum.RegCInitApplication:
                    return 10;
                case CCOLCodeTypeEnum.RegCSystemApplication:
                    return 10;
                case CCOLCodeTypeEnum.RegCWachtgroen:
                    return 20;
                case CCOLCodeTypeEnum.RegCFileVerwerking:
                    return 10;
                case CCOLCodeTypeEnum.OvCPARCorrecties:
                    return 10;
                default:
                    return 0;
            }
        }

        public override string GetCode(ControllerModel c, CCOLCodeTypeEnum type, string ts)
        {
            StringBuilder sb = new StringBuilder();

            switch (type)
            {
                case CCOLCodeTypeEnum.RegCTop:
                    if (c.FileIngrepen.Any(x => x.EerlijkDoseren))
                    {
                        sb.AppendLine("/* Variabelen eerlijke filedosering */");
                        sb.AppendLine("");

                        foreach (var fi in c.FileIngrepen)
                        {
                            if (fi.EerlijkDoseren)
                            {
                                sb.AppendLine($"/* File ingreep {fi.Naam} */");
                                sb.AppendLine($"#define filefcmax{fi.Naam} {fi.TeDoserenSignaalGroepen.Count} // Aantal fasen die worden gedoseerd");
                                sb.AppendLine($"static count filefc_{fi.Naam}[filefcmax{fi.Naam}]; // Opslag fasenummers");
                                sb.AppendLine(c.Data.TypeGroentijden == GroentijdenTypeEnum.MaxGroentijden
                                    ? $"static count filefcmg_{fi.Naam}[filefcmax{fi.Naam}][{c.PeriodenData.Perioden.Count(x => x.Type == PeriodeTypeEnum.Groentijden) + 1}]; // Opslag bij fasen behorende MG parameter nummers"
                                    : $"static count filefcvg_{fi.Naam}[filefcmax{fi.Naam}][{c.PeriodenData.Perioden.Count(x => x.Type == PeriodeTypeEnum.Groentijden) + 1}]; // Opslag bij fasen behorende MG parameter nummers");
                                sb.AppendLine($"static int nogtedoseren_{fi.Naam}[filefcmax{fi.Naam}] = {{0}}; // Opslag nog te doseren actueel per fase");
                            }
                        }
                    }
                    return sb.ToString();

                case CCOLCodeTypeEnum.RegCInitApplication:
                    if (c.FileIngrepen.Any(x => x.EerlijkDoseren))
                    {
                        sb.AppendLine($"{ts}/* Initialiseren variabelen voor eerlijke filedosering */");

                        foreach (var fi in c.FileIngrepen)
                        {
                            if (!fi.EerlijkDoseren) continue;
                            var i = 0;
                            foreach(var fc in fi.TeDoserenSignaalGroepen)
                            {
                                sb.AppendLine($"{ts}filefc_{fi.Naam}[{i++}] = {_fcpf}{fc.FaseCyclus};");
                            }
                            i = 0;
                            var gtt = c.Data.TypeGroentijden == GroentijdenTypeEnum.MaxGroentijden ? "mg" : "vg";
                            foreach (var fc in fi.TeDoserenSignaalGroepen)
                            {
                                sb.Append($"{ts}");
                                var defmg = c.GroentijdenSets.FirstOrDefault(
                                    x => x.Naam == c.PeriodenData.DefaultPeriodeGroentijdenSet);
                                var defmgfc = defmg?.Groentijden.FirstOrDefault(x => x.FaseCyclus == fc.FaseCyclus);
                                if (defmgfc?.Waarde != null)
                                {
                                    sb.Append($"filefc{gtt}_{fi.Naam}[{i}][0] = {_prmpf}{c.PeriodenData.DefaultPeriodeGroentijdenSet.ToLower()}_{fc.FaseCyclus};");
                                }
                                var j = 1;
                                foreach (var per in c.PeriodenData.Perioden)
                                {

                                
                                    if (per.Type == PeriodeTypeEnum.Groentijden)
                                    {
                                        foreach (var mgsm in c.GroentijdenSets)
                                        {
                                            if (mgsm.Naam == per.GroentijdenSet)
                                            {
                                                foreach (var mgm in mgsm.Groentijden)
                                                {
                                                    if (mgm.FaseCyclus == fc.FaseCyclus && mgm.Waarde.HasValue)
                                                    {
                                                        sb.Append($" filefc{gtt}_{fi.Naam}[{i}][{j}] = {_prmpf}{mgsm.Naam.ToLower()}_{fc.FaseCyclus};");
                                                    }
                                                }
                                            }
                                        }
                                        ++j;
                                    }
                                }
                                ++i;
                                sb.AppendLine();
                            }
                        }
                        sb.AppendLine();
                    }
                    return sb.ToString();

                case CCOLCodeTypeEnum.RegCWachtgroen:
                    sb.AppendLine($"{ts}/* Niet in wachtgroen vasthouden tijdens file */");
                    foreach (var fi in c.FileIngrepen)
                    {
                        foreach(var tdfc in fi.TeDoserenSignaalGroepen)
                        {
                            sb.AppendLine($"{ts}if (IH[{_hpf}{_hfile}{fi.Naam}]) RW[{_fcpf}{tdfc.FaseCyclus}] &= ~BIT4;");
                        }
                    }
                    return sb.ToString();

                case CCOLCodeTypeEnum.RegCFileVerwerking:
                    if (!c.FileIngrepen.Any()) return "";

                    sb.AppendLine($"{ts}/* File afhandeling */");
                    sb.AppendLine($"{ts}/* ---------------- */");
                    sb.AppendLine();

                    if (c.FileIngrepen.Any(x => x.TeDoserenSignaalGroepen.Any(x2 => x2.AfkappenOpStartFile || x2.MinimaleRoodtijd)))
                    {
                        sb.AppendLine($"{ts}/* reset bitsturing */");
                        sb.AppendLine($"{ts}for (fc = 0; fc < FCMAX; ++fc)");
                        sb.AppendLine($"{ts}{{");
                        if (c.FileIngrepen.Any(x => x.TeDoserenSignaalGroepen.Any(x2 => x2.AfkappenOpStartFile)))
                            sb.AppendLine($"{ts}{ts}Z[fc] &= ~BIT5;");
                        if (c.FileIngrepen.Any(x => x.TeDoserenSignaalGroepen.Any(x2 => x2.MinimaleRoodtijd)))
                            sb.AppendLine($"{ts}{ts}X[fc] &= ~BIT5;");
                        sb.AppendLine($"{ts}}}");
                        sb.AppendLine();
                    }

                    var first = true;
                    foreach (var fm in c.FileIngrepen)
                    {
                        if (!first) sb.AppendLine();

                        sb.AppendLine($"{ts}/* File ingreep {fm.Naam} */");
                        sb.AppendLine();

                        foreach (var fd in fm.FileDetectoren)
                        {
                            sb.AppendLine($"{ts}FileMeldingV2({_dpf}{fd.Detector}, {_tpf}{_tbz}{fd.Detector}, {_tpf}{_trij}{fd.Detector}, {_tpf}{_tafv}{fd.Detector}, {_hpf}{_hfile}{fd.Detector});");
                        }

                        sb.Append($"{ts}RT[{_tpf}{_tafv}{fm.Naam}] = ");
                        int i = 0;
                        foreach (var fd in fm.FileDetectoren)
                        {
                            if (i != 0)
                            {
                                sb.Append(" || ");
                            }
                            sb.Append($"D[{_dpf}{fd.Detector}]");
                            ++i;
                        }
                        sb.AppendLine(";");

                        sb.AppendLine($"{ts}if (!(T[{_tpf}{_tafv}{fm.Naam}] || RT[{_tpf}{_tafv}{fm.Naam}]))");
                        sb.AppendLine($"{ts}{{");
                        foreach (var fd in fm.FileDetectoren)
                        {
                            sb.AppendLine($"{ts}{ts}IH[{_hpf}{_hfile}{fd.Detector}] = FALSE;");
                        }
                        sb.AppendLine($"{ts}}}");
                        
                        var detectorDict = new Dictionary<int, List<string>>();
                        foreach (var fmd in fm.FileDetectoren)
                        {
                            var d = c.Fasen.SelectMany(x => x.Detectoren).FirstOrDefault(x => x.Naam == fmd.Detector) ?? 
                                    c.Detectoren.FirstOrDefault(x => x.Naam == fmd.Detector);
                            if (d?.Rijstrook == null) continue;
                            if (!detectorDict.ContainsKey(d.Rijstrook.Value))
                            {
                                detectorDict.Add(d.Rijstrook.Value, new List<string> {fmd.Detector});
                            }
                            else
                            {
                                detectorDict[d.Rijstrook.Value].Add(fmd.Detector);
                            }
                        }
                        var multiStrook = detectorDict.Count > 1;
                        var multiLusPerStrook = detectorDict.Any(x => x.Value.Count > 1) ||
                                                detectorDict.Count == 0 && fm.FileDetectoren.Count > 1;
                        if (multiStrook)
                        {
                            if (multiLusPerStrook)
                            {
                                sb.AppendLine($"{ts}if(SCH[{_schpf}{_hfile}{fm.Naam}{_schparstrook}])");
                                sb.AppendLine($"{ts}{{");
                                sb.AppendLine($"{ts}{ts}/* strook en lus parallel: file via om het even welke lus */");
                                sb.AppendLine($"{ts}{ts}if(SCH[{_schpf}{_hfile}{fm.Naam}{_schparlus}])");
                                sb.AppendLine($"{ts}{ts}{{");
                                sb.Append($"{ts}{ts}{ts}IH[{_hpf}{_hfile}{fm.Naam}] = ");
                                var id = 0;
                                foreach (var fd in fm.FileDetectoren)
                                {
                                    if (id > 0) sb.Append(" || ");
                                    ++id;
                                    sb.Append($"IH[{_hpf}{_hfile}{fd.Detector}]");
                                }
                                sb.AppendLine($";");
                                sb.AppendLine($"{ts}{ts}}}");
                                sb.AppendLine($"{ts}{ts}/* alleen strook parallel: file via alle lussen op 1 strook */");
                                sb.AppendLine($"{ts}{ts}else");
                                sb.AppendLine($"{ts}{ts}{{");
                                sb.Append($"{ts}{ts}{ts}IH[{_hpf}{_hfile}{fm.Naam}] = ");
                                id = 0;
                                foreach (var dd in detectorDict)
                                {
                                    if (id > 0) sb.Append(" || ");
                                    ++id;
                                    sb.Append("(");
                                    var rid = 0;
                                    foreach (var rstrd in dd.Value)
                                    {
                                        if (rid > 0) sb.Append(" && ");
                                        ++rid;
                                        sb.Append($"IH[{_hpf}{_hfile}{rstrd}]");
                                    }
                                    sb.Append(")");
                                }
                                sb.AppendLine(";");
                                sb.AppendLine($"{ts}{ts}}}");
                                sb.AppendLine($"{ts}}}");
                                sb.AppendLine($"{ts}else");
                                sb.AppendLine($"{ts}{{");
                                sb.AppendLine($"{ts}{ts}/* alleen parallel lus: file via minimaal 1 lus per strook */");
                                sb.AppendLine($"{ts}{ts}if(SCH[{_schpf}{_hfile}{fm.Naam}{_schparlus}])");
                                sb.AppendLine($"{ts}{ts}{{");
                                sb.Append($"{ts}{ts}{ts}IH[{_hpf}{_hfile}{fm.Naam}] = ");
                                id = 0;
                                foreach (var dd in detectorDict)
                                {
                                    if (id > 0) sb.Append(" && ");
                                    ++id;
                                    sb.Append("(");
                                    var rid = 0;
                                    foreach (var rstrd in dd.Value)
                                    {
                                        if (rid > 0) sb.Append(" || ");
                                        ++rid;
                                        sb.Append($"IH[{_hpf}{_hfile}{rstrd}]");
                                    }
                                    sb.Append(")");
                                }
                                sb.AppendLine(";");
                                sb.AppendLine($"{ts}{ts}}}");
                                sb.AppendLine($"{ts}{ts}/* niet parallel: file bij melding van alle lussen */");
                                sb.AppendLine($"{ts}{ts}else");
                                sb.AppendLine($"{ts}{ts}{{");
                                sb.Append($"{ts}{ts}{ts}IH[{_hpf}{_hfile}{fm.Naam}] = ");
                                id = 0;
                                foreach (var fd in fm.FileDetectoren)
                                {
                                    if (id > 0) sb.Append(" && ");
                                    ++id;
                                    sb.Append($"IH[{_hpf}{_hfile}{fd.Detector}]");
                                }
                                sb.AppendLine($";");
                                sb.AppendLine($"{ts}{ts}}}");
                                sb.AppendLine($"{ts}}}");
                            }
                            else
                            {
                                sb.AppendLine($"{ts}{ts}/* strook parallel: file bij melding op 1 strook */");
                                sb.AppendLine($"{ts}if(SCH[{_schpf}{_hfile}{fm.Naam}{_schparstrook}])");
                                sb.AppendLine($"{ts}{{");
                                sb.Append($"{ts}{ts}IH[{_hpf}{_hfile}{fm.Naam}] = ");
                                var id = 0;
                                foreach (var fd in fm.FileDetectoren)
                                {
                                    if (id > 0) sb.Append(" || ");
                                    ++id;
                                    sb.Append($"IH[{_hpf}{_hfile}{fd.Detector}]");
                                }
                                sb.AppendLine($";");
                                sb.AppendLine($"{ts}}}");
                                sb.AppendLine($"{ts}/* niet parallel: file bij melding van alle lussen */");
                                sb.AppendLine($"{ts}else");
                                sb.AppendLine($"{ts}{{");
                                sb.Append($"{ts}{ts}IH[{_hpf}{_hfile}{fm.Naam}] = ");
                                id = 0;
                                foreach (var fd in fm.FileDetectoren)
                                {
                                    if (id > 0) sb.Append(" && ");
                                    ++id;
                                    sb.Append($"IH[{_hpf}{_hfile}{fd.Detector}]");
                                }
                                sb.AppendLine($";");
                                sb.AppendLine($"{ts}}}");
                            }
                        }
                        else if (multiLusPerStrook)
                        {
                            sb.AppendLine($"{ts}/* lus parallel: file bij melding op 1 lus */");
                            sb.AppendLine($"{ts}if(SCH[{_schpf}{_hfile}{fm.Naam}{_schparlus}])");
                            sb.AppendLine($"{ts}{{");
                            sb.Append($"{ts}{ts}IH[{_hpf}{_hfile}{fm.Naam}] = ");
                            var id = 0;
                            foreach (var fd in fm.FileDetectoren)
                            {
                                if (id > 0) sb.Append(" || ");
                                ++id;
                                sb.Append($"IH[{_hpf}{_hfile}{fd.Detector}]");
                            }
                            sb.AppendLine(";");
                            sb.AppendLine($"{ts}}}");
                            sb.AppendLine($"{ts}/* niet parallel: file bij melding van alle lussen */");
                            sb.AppendLine($"{ts}else");
                            sb.AppendLine($"{ts}{{");
                            sb.Append($"{ts}{ts}IH[{_hpf}{_hfile}{fm.Naam}] = ");
                            id = 0;
                            foreach (var fd in fm.FileDetectoren)
                            {
                                if (id > 0) sb.Append(" && ");
                                ++id;
                                sb.Append($"IH[{_hpf}{_hfile}{fd.Detector}]");
                            }
                            sb.AppendLine($";");
                            sb.AppendLine($"{ts}}}");
                        }
                        else // 1 lus
                        {
                            sb.Append($"{ts}{ts}IH[{_hpf}{_hfile}{fm.Naam}] = ");
                            i = 0;
                            foreach (var fd in fm.FileDetectoren)
                            {
                                if (i != 0)
                                {
                                    sb.Append(" || ");
                                }
                                sb.Append($"IH[{_hpf}{_hfile}{fd.Detector}]");
                                ++i;
                            }
                            sb.AppendLine($";");
                        }
                        
                        sb.AppendLine();
                        sb.AppendLine($"{ts}/* percentage MG bij filemelding */");
                        sb.AppendLine($"{ts}if (IH[{_hpf}{_hfile}{fm.Naam}] && SCH[{_schpf}{_schfile}{fm.Naam}])");
                        sb.AppendLine($"{ts}{{");
                        foreach (var ff in fm.TeDoserenSignaalGroepen)
                        {
                            string grfunc = "";
                            switch (c.Data.TypeGroentijden)
                            {
                                case GroentijdenTypeEnum.MaxGroentijden: grfunc = "PercentageMaxGroenTijden"; break;
                                case GroentijdenTypeEnum.VerlengGroentijden: grfunc = "PercentageVerlengGroenTijden"; break;
                            }
                            sb.AppendLine(fm.EerlijkDoseren
                                ? $"{ts}{ts}{grfunc}({_fcpf}{ff.FaseCyclus}, {_mpf}{_mperiod}, {_prmpf}{_prmfperc}{fm.Naam},"
                                : $"{ts}{ts}{grfunc}({_fcpf}{ff.FaseCyclus}, {_mpf}{_mperiod}, {_prmpf}{_prmfperc}{fm.Naam}{ff.FaseCyclus},");
                            sb.Append("".PadLeft($"{ts}{ts}{grfunc}(".Length));
                            var rest = "";
                            var irest = 1;
                            rest += $", {_prmpf}{(c.PeriodenData.DefaultPeriodeGroentijdenSet == null ? "NG" : c.PeriodenData.DefaultPeriodeGroentijdenSet.ToLower())}_{ff.FaseCyclus}";

                            foreach (var per in c.PeriodenData.Perioden.Where(x => x.Type == PeriodeTypeEnum.Groentijden))
                            {
                                foreach (var mgsm in c.GroentijdenSets.Where(x => x.Naam == per.GroentijdenSet))
                                {
                                    foreach (var mgm in mgsm.Groentijden.Where(
                                        x => x.FaseCyclus == ff.FaseCyclus && x.Waarde.HasValue))
                                    {
                                        ++irest;
                                        rest += $", {_prmpf}{per.GroentijdenSet.ToLower()}_{ff.FaseCyclus}";
                                    }
                                }
                            }
                            sb.AppendLine($"{irest}{rest});");
                        }
                        sb.AppendLine($"{ts}}}");

                        if (fm.TeDoserenSignaalGroepen.Any(x => x.AfkappenOpStartFile))
                        {
                            sb.AppendLine($"{ts}/* Eenmalige afkappen op start file ingreep */");
                            foreach (var tdfc in fm.TeDoserenSignaalGroepen.Where(x => x.AfkappenOpStartFile))
                            {
                                sb.AppendLine($"{ts}/* Eenmalige afkappen fase {tdfc.FaseCyclus} */");
                                sb.AppendLine($"{ts}RT[{_tpf}{_tafkmingroen}{tdfc.FaseCyclus}{_hfile}{fm.Naam}] = ER[{_fcpf}{tdfc.FaseCyclus}] && T_max[{_tpf}{_tafkmingroen}{tdfc.FaseCyclus}{_hfile}{fm.Naam}];");
                                sb.AppendLine($"{ts}if (SH[{_hpf}{_hfile}{fm.Naam}] && G[{_fcpf}{tdfc.FaseCyclus}]) IH[{_hpf}{_hafk}{tdfc.FaseCyclus}{_hfile}{fm.Naam}] = TRUE;");
                                sb.AppendLine($"{ts}if (EG[{_fcpf}{tdfc.FaseCyclus}]) IH[{_hpf}{_hafk}{tdfc.FaseCyclus}{_hfile}{fm.Naam}] = FALSE;");
                                sb.AppendLine($"{ts}if (IH[{_hpf}{_hafk}{tdfc.FaseCyclus}{_hfile}{fm.Naam}] && G[{_fcpf}{tdfc.FaseCyclus}] && !T[{_tpf}{_tafkmingroen}{tdfc.FaseCyclus}{_hfile}{fm.Naam}] && T_max[{_tpf}{_tafkmingroen}{tdfc.FaseCyclus}{_hfile}{fm.Naam}]) Z[{_fcpf}{tdfc.FaseCyclus}] |= BIT5;");
                            }
                            sb.AppendLine();
                        }

                        if (fm.TeDoserenSignaalGroepen.Any(x => x.MinimaleRoodtijd))
                        {
                            sb.AppendLine($"{ts}/* Eenmalige afkappen op start file ingreep */");
                            foreach (var tdfc in fm.TeDoserenSignaalGroepen.Where(x => x.MinimaleRoodtijd))
                            {
                                sb.AppendLine($"{ts}/* Minimale roodtijd tijdens fase {tdfc.FaseCyclus} tijdens file ingreep */");
                                sb.AppendLine($"{ts}RT[{_tpf}{_tminrood}{tdfc.FaseCyclus}{_hfile}{fm.Naam}] = EGL[{_fcpf}{tdfc.FaseCyclus}] && T_max[{_tpf}{_tminrood}{tdfc.FaseCyclus}{_hfile}{fm.Naam}] && IH[{_hpf}{_hfile}{fm.Naam}];");
                                sb.AppendLine($"{ts}if (R[{_fcpf}{tdfc.FaseCyclus}] && T[{_tpf}{_tminrood}{tdfc.FaseCyclus}{_hfile}{fm.Naam}]) X[{_fcpf}{tdfc.FaseCyclus}] |= BIT5;");
                            }
                            sb.AppendLine();
                        }

                        first = false;
                    }
                    if (c.FileIngrepen.Any(x => x.EerlijkDoseren))
                    {
                        sb.Append($"{ts}/* Eerlijk doseren: deze functie compenseert zodanig, dat voor alle richtingen gelijk wordt gedoseerd. */");
                        foreach (var fm in c.FileIngrepen)
                        {
                            sb.AppendLine();
                            if (fm.EerlijkDoseren && fm.TeDoserenSignaalGroepen.Count > 0)
                            {
                                sb.AppendLine($"{ts}if(SCH[{_schpf}{_scheerlijkdoseren}{fm.Naam}])");
                                sb.AppendLine($"{ts}{{");
                                if (c.Data.TypeGroentijden == GroentijdenTypeEnum.MaxGroentijden)
                                {
                                    if (!c.Data.MultiModuleReeksen)
                                    {
                                        sb.AppendLine($"{ts}{ts}Eerlijk_doseren_V1({_hpf}{_hfile}{fm.Naam}, {_prmpf}{_prmfperc}{fm.Naam}, filefcmax{fm.Naam}, filefc_{fm.Naam}, filefcmg_{fm.Naam}, nogtedoseren_{fm.Naam}, PRML, ML);");
                                    }
                                    else
                                    {
                                        var r = c.MultiModuleMolens.FirstOrDefault(x => x.Modules.Any(x2 => x2.Fasen.Any(x3 => fm.TeDoserenSignaalGroepen.Any(x4 => x3.FaseCyclus == x4.FaseCyclus))));
                                        if (r != null)
                                        {
                                            sb.AppendLine($"{ts}{ts}Eerlijk_doseren_V1({_hpf}{_hfile}{fm.Naam}, {_prmpf}{_prmfperc}{fm.Naam}, filefcmax{fm.Naam}, filefc_{fm.Naam}, filefcmg_{fm.Naam}, nogtedoseren_{fm.Naam}, PR{r.Reeks}, {r.Reeks});");
                                        }
                                    }
                                }
                                else
                                {
                                    if (!c.Data.MultiModuleReeksen)
                                    {
                                        sb.AppendLine($"{ts}{ts}Eerlijk_doseren_VerlengGroenTijden_V1({_hpf}{_hfile}{fm.Naam}, {_prmpf}{_prmfperc}{fm.Naam}, filefcmax{fm.Naam}, filefc_{fm.Naam}, filefcvg_{fm.Naam}, nogtedoseren_{fm.Naam}, &PRML, ML);");
                                    }
                                    else
                                    {
                                        var r = c.MultiModuleMolens.FirstOrDefault(x => x.Modules.Any(x2 => x2.Fasen.Any(x3 => fm.TeDoserenSignaalGroepen.Any(x4 => x3.FaseCyclus == x4.FaseCyclus))));
                                        if (r != null)
                                        {
                                            sb.AppendLine($"{ts}{ts}Eerlijk_doseren_VerlengGroenTijden_V1({_hpf}{_hfile}{fm.Naam}, {_prmpf}{_prmfperc}{fm.Naam}, filefcmax{fm.Naam}, filefc_{fm.Naam}, filefcvg_{fm.Naam}, nogtedoseren_{fm.Naam}, &PR{r.Reeks}, {r.Reeks});");
                                        }
                                    }
                                }
                                    
                                sb.AppendLine($"{ts}}}");
                            }
                        }
                    }
                    return sb.ToString();

                case CCOLCodeTypeEnum.RegCSystemApplication:
                    if (!c.FileIngrepen.Any()) return "";

                    sb.AppendLine($"{ts}/* file verklikking */");
                    sb.AppendLine($"{ts}/* ---------------- */");
                    foreach (var f in c.FileIngrepen)
                    {
                        sb.AppendLine($"{ts}CIF_GUS[{_uspf}{_usfile}{f.Naam}] = IH[{_hpf}{_hfile}{f.Naam}];");
                    }
                    return sb.ToString();

                case CCOLCodeTypeEnum.OvCPARCorrecties:
                    if (!c.FileIngrepen.Any()) return "";

                    var yes = false;
                    foreach (var fcm in c.Fasen)
                    {
                        if (fcm.Meeverlengen != NooitAltijdAanUitEnum.Nooit)
                        {
                            {
                                var fm = c.FileIngrepen.FirstOrDefault(
                                    x => x.TeDoserenSignaalGroepen.Any(x2 => x2.FaseCyclus == fcm.Naam));
                                if (fm != null)
                                {
                                    if (!yes)
                                    {
                                        yes = true;
                                        sb.AppendLine();
                                        sb.AppendLine($"{ts}/* Niet alternatief komen tijdens file */");
                                    }
                                    sb.AppendLine(
                                        $"{ts}if (IH[{_hpf}{_hfile}{fm.Naam}]) PAR[{_fcpf}{fcm.Naam}] = FALSE;");
                                }
                            }
                        }
                    }

                    return sb.ToString();

                default:
                    return null;
            }
        }

        public override bool SetSettings(CCOLGeneratorClassWithSettingsModel settings)
        {
            _mperiod = CCOLGeneratorSettingsProvider.Default.GetElementName("mperiod");

            return base.SetSettings(settings);
        }
    }
}
