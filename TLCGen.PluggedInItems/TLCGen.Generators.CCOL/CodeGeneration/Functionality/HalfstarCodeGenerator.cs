﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLCGen.Extensions;
using TLCGen.Models;
using TLCGen.Generators.Shared;
using TLCGen.Models.Enumerations;
using TLCGen.Settings;

namespace TLCGen.Generators.CCOL.CodeGeneration.Functionality
{
    [CCOLCodePieceGenerator]
	public class HalfstarCodeGenerator : CCOLCodePieceGeneratorBase
	{
		private string _mperiod;
		private string _cvc;
		private string _schmv;
		private string _schwg;
		private string _schca;
        private string _tnlsg;
		private string _tnlsgd;
		private string _tnlcv;
		private string _tnlcvd;
		private string _tnleg;
		private string _tnlegd;
		private string _huks;
		private string _hiks;
        private string _prmxnl;
        private string _hnla;
        private string _hprioin;
        private string _hpriouit;

#pragma warning disable 0649
        private CCOLGeneratorCodeStringSettingModel _usmlact;
		private CCOLGeneratorCodeStringSettingModel _usplact;
		private CCOLGeneratorCodeStringSettingModel _uskpact;
		private CCOLGeneratorCodeStringSettingModel _usmlpl;
		private CCOLGeneratorCodeStringSettingModel _ustxtimer;
		private CCOLGeneratorCodeStringSettingModel _usmaster;
		private CCOLGeneratorCodeStringSettingModel _usslave;
		private CCOLGeneratorCodeStringSettingModel _usklok;
		private CCOLGeneratorCodeStringSettingModel _ushand;
		private CCOLGeneratorCodeStringSettingModel _usleven;
		private CCOLGeneratorCodeStringSettingModel _ussyncok;
		private CCOLGeneratorCodeStringSettingModel _ustxsok;
		private CCOLGeneratorCodeStringSettingModel _uskpuls;
		private CCOLGeneratorCodeStringSettingModel _uspervar;
		private CCOLGeneratorCodeStringSettingModel _usperarh;

		private CCOLGeneratorCodeStringSettingModel _mklok;
		private CCOLGeneratorCodeStringSettingModel _mhand;
		private CCOLGeneratorCodeStringSettingModel _mmaster;
		private CCOLGeneratorCodeStringSettingModel _mslave;
		private CCOLGeneratorCodeStringSettingModel _mleven;
		private CCOLGeneratorCodeStringSettingModel _hkpact;
		private CCOLGeneratorCodeStringSettingModel _hplact;
		private CCOLGeneratorCodeStringSettingModel _hmlact;
		private CCOLGeneratorCodeStringSettingModel _schvar;
		private CCOLGeneratorCodeStringSettingModel _scharh;
		private CCOLGeneratorCodeStringSettingModel _schpervar;
		private CCOLGeneratorCodeStringSettingModel _schslavebep;
		private CCOLGeneratorCodeStringSettingModel _hpervar;
		private CCOLGeneratorCodeStringSettingModel _schperarh;
		private CCOLGeneratorCodeStringSettingModel _hperarh;
		private CCOLGeneratorCodeStringSettingModel _schvarstreng;
		private CCOLGeneratorCodeStringSettingModel _schvaml;
		private CCOLGeneratorCodeStringSettingModel _hplhd;
		private CCOLGeneratorCodeStringSettingModel _schovpriople;
		private CCOLGeneratorCodeStringSettingModel _prmplxper;
		private CCOLGeneratorCodeStringSettingModel _prmtx;
		private CCOLGeneratorCodeStringSettingModel _hxpl;
		private CCOLGeneratorCodeStringSettingModel _schinst;
		private CCOLGeneratorCodeStringSettingModel _schinstprm;
        private CCOLGeneratorCodeStringSettingModel _homschtegenh;
		private CCOLGeneratorCodeStringSettingModel _prmrstotxa;
		private CCOLGeneratorCodeStringSettingModel _tleven;
		private CCOLGeneratorCodeStringSettingModel _hleven;
		private CCOLGeneratorCodeStringSettingModel _prmvolgmasterpl;
		private CCOLGeneratorCodeStringSettingModel _toffset;
		private CCOLGeneratorCodeStringSettingModel _txmarge;
		private CCOLGeneratorCodeStringSettingModel _uspl;

        private CCOLGeneratorCodeStringSettingModel _prmaltphst;
        private CCOLGeneratorCodeStringSettingModel _schaltghst;

        private CCOLGeneratorCodeStringSettingModel _cvchst;
        private CCOLGeneratorCodeStringSettingModel _prmpriohst;

        private CCOLGeneratorCodeStringSettingModel _schtegenov;
        private CCOLGeneratorCodeStringSettingModel _schafkwgov;
        private CCOLGeneratorCodeStringSettingModel _schafkvgov;

        private CCOLGeneratorCodeStringSettingModel _prmnatxdhst;

#pragma warning restore 0649

        public void CollectKoppelSignalen(ControllerModel c)
        {
            var _myKoppelSignalen = new List<CCOLKoppelSignaal>();

        }

        public override void CollectCCOLElements(ControllerModel c, ICCOLGeneratorSettingsProvider settingsProvider = null)
		{
			_myElements = new List<CCOLElement>();
			_myBitmapOutputs = new List<CCOLIOElement>();

			if (c.HalfstarData.IsHalfstar)
			{
				var hsd = c.HalfstarData;

                var gelijkstarttuples = CCOLCodeHelper.GetFasenWithGelijkStarts(c);
                if (c.ModuleMolen.LangstWachtendeAlternatief)
                {
                    foreach (var fc in hsd.FaseCyclusInstellingen)
                    {
                        var hasgs = gelijkstarttuples.FirstOrDefault(x => x.Item1 == fc.FaseCyclus && x.Item2.Count > 1);
                        if (hasgs != null)
                        {
                            var namealtphst = _prmaltphst + string.Join(string.Empty, hasgs.Item2);
                            if (!_myElements.Any(i => i.Naam == namealtphst))
                            _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{namealtphst}", fc.AlternatieveRuimte, CCOLElementTimeTypeEnum.TE_type, _prmaltphst, "fasen", string.Join(", ", hasgs.Item2)));
                        }
                        else
                        {
                            _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_prmaltphst}{fc.FaseCyclus}", fc.AlternatieveRuimte, CCOLElementTimeTypeEnum.TE_type, _prmaltphst, "fase", fc.FaseCyclus));
                        }
                    }
                    foreach (var fc in hsd.FaseCyclusInstellingen)
                    {
                        var hasgs = gelijkstarttuples.FirstOrDefault(x => x.Item1 == fc.FaseCyclus && x.Item2.Count > 1);
                        if (hasgs != null)
                        {
                            var namealtghst = _schaltghst + string.Join(string.Empty, hasgs.Item2);
                            if (!_myElements.Any(i => i.Naam == namealtghst && i.Type == CCOLElementTypeEnum.Schakelaar))
                                _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{namealtghst}", fc.AlternatiefToestaan ? 1 : 0, CCOLElementTimeTypeEnum.SCH_type, _schaltghst, "fasen", string.Join(", ", hasgs.Item2)));
                        }
                        else
                        {
                            _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schaltghst}{fc.FaseCyclus}", fc.AlternatiefToestaan ? 1 : 0, CCOLElementTimeTypeEnum.SCH_type, _schaltghst, "fase", fc.FaseCyclus));
                        }
                    }
                }

                foreach(var hr in c.HalfstarData.Hoofdrichtingen)
                {
				    _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schtegenov}{hr.FaseCyclus}", hr.Tegenhouden ? 1 : 0, CCOLElementTimeTypeEnum.SCH_type, _schtegenov, hr.FaseCyclus));
				    _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schafkwgov}{hr.FaseCyclus}", hr.AfkappenWG ? 1 : 0, CCOLElementTimeTypeEnum.SCH_type, _schafkwgov, hr.FaseCyclus));
				    _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schafkvgov}{hr.FaseCyclus}", hr.AfkappenVG ? 1 : 0, CCOLElementTimeTypeEnum.SCH_type, _schafkvgov, hr.FaseCyclus));
                }

                if (c.PrioData.PrioIngrepen.Any())
                {
                    foreach (var prio in c.PrioData.PrioIngrepen) _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_cvchst}{CCOLCodeHelper.GetPriorityName(prio)}", 999, CCOLElementTimeTypeEnum.CT_type, _cvchst, prio.FaseCyclus, prio.Type.GetDescription()));
                    foreach (var prio in c.PrioData.PrioIngrepen) _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_prmpriohst}{CCOLCodeHelper.GetPriorityName(prio)}", prio.HalfstarIngreepData.Prioriteit, CCOLElementTimeTypeEnum.None, _prmpriohst, prio.FaseCyclus, prio.Type.GetDescription()));
                    foreach (var prio in c.PrioData.PrioIngrepen) _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_prmnatxdhst}{CCOLCodeHelper.GetPriorityName(prio)}", prio.HalfstarIngreepData.GroenNaTXDTijd, CCOLElementTimeTypeEnum.TE_type, _prmnatxdhst, prio.FaseCyclus, prio.Type.GetDescription()));
                }
                if (c.PrioData.HDIngrepen.Any())
                {
                    _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_hplhd}", _hplhd));
                }

                _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_usplact}", _usplact));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_uskpact}", _uskpact));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_usmlact}", _usmlact));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_usmlpl}", _usmlpl));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_ustxtimer}", _ustxtimer));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_usklok}", _usklok));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_ushand}", _ushand));

                if (c.HalfstarData.PlantijdenInParameters)
                {
                    _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schinstprm}", 0, CCOLElementTimeTypeEnum.SCH_type, _schinstprm));
                }

                foreach (var pl in c.HalfstarData.SignaalPlannen)
                {
                    _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_uspl}{pl.Naam}", _uspl, pl.Naam));
                    _myBitmapOutputs.Add(new CCOLIOElement(pl, $"{_uspf}{_uspl}{pl.Naam}"));

                    foreach(var fcpl in pl.Fasen)
                    {
                        if (fcpl.B2.HasValue && fcpl.D2.HasValue || c.HalfstarData.PlantijdenInParameters)
                        {
                            var times = new []{ fcpl.A1, fcpl.B1, fcpl.C1, fcpl.D1, fcpl.E1, fcpl.A2, fcpl.B2, fcpl.C2, fcpl.D2, fcpl.E2 };
                            var moments = new [] { "A", "B", "C", "D", "E", "A", "B", "C", "D", "E" };
                            var realisation = 1;
                            for (var i = 0; i < 10; ++i)
                            {
                                if (i == 5) realisation = 2;
                                _myElements.Add(new CCOLElement(
                                    $"{_prmtx}{moments[i]}{realisation}{pl.Naam}_{fcpl.FaseCyclus}",
                                    times[i] ?? 0,
                                    CCOLElementTimeTypeEnum.None,
                                    CCOLElementTypeEnum.Parameter,
									CCOLGeneratorSettingsProvider.Default,
                                    CCOLGeneratorSettingsProvider.Default.GetElementDescription(_prmtx.Description, CCOLElementTypeEnum.Parameter, realisation == 1 ? "Eerste" : "Tweede", pl.Naam, fcpl.FaseCyclus, moments[i])));
                            }
                        }
                    }
                }

                _myBitmapOutputs.Add(new CCOLIOElement(hsd.PlActUitgang, $"{_uspf}{_usplact}"));
				_myBitmapOutputs.Add(new CCOLIOElement(hsd.MlActUitgang, $"{_uspf}{_usmlact}"));
				_myBitmapOutputs.Add(new CCOLIOElement(hsd.KpActUitgang, $"{_uspf}{_uskpact}"));
				_myBitmapOutputs.Add(new CCOLIOElement(hsd.MlPlUitgang, $"{_uspf}{_usmlpl}"));
				_myBitmapOutputs.Add(new CCOLIOElement(hsd.TxTimerUitgang, $"{_uspf}{_ustxtimer}"));
				_myBitmapOutputs.Add(new CCOLIOElement(hsd.KlokUitgang, $"{_uspf}{_usklok}"));
				_myBitmapOutputs.Add(new CCOLIOElement(hsd.HandUitgang, $"{_uspf}{_ushand}"));
				
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_hplact}", _hplact));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_hkpact}", _hkpact));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_hmlact}", _hmlact));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_hpervar}", _hpervar));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_hperarh}", _hperarh));
				
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_homschtegenh}", _homschtegenh));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_prmrstotxa}", 50, CCOLElementTimeTypeEnum.TE_type, _prmrstotxa));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schinst}", 0, CCOLElementTimeTypeEnum.SCH_type, _schinst));
				
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_tleven}", 10, CCOLElementTimeTypeEnum.TE_type, _tleven));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_mleven}", _mleven));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_hleven}", _hleven));
				
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_mklok}", _mklok));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_mhand}", _mhand));
				if (c.HalfstarData.Type != HalfstarTypeEnum.Master)
				{
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_usmaster}", _usmaster));
					_myBitmapOutputs.Add(new CCOLIOElement(hsd.MasterUitgang, $"{_uspf}{_usmaster}"));

                    _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_usslave}", _usslave));
                    _myBitmapOutputs.Add(new CCOLIOElement(hsd.SlaveUitgang, $"{_uspf}{_usslave}"));

                    _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_mmaster}", _mmaster));
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_mslave}", _mslave));
					
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schslavebep}", 0, CCOLElementTimeTypeEnum.SCH_type, _schslavebep));
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_prmvolgmasterpl}", 65535, CCOLElementTimeTypeEnum.None, _prmvolgmasterpl));
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_toffset}", 0, CCOLElementTimeTypeEnum.TS_type, _toffset));
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_txmarge}", 2, CCOLElementTimeTypeEnum.TS_type, _txmarge));
				}
				
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schvaml}", hsd.TypeVARegelen == HalfstarVARegelenTypeEnum.ML ? 1 : 0, CCOLElementTimeTypeEnum.SCH_type, _schvaml));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schvar}", hsd.VARegelen ? 1 : 0, CCOLElementTimeTypeEnum.SCH_type, _schvar));
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_scharh}", hsd.AlternatievenVoorHoofdrichtingen ? 1 : 0 , CCOLElementTimeTypeEnum.SCH_type, _scharh));
				if (hsd.Type != HalfstarTypeEnum.Slave)
				{
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schvarstreng}", 0, CCOLElementTimeTypeEnum.SCH_type, _schvarstreng));
				}

				var iplx = 0;
				for (var index = 0; index < hsd.SignaalPlannen.Count; index++)
				{
					var pl = hsd.SignaalPlannen[index];
					if (hsd.DefaultPeriodeSignaalplan == pl.Naam)
					{
						iplx = index + 1;
						break;
					}
				}
				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_prmplxper}def", iplx, CCOLElementTimeTypeEnum.None, _prmplxper, "default"));
				var iper = 1;
				foreach (var per in hsd.HalfstarPeriodenData)
				{
					for (var index = 0; index < hsd.SignaalPlannen.Count; index++)
					{
						var pl = hsd.SignaalPlannen[index];
						if (per.Signaalplan == pl.Naam)
						{
							iplx = index + 1;
							break;
						}
					}
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_prmplxper}{(c.PeriodenData.GebruikPeriodenNamen ? per.Periode : iper.ToString())}", iplx, CCOLElementTimeTypeEnum.None, _prmplxper, per.Periode));
					++iper;
				}

				foreach (var k in hsd.GekoppeldeKruisingen)
				{
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_tleven}{k.KruisingNaam}", 30, CCOLElementTimeTypeEnum.TE_type, _tleven, k.KruisingNaam));
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_mleven}{k.KruisingNaam}", _mleven, k.KruisingNaam));
					if(k.Type == HalfstarGekoppeldTypeEnum.Master)
					{
					
                        _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"in{k.KruisingNaam}{_usleven}", _usleven, k.KruisingNaam, "in"));
						_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"in{k.KruisingNaam}{_uskpuls}", _uskpuls, k.KruisingNaam, "in"));
						_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"in{k.KruisingNaam}{_uspervar}", _uspervar, k.KruisingNaam, "in"));
						_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"in{k.KruisingNaam}{_usperarh}", _usperarh, k.KruisingNaam, "in"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.InLeven, $"{_uspf}in{k.KruisingNaam}{_usleven}"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.InKoppelpuls, $"{_uspf}in{k.KruisingNaam}{_uskpuls}"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.InPeriodeVARegelen, $"{_uspf}in{k.KruisingNaam}{_uspervar}"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.InPeriodenAlternatievenHoofdrichtingen, $"{_uspf}in{k.KruisingNaam}{_usperarh}"));
                        foreach (var pl in hsd.SignaalPlannen)
						{
                        	_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"in{k.KruisingNaam}{pl.Naam}", _uspl, pl.Naam, k.KruisingNaam, "in"));
						}
						foreach (var pl in k.PlanIngangen)
						{
							_myBitmapOutputs.Add(new CCOLIOElement(pl, $"{_uspf}in{k.KruisingNaam}{pl.Plan}"));
						}
                        _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"uit{k.KruisingNaam}{_usleven}", _usleven, k.KruisingNaam, "uit"));
						_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"uit{k.KruisingNaam}{_ussyncok}", _ussyncok, k.KruisingNaam, "uit"));
						_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"uit{k.KruisingNaam}{_ustxsok}", _ustxsok, k.KruisingNaam, "uit"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.UitLeven, $"{_uspf}uit{k.KruisingNaam}{_usleven}"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.UitSynchronisatieOk, $"{_uspf}uit{k.KruisingNaam}{_ussyncok}"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.UitTxsOk, $"{_uspf}uit{k.KruisingNaam}{_ustxsok}"));
					}
					if (k.Type == HalfstarGekoppeldTypeEnum.Slave)
					{
                        _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"uit{k.KruisingNaam}{_usleven}", _usleven, k.KruisingNaam, "uit"));
						_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"uit{k.KruisingNaam}{_uskpuls}", _uskpuls, k.KruisingNaam, "uit"));
						_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"uit{k.KruisingNaam}{_uspervar}", _uspervar, k.KruisingNaam, "uit"));
						_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"uit{k.KruisingNaam}{_usperarh}", _usperarh, k.KruisingNaam, "uit"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.UitLeven, $"{_uspf}uit{k.KruisingNaam}{_usleven}"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.UitKoppelpuls, $"{_uspf}uit{k.KruisingNaam}{_uskpuls}"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.UitPeriodeVARegelen, $"{_uspf}uit{k.KruisingNaam}{_uspervar}"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.UitPeriodenAlternatievenHoofdrichtingen, $"{_uspf}uit{k.KruisingNaam}{_usperarh}"));
						foreach (var pl in hsd.SignaalPlannen)
						{
                        	_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"uit{k.KruisingNaam}{pl.Naam}", _uspl, pl.Naam, k.KruisingNaam, "uit"));
						}
						foreach (var pl in k.PlanUitgangen)
						{
							_myBitmapOutputs.Add(new CCOLIOElement(pl, $"{_uspf}uit{k.KruisingNaam}{pl.Plan}"));
						}
                        _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"in{k.KruisingNaam}{_usleven}", _usleven, k.KruisingNaam, "in"));
						_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"in{k.KruisingNaam}{_ussyncok}", _ussyncok, k.KruisingNaam, "in"));
						_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"in{k.KruisingNaam}{_ustxsok}", _ustxsok, k.KruisingNaam, "in"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.InLeven, $"{_uspf}in{k.KruisingNaam}{_usleven}"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.InSynchronisatieOk, $"{_uspf}in{k.KruisingNaam}{_ussyncok}"));
						_myBitmapOutputs.Add(new CCOLIOElement(k.InTxsOk, $"{_uspf}in{k.KruisingNaam}{_ustxsok}"));
					}
                    var signals = ((IHaveKoppelSignalen)k).UpdateKoppelSignalen();
                    foreach (var s in signals)
                    {
                        CCOLElementCollector.AddKoppelSignaal(k.PTPKruising, s.Count, s.Name, s.Richting);
                    }
                }

                _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schpervar}def", hsd.DefaultPeriodeVARegelen ? 1 : 0, CCOLElementTimeTypeEnum.SCH_type, _schpervar, "default"));
				iper = 1;
				foreach (var per in hsd.HalfstarPeriodenData)
				{
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schpervar}{(c.PeriodenData.GebruikPeriodenNamen ? per.Periode : iper.ToString())}", 
                        per.VARegelen ? 1 : 0, 
                        CCOLElementTimeTypeEnum.SCH_type, 
                        _schpervar, 
                        per.Periode));
					++iper;
				}

				_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schperarh}def", hsd.DefaultPeriodeAlternatievenVoorHoofdrichtingen ? 1 : 0, CCOLElementTimeTypeEnum.SCH_type, _schperarh, "default"));
				iper = 1;
				foreach (var per in hsd.HalfstarPeriodenData)
				{
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schperarh}{(c.PeriodenData.GebruikPeriodenNamen ? per.Periode : iper.ToString())}", per.AlternatievenVoorHoofdrichtingen ? 1 : 0, CCOLElementTimeTypeEnum.SCH_type, _schperarh, per.Periode));
					++iper;
				}

				if (c.PrioData.PrioIngreepType != PrioIngreepTypeEnum.Geen)
				{
					_myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement($"{_schovpriople}", hsd.OVPrioriteitPL ? 1 : 0, CCOLElementTimeTypeEnum.SCH_type, _schovpriople));
				}

                if (c.InterSignaalGroep.Gelijkstarten.Any())
                {
                    var added = new List<string>();
                    foreach (var gs in gelijkstarttuples)
                    {
                        var hxpl = _hxpl + string.Join(string.Empty, gs.Item2);
                        if (!added.Contains(hxpl))
                        {
                            added.Add(hxpl);
					        _myElements.Add(CCOLGeneratorSettingsProvider.Default.CreateElement(hxpl, _hxpl, string.Join(" ", gs.Item2)));
                        }
                    }
                }
			}
			
		}

		public override bool HasCCOLElements() => true;
		
		public override bool HasCCOLBitmapOutputs() => true;

        public override bool HasFunctionLocalVariables() => true;

        public override IEnumerable<Tuple<string, string, string>> GetFunctionLocalVariables(ControllerModel c, CCOLCodeTypeEnum type)
        {
            if (!c.HalfstarData.IsHalfstar) return base.GetFunctionLocalVariables(c, type);
            return type switch
            {
                CCOLCodeTypeEnum.HstCAanvragen => new List<Tuple<string, string, string>> {new Tuple<string, string, string>("int", "fc", "")},
                CCOLCodeTypeEnum.HstCVerlenggroen => new List<Tuple<string, string, string>> {new Tuple<string, string, string>("int", "fc", "")},
                CCOLCodeTypeEnum.HstCMaxgroen => new List<Tuple<string, string, string>> {new Tuple<string, string, string>("int", "fc", "")},
                CCOLCodeTypeEnum.HstCMeetkriterium => new List<Tuple<string, string, string>> {new Tuple<string, string, string>("int", "fc", "")},
                CCOLCodeTypeEnum.HstCMeeverlengen => new List<Tuple<string, string, string>> {new Tuple<string, string, string>("int", "fc", "")},
                CCOLCodeTypeEnum.HstCSynchronisaties => new List<Tuple<string, string, string>> {new Tuple<string, string, string>("int", "fc", "")},
                CCOLCodeTypeEnum.HstCRealisatieAfhandeling => new List<Tuple<string, string, string>> {new Tuple<string, string, string>("int", "fc", "")},
                CCOLCodeTypeEnum.HstCPreSystemApplication => new List<Tuple<string, string, string>> {new Tuple<string, string, string>("int", "fc", "")},
                CCOLCodeTypeEnum.PrioCPrioriteitsOpties => new List<Tuple<string, string, string>> {new Tuple<string, string, string>("int", "fc", "")},
                CCOLCodeTypeEnum.PrioCPostAfhandelingPrio => new List<Tuple<string, string, string>> {new Tuple<string, string, string>("int", "fc", "")},
                CCOLCodeTypeEnum.HstCAlternatief => new List<Tuple<string, string, string>> {new Tuple<string, string, string>("int", "ov", ""), new Tuple<string, string, string>("int", "fc", "")},
                CCOLCodeTypeEnum.HstCKlokPerioden => new List<Tuple<string, string, string>>
                {
                    //new Tuple<string, string, string>($"{c.GetBoolV()}", "omschakelmag", "FALSE"),
                    new Tuple<string, string, string>("char", "volgMaster", "TRUE")
                },
                _ => base.GetFunctionLocalVariables(c, type)
            };
        }

        public override int HasCode(CCOLCodeTypeEnum type)
        {
            return type switch
            {
                CCOLCodeTypeEnum.RegCPreApplication => 20,
                CCOLCodeTypeEnum.HstCPreApplication => 10,
                CCOLCodeTypeEnum.HstCKlokPerioden => 10,
                CCOLCodeTypeEnum.HstCAanvragen => 10,
                CCOLCodeTypeEnum.HstCVerlenggroen => 10,
                CCOLCodeTypeEnum.HstCMaxgroen => 10,
                CCOLCodeTypeEnum.HstCWachtgroen => 10,
                CCOLCodeTypeEnum.HstCMeetkriterium => 10,
                CCOLCodeTypeEnum.HstCMeeverlengen => 10,
                CCOLCodeTypeEnum.HstCSynchronisaties => 10,
                CCOLCodeTypeEnum.HstCAlternatief => 20,
                CCOLCodeTypeEnum.HstCRealisatieAfhandeling => 10,
                CCOLCodeTypeEnum.HstCPostApplication => 10,
                CCOLCodeTypeEnum.HstCPreSystemApplication => 10,
                CCOLCodeTypeEnum.HstCPostSystemApplication => 10,
                CCOLCodeTypeEnum.HstCPostDumpApplication => 10,
                CCOLCodeTypeEnum.HstCPrioHalfstarSettings => 10,
                CCOLCodeTypeEnum.PrioCInitPrio => 10,
                CCOLCodeTypeEnum.PrioCInstellingen => 10,
                CCOLCodeTypeEnum.PrioCPrioriteitsOpties => 20,
                CCOLCodeTypeEnum.PrioCOnderMaximum => 10,
                CCOLCodeTypeEnum.PrioCAfkapGroen => 10,
                CCOLCodeTypeEnum.PrioCStartGroenMomenten => 10,
                CCOLCodeTypeEnum.PrioCTegenhoudenConflicten => 10,
                CCOLCodeTypeEnum.PrioCAfkappen => 10,
                CCOLCodeTypeEnum.PrioCTerugkomGroen => 10,
                CCOLCodeTypeEnum.PrioCGroenVasthouden => 10,
                CCOLCodeTypeEnum.PrioCMeetkriterium => 10,
                _ => 0
            };
        }

		public override string GetCode(ControllerModel c, CCOLCodeTypeEnum type, string ts)
		{
			if (!c.HalfstarData.IsHalfstar || !c.HalfstarData.SignaalPlannen.Any())
			{
				return "";
			}

			var sb = new StringBuilder();
			var master = c.HalfstarData.GekoppeldeKruisingen.FirstOrDefault(x => x.IsMaster);

			if (c.HalfstarData.Type != HalfstarTypeEnum.Master && master == null)
			{
				return "";
			}

			switch (type)
			{
                #region reg.c

                case CCOLCodeTypeEnum.RegCPreApplication:
                    sb.AppendLine($"{ts}/* bepalen of regeling mag omschakelen */");
					sb.AppendLine($"{ts}IH[{_hpf}{_homschtegenh}] = FALSE;");
					return sb.ToString();

                #endregion // reg.c

                #region hst.c
                
                case CCOLCodeTypeEnum.HstCTop:
                    return sb.ToString();

				case CCOLCodeTypeEnum.HstCPreApplication:
					sb.AppendLine($"{ts}/* na omschakeling van PL -> VA, modules opnieuw initialiseren */");
					sb.AppendLine($"{ts}if (SH[{_hpf}{_hplact}] || EH[{_hpf}{_hplact}])");
					sb.AppendLine($"{ts}{{");
                    if (c.Data.MultiModuleReeksen)
                    {
                        foreach (var r in c.MultiModuleMolens.Where(x => x.Modules.Any(x2 => x2.Fasen.Any())))
                        {
                            sb.AppendLine($"{ts}{ts}init_modules({r.Reeks}_MAX, PR{r.Reeks}, Y{r.Reeks}, &{r.Reeks}, &S{r.Reeks});");
                        }
                    }
                    else
                    {
					    sb.AppendLine($"{ts}{ts}init_modules(ML_MAX, PRML, YML, &ML, &SML);");
                    }
					if (c.InterSignaalGroep.Gelijkstarten.Any() ||
					    c.InterSignaalGroep.Voorstarten.Any())
					{
						sb.AppendLine($"{ts}{ts}init_realisation_timers();");
						sb.AppendLine($"{ts}{ts}reset_realisation_timers();");
                    }
					sb.AppendLine($"{ts}{ts}sync_pg();");
					sb.AppendLine($"{ts}{ts}reset_fc_halfstar();");
					sb.AppendLine($"{ts}}}");
					sb.AppendLine();
                    sb.AppendLine($"{ts}if (IH[{_hpf}{_hkpact}])");
					sb.AppendLine($"{ts}{{");
                    sb.AppendLine($"{ts}{ts}/* bijhouden verlenggroentijden t.b.v. calculaties diverse functies */");
					sb.AppendLine($"{ts}{ts}tvga_timer_halfstar();");
					sb.AppendLine($"{ts}}}");
					//sb.AppendLine();
                    //if (c.OVData.OVIngreepType != OVIngreepTypeEnum.Geen)
					//{
					//	sb.AppendLine($"{ts}/* tbv PRIO_ple */");
					//	sb.AppendLine($"{ts}if (SCH[{_schpf}{_schovpriople}])");
					//	sb.AppendLine($"{ts}{{");
					//	sb.AppendLine($"{ts}{ts}/* Instellen OV parameters */");
					//	sb.AppendLine($"{ts}{ts}if (CIF_PARM1WIJZPB != CIF_GEEN_PARMWIJZ ||");
					//	sb.AppendLine($"{ts}{ts}    CIF_PARM1WIJZAP != CIF_GEEN_PARMWIJZ)");
					//	sb.AppendLine($"{ts}{ts}{{");
					//	sb.AppendLine($"{ts}{ts}{ts}PrioHalfstarSettings();");
					//	sb.AppendLine($"{ts}{ts}}}");
					//	sb.AppendLine($"{ts}{ts}{ts}");
					//	sb.AppendLine($"{ts}{ts}BijhoudenWachtTijd();");
					//	sb.AppendLine($"{ts}{ts}BijhoudenMinimumGroenTijden();");
					//	sb.AppendLine($"{ts}}}");
					//	sb.AppendLine();
					//}
					return sb.ToString();

				case CCOLCodeTypeEnum.HstCKlokPerioden:
					sb.AppendLine($"{ts}/* BepaalKoppeling */");
					sb.AppendLine($"{ts}/* --------------- */");

					#region Reset data

					sb.AppendLine($"{ts}MM[{_mpf}{_mklok}] = FALSE;");
					sb.AppendLine($"{ts}MM[{_mpf}{_mhand}] = FALSE;");
					if (c.HalfstarData.Type != HalfstarTypeEnum.Master)
					{
						sb.AppendLine($"{ts}MM[{_mpf}{_mmaster}] = FALSE;");
						sb.AppendLine($"{ts}MM[{_mpf}{_mslave}] = FALSE;");
                    }
					sb.AppendLine($"{ts}IH[{_hpf}{_hkpact}] = TRUE;");
					sb.AppendLine($"{ts}IH[{_hpf}{_hplact}] = TRUE;");
					sb.AppendLine($"{ts}IH[{_hpf}{_hmlact}] = FALSE;");
					sb.AppendLine($"{ts}APL = NG;");
					sb.AppendLine();

					#endregion // Reset data

					#region Bepalen PL/var/arh door master

					if (c.HalfstarData.Type != HalfstarTypeEnum.Master)
					{
						if (master != null)
						{
							sb.AppendLine($"/* Master bepaalt wat er gaat gebeuren */");
							sb.AppendLine($"{ts}if (MM[{_mpf}{_mleven}{master.KruisingNaam}] && !SCH[{_schpf}{_schslavebep}])");
							sb.AppendLine($"{ts}{{");
							sb.AppendLine($"{ts}{ts}MM[{_mpf}{_mmaster}] = TRUE;");
							sb.AppendLine();
							sb.Append($"{ts}{ts}if      ");
							var i = 0;
							foreach (var pl in c.HalfstarData.SignaalPlannen)
							{
								if (i > 0)
								{
									sb.AppendLine();
									sb.Append($"{ts}{ts}else if ");
								}
                                var ipl = CCOLElementCollector.GetKoppelSignaalCount(master.PTPKruising, $"{master.KruisingNaam}pl{pl.Naam}", KoppelSignaalRichtingEnum.In);
								sb.Append($"(IH[{_hpf}{master.PTPKruising}{_hiks}{ipl:00}]) APL = {pl.Naam};");
								++i;
							}
							sb.AppendLine();
							sb.AppendLine($"{ts}{ts}else APL = PL1;");
							sb.AppendLine();
							sb.AppendLine($"{ts}{ts}if (PRM[{_prmpf}{_prmvolgmasterpl}] > 0)");
							sb.AppendLine($"{ts}{ts}{{");
							i = 1;
							sb.Append($"{ts}{ts}{ts}if (");
							foreach (var pl in c.HalfstarData.SignaalPlannen)
							{
								if (i > 1)
								{
									sb.AppendLine(" ||");
								}
								sb.Append($"{ts}{ts}{ts}    (APL == {pl.Naam}) && !(PRM[{_prmpf}{_prmvolgmasterpl}] & BIT{i})");
								++i;
							}
							sb.AppendLine($")");
							sb.AppendLine($"{ts}{ts}{ts}{{");
							sb.AppendLine($"{ts}{ts}{ts}{ts}volgMaster = FALSE;");
							sb.AppendLine($"{ts}{ts}{ts}}}");
							sb.AppendLine($"{ts}{ts}}}");
							sb.AppendLine($"{ts}{ts}if (volgMaster == FALSE)");
							sb.AppendLine($"{ts}{ts}{{");
							sb.AppendLine($"{ts}{ts}{ts}IH[{_hpf}{_hkpact}] = FALSE;");
							sb.AppendLine($"{ts}{ts}}}");
							sb.AppendLine($"{ts}{ts}else");
							sb.AppendLine($"{ts}{ts}{{");
                            var ipl2 = CCOLElementCollector.GetKoppelSignaalCount(master.PTPKruising, $"{master.KruisingNaam}pervar", KoppelSignaalRichtingEnum.In);
                            sb.AppendLine($"{ts}{ts}{ts}IH[{_hpf}{_hpervar}] =  IH[{_hpf}{master.PTPKruising}{_hiks}{ipl2:00}];");
                            ipl2 = CCOLElementCollector.GetKoppelSignaalCount(master.PTPKruising, $"{master.KruisingNaam}perarh", KoppelSignaalRichtingEnum.In);
							sb.AppendLine($"{ts}{ts}{ts}IH[{_hpf}{_hperarh}] =  IH[{_hpf}{master.PTPKruising}{_hiks}{ipl2:00}];");
							sb.AppendLine($"{ts}{ts}}}");
							sb.AppendLine($"{ts}}}");
						}
					}

					#endregion // Bepalen PL/var/arh door master

					#region Zelf bepalen PL/var/arh
					
					var mts = c.HalfstarData.Type == HalfstarTypeEnum.Master ? ts : ts + ts;

					if (c.HalfstarData.Type != HalfstarTypeEnum.Master)
					{
						sb.AppendLine(
							$"{ts}/* Bij afwezigheid Master bepaalt Slave zelf wat er gaat gebeuren. In dit geval neemt de slave de functie van Master over */");
						sb.AppendLine($"{ts}else");
						sb.AppendLine($"{ts}{{");
						sb.AppendLine($"{mts}MM[{_mpf}{_mslave}] = TRUE;");
					}

					sb.AppendLine($"{mts}switch (MM[{_mpf}{_mperiod}])");
					sb.AppendLine($"{mts}{{");
					sb.AppendLine($"{mts}{ts}case 0: /* default */");
					sb.AppendLine($"{mts}{ts}{{");
					sb.AppendLine($"{mts}{ts}{ts}APL = PRM[{_prmpf}{_prmplxper}def] - 1;");
					sb.AppendLine($"{mts}{ts}{ts}break;");
					sb.AppendLine($"{mts}{ts}}}");
					var iper = 1;
					foreach (var per in c.HalfstarData.HalfstarPeriodenData)
					{
						sb.AppendLine($"{mts}{ts}case {iper}: /* default */");
						sb.AppendLine($"{mts}{ts}{{");
						sb.AppendLine($"{mts}{ts}{ts}APL = PRM[{_prmpf}{_prmplxper}{(c.PeriodenData.GebruikPeriodenNamen ? per.Periode : iper.ToString())}] - 1;");
						sb.AppendLine($"{mts}{ts}{ts}break;");
						sb.AppendLine($"{mts}{ts}}}");
						++iper;
					}
					sb.AppendLine($"{mts}{ts}default:");
					sb.AppendLine($"{mts}{ts}{{");
					sb.AppendLine($"{mts}{ts}{ts}APL = PRM[{_prmpf}{_prmplxper}def] - 1;");
					sb.AppendLine($"{mts}{ts}{ts}break;");
					sb.AppendLine($"{mts}{ts}}}");
					sb.AppendLine($"{mts}}}");

					#region Klok bepaling VA bedrijf

					sb.AppendLine($"{mts}/* Klokbepaling voor VA-bedrijf */");
					sb.AppendLine($"{mts}if (!IH[{_hpf}{_homschtegenh}])");
					sb.AppendLine($"{mts}{{");
					sb.Append($"{mts}{ts}if ((SCH[{_schpf}{_schpervar}def] && (MM[{_mpf}{_mperiod}] == 0)");
					iper = 1;
					foreach (var per in c.HalfstarData.HalfstarPeriodenData)
					{
						sb.AppendLine(") ||");
						sb.Append($"{mts}{ts}    (SCH[{_schpf}{_schpervar}{(c.PeriodenData.GebruikPeriodenNamen ? per.Periode : iper.ToString())}] && (MM[{_mpf}{_mperiod}] == {iper})");
						++iper;
					}
					sb.AppendLine("))");
					sb.AppendLine($"{mts}{ts}{{");
					sb.AppendLine($"{mts}{ts}{ts}IH[{_hpf}{_hpervar}] = TRUE;");
					sb.AppendLine($"{mts}{ts}}}");
					sb.AppendLine($"{mts}{ts}else");
					sb.AppendLine($"{mts}{ts}{{");
					sb.AppendLine($"{mts}{ts}{ts}IH[{_hpf}{_hpervar}] = FALSE;");
					sb.AppendLine($"{mts}{ts}}}");
					sb.AppendLine($"{mts}}}");
					sb.AppendLine();

					#endregion // Klok bepaling VA bedrijf

					#region Klok bepaling alternatieven hoofdrichtingen

					sb.AppendLine($"{mts}/* Klokbepaling voor alternatieve realisaties voor de hoofdrichtingen */");
					sb.Append($"{mts}if ((SCH[{_schpf}{_schperarh}def] && (MM[{_mpf}{_mperiod}] == 0)");
					iper = 1;
					foreach (var per in c.HalfstarData.HalfstarPeriodenData)
					{
						sb.AppendLine(") ||");
						sb.Append($"{mts}    (SCH[{_schpf}{_schperarh}{(c.PeriodenData.GebruikPeriodenNamen ? per.Periode : iper.ToString())}] && (MM[{_mpf}{_mperiod}] == {iper})");
						++iper;
					}
					sb.AppendLine("))");
					sb.AppendLine($"{mts}{{");
					sb.AppendLine($"{mts}{ts}IH[{_hpf}{_hperarh}] = TRUE;");
					sb.AppendLine($"{mts}}}");
					sb.AppendLine($"{mts}else");
					sb.AppendLine($"{mts}{{");
					sb.AppendLine($"{mts}{ts}IH[{_hpf}{_hperarh}] = FALSE;");
					sb.AppendLine($"{mts}}}");
					
					#endregion // Bepalen alternatieven hoofdrichtingen

					if (c.HalfstarData.Type != HalfstarTypeEnum.Master)
					{
						sb.AppendLine($"{ts}}}");
					}
					sb.AppendLine();

					#endregion // Zelf bepalen PL/var/arh

					#region Bepalen VA bedrijf schakelaar

					sb.AppendLine($"{ts}/* Klokbepaling voor VA-bedrijf */");
					if (c.HalfstarData.Type == HalfstarTypeEnum.Slave)
					{
						sb.AppendLine($"{ts}if (SCH[{_schpf}{_schvar}])");
					}
					else
					{
						sb.AppendLine($"{ts}if (SCH[{_schpf}{_schvar}] || SCH[{_schpf}{_schvarstreng}])");
					}
					sb.AppendLine($"{ts}{{");
					sb.AppendLine($"{ts}{ts}/* Halfstar/va afhankelijk van schakelaar */");
					sb.AppendLine($"{ts}{ts}IH[{_hpf}{_hkpact}] = FALSE;");
					sb.AppendLine($"{ts}{ts}MM[{_mpf}{_mhand}]  = TRUE;");
					if (c.HalfstarData.Type != HalfstarTypeEnum.Master)
					{
						sb.AppendLine($"{ts}{ts}MM[{_mpf}{_mmaster}]  = FALSE;");
						sb.AppendLine($"{ts}{ts}MM[{_mpf}{_mslave}]  = FALSE;");
					}
					sb.AppendLine($"{ts}}} ");
					sb.AppendLine($"{ts}else");
					sb.AppendLine($"{ts}{{");
					sb.AppendLine($"{ts}{ts}MM[{_mpf}{_mklok}] = TRUE;");
					sb.AppendLine($"{ts}}}");
					sb.AppendLine();

					#endregion // Bepalen VA bedrijf schakelaar

					#region Bepalen alternatieven hoofdrichtingen schakelaar

					sb.AppendLine($"{ts}/* Toestaan alternatief hoofdrichtingen ook mogelijk met schakelaar */");
					sb.AppendLine($"{ts}if (SCH[{_schpf}{_scharh}])");
					sb.AppendLine($"{ts}{{");
					sb.AppendLine($"{ts}{ts}IH[{_hpf}{_hperarh}] = TRUE;");
					sb.AppendLine($"{ts}{ts}MM[{_mpf}{_mhand}]   = TRUE;");
					sb.AppendLine($"{ts}}}");

					#endregion // Bepalen alternatieven hoofdrichtingen schakelaar

					sb.AppendLine($"{ts}/* Koppelen actief */");
					switch (c.HalfstarData.Type)
					{
						case HalfstarTypeEnum.Master:
							sb.AppendLine($"{ts}if (H[{_hpf}{_hpervar}] || SCH[{_schpf}{_schvar}] || SCH[{_schpf}{_schvarstreng}])");
							break;
						case HalfstarTypeEnum.FallbackMaster:
							sb.AppendLine($"{ts}if (H[{_hpf}{_hpervar}] || SCH[{_schpf}{_schvar}] || SCH[{_schpf}{_schvarstreng}])");
							break;
						case HalfstarTypeEnum.Slave:
							sb.AppendLine($"{ts}if (H[{_hpf}{_hpervar}] || SCH[{_schpf}{_schvar}])");
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					sb.AppendLine($"{ts}{ts}IH[{_hpf}{_hkpact}] = FALSE;");
					sb.AppendLine($"{ts}else");
					sb.AppendLine($"{ts}{ts}IH[{_hpf}{_hkpact}] = TRUE;");

					sb.AppendLine($"{ts}/* Indien VA-bedrijf, dan met schakelaar te bepalen of dit in ML-bedrijf of in versneld PL-bedrijf gebeurt */");
					sb.AppendLine($"{ts}if (!IH[{_hpf}{_hkpact}])");
					sb.AppendLine($"{ts}{{");
					sb.AppendLine($"{ts}{ts}if (SCH[{_schpf}{_schvaml}] || (APL == NG))");
					sb.AppendLine($"{ts}{ts}{{");
					sb.AppendLine($"{ts}{ts}{ts}IH[{_hpf}{_hmlact}] = TRUE;");
					sb.AppendLine($"{ts}{ts}{ts}IH[{_hpf}{_hplact}] = FALSE;");
					sb.AppendLine($"{ts}{ts}}}");
					sb.AppendLine($"{ts}{ts}else");
					sb.AppendLine($"{ts}{ts}{{");
					sb.AppendLine($"{ts}{ts}{ts}IH[{_hpf}{_hplact}] = TRUE;");
					sb.AppendLine($"{ts}{ts}{ts}IH[{_hpf}{_hmlact}] = FALSE;");
					sb.AppendLine($"{ts}{ts}}}");
					sb.AppendLine($"{ts}}}");
					sb.AppendLine($"{ts}else");
					sb.AppendLine($"{ts}{{");
					sb.AppendLine($"{ts}{ts}IH[{_hpf}{_hplact}] = TRUE;");
					sb.AppendLine($"{ts}{ts}IH[{_hpf}{_hmlact}] = FALSE;");
					sb.AppendLine($"{ts}}}");
					sb.AppendLine();

					if (c.PrioData.HDIngrepen.Any())
					{
						sb.AppendLine($"{ts}/* Bij hulpdienstingreep, lokaal VA regelen */");
						sb.AppendLine($"{ts}if (IH[{_hpf}{_hplhd}])");
						sb.AppendLine($"{ts}{{");
						sb.AppendLine($"{ts}{ts}IH[{_hpf}{_hmlact}] = TRUE;");
						sb.AppendLine($"{ts}{ts}IH[{_hpf}{_hplact}] = FALSE;");
						sb.AppendLine($"{ts}}}");
					}

					return sb.ToString();

				case CCOLCodeTypeEnum.HstCAanvragen:
					sb.AppendLine($"{ts}/* tijdens ple, wachtstandaanvraag uit */");
					sb.AppendLine($"{ts}if (IH[{_hpf}{_hkpact}])");
					sb.AppendLine($"{ts}{{");
					sb.AppendLine($"{ts}{ts}for (fc = 0; fc < FCMAX; ++fc)");
					sb.AppendLine($"{ts}{ts}{ts}A[fc] &= ~BIT2;");
                    if(c.HalfstarData.FaseCyclusInstellingen.Any(x => x.AanvraagOpTxB))
                    {
                        sb.AppendLine();
                        sb.AppendLine($"{ts}/* Aanvragen op TXB */");
                        foreach(var sg in c.HalfstarData.FaseCyclusInstellingen.Where(x => x.AanvraagOpTxB))
                        {
                            sb.Append($"{ts}if (aanvraag_txb({_fcpf}{sg.FaseCyclus})");
                            if (sg.PrivilegePeriodeOpzetten) sb.Append($" && PP[{_fcpf}{sg.FaseCyclus}]");
                            sb.AppendLine($") A[{_fcpf}{sg.FaseCyclus}] |= TRUE;");
                        }
                    }
					sb.AppendLine($"{ts}}}");
					return sb.ToString();

				case CCOLCodeTypeEnum.HstCVerlenggroen:
				case CCOLCodeTypeEnum.HstCMaxgroen:
                    sb.AppendLine($"{ts}for (fc = 0; fc < FCMAX; ++fc)");
					sb.AppendLine($"{ts}{{");
					sb.AppendLine($"{ts}{ts}/* afzetten functies en BITJES van ML-bedrijf */");
					sb.AppendLine($"{ts}{ts}TVG_max[fc] = 0;");
					sb.AppendLine($"{ts}{ts}YV[fc] &= ~(BIT2 | BIT4);");
					sb.AppendLine($"{ts}{ts}FM[fc] &= ~BIT2;");
					sb.AppendLine($"{ts}{ts}RW[fc] &= ~BIT2;");
					sb.AppendLine($"{ts}{ts}/* opzetten verlengfunctie (Vasthouden verlenggroen) bij PL-bedrijf */");
					sb.AppendLine($"{ts}{ts}YV[fc] |= MK[fc] && (YV_PL[fc] && PR[fc] || AR[fc] && yv_ar_max_pl(fc, 0)) ? BIT4 : 0;");
					sb.AppendLine($"{ts}}}");
					return sb.ToString();

				case CCOLCodeTypeEnum.HstCWachtgroen:
					sb.AppendLine($"{ts}/* Retour wachtgroen bij wachtgroen richtingen, let op: inclusief aanvraag! */");
					foreach (var fc in c.Fasen)
					{
						if (fc.Wachtgroen != NooitAltijdAanUitEnum.Nooit)
						{
							if (fc.Wachtgroen == NooitAltijdAanUitEnum.Altijd)
							{
								sb.AppendLine($"{ts}wachtstand_halfstar({_fcpf}{fc.Naam}, IH[{_hpf}{_hplact}], ({c.GetBoolV()})(TRUE), ({c.GetBoolV()})(TRUE));");
							}
							else
							{
								sb.AppendLine($"{ts}wachtstand_halfstar({_fcpf}{fc.Naam}, IH[{_hpf}{_hplact}], ({c.GetBoolV()})(SCH[{_schpf}{_schca}{fc.Naam}]), ({c.GetBoolV()})(SCH[{_schpf}{_schwg}{fc.Naam}]));");								
							}
						}
					}
					return sb.ToString();

				case CCOLCodeTypeEnum.HstCMeetkriterium:
                    sb.AppendLine($"{ts}for (fc = 0; fc < FCMAX; ++fc)");
                    sb.AppendLine($"{ts}{{");
                    sb.AppendLine($"{ts}{ts}{ts}/* afzetten BITJES van ML-bedrijf */");
                    sb.AppendLine($"{ts}{ts}{ts} Z[fc] &= ~BIT6;");
                    sb.AppendLine($"{ts}{ts}{ts}FM[fc] &= ~BIT6;");
                    sb.AppendLine($"{ts}{ts}{ts}RW[fc] &= ~BIT6;");
                    sb.AppendLine($"{ts}{ts}{ts}RR[fc] &= ~BIT6;");
                    sb.AppendLine($"{ts}{ts}{ts}YV[fc] &= ~BIT6;");
                    sb.AppendLine($"{ts}{ts}{ts}MK[fc] &= ~BIT6;");
                    sb.AppendLine($"{ts}{ts}{ts}PP[fc] &= ~BIT6;");
                    sb.AppendLine($"{ts}}}");
                    sb.AppendLine();
                    if (c.PrioData.PrioIngreepType != PrioIngreepTypeEnum.Geen)
                    {
                        sb.AppendLine($"{ts}if (SCH[{_schpf}{_schovpriople}])");
                        sb.AppendLine($"{ts}{{");
                        sb.AppendLine($"{ts}{ts}/* Prio meetkriterium bij PL bedrijf */");
                        foreach (var prio in c.PrioData.PrioIngrepen)
                        {
                            sb.AppendLine($"{ts}{ts}yv_PRIO_pl_halfstar({_fcpf}{prio.FaseCyclus}, BIT7, C[{_ctpf}{_cvc}{CCOLCodeHelper.GetPriorityName(prio)}]);");
                        }
                        sb.AppendLine($"{ts}}}");
                    }

					return sb.ToString();
				
				case CCOLCodeTypeEnum.HstCMeeverlengen:
					sb.AppendLine($"{ts}/* Resetten YM bit voor PL regelen */");
					sb.AppendLine($"{ts}for (fc = 0; fc < FCMAX; ++fc)");
					sb.AppendLine($"{ts}{ts}YM[fc] &= ~YM_HALFSTAR;");
					sb.AppendLine();

                    foreach (var fc in c.Fasen.Where(x => x.Meeverlengen != NooitAltijdAanUitEnum.Nooit))
                    {
                        var set_ym_pl_halfstar = "set_ym_pl_halfstar";
                        var set_ym_pl_halfstar_args = "";
                        if (c.Data.MultiModuleReeksen)
                        {
                            set_ym_pl_halfstar = "set_ym_pl_halfstar_fcfc";
                            var reeks = c.MultiModuleMolens.FirstOrDefault(x => x.Modules.Any(x2 => x2.Fasen.Any(x3 => x3.FaseCyclus == fc.Naam)));
                            if (reeks != null)
                            {
                                var rfc1 = c.Fasen.FirstOrDefault(x => reeks.Modules.SelectMany(x2 => x2.Fasen).Any(x3 => x3.FaseCyclus == x.Naam));
                                var rfc2 = c.Fasen.LastOrDefault(x => reeks.Modules.SelectMany(x2 => x2.Fasen).Any(x3 => x3.FaseCyclus == x.Naam));
                                if (rfc1 == null || rfc2 == null)
                                {
                                    set_ym_pl_halfstar_args = ", 0, FCMAX";
                                }
                                else
                                {
                                    var id2 = c.Fasen.IndexOf(rfc2);
                                    ++id2;
                                    set_ym_pl_halfstar_args = $", {_fcpf}{rfc1.Naam}, {(id2 == c.Fasen.Count ? "FCMAX" : $"{_fcpf}{c.Fasen[id2].Naam}")}";
                                }
                            }
                            else
                            {
                                set_ym_pl_halfstar_args = ", 0, FCMAX";
                            }
                        }

                        switch (fc.Meeverlengen)
                        {
                            case NooitAltijdAanUitEnum.Altijd:
                                sb.AppendLine($"{ts}{set_ym_pl_halfstar}({_fcpf}{fc.Naam}, TRUE{set_ym_pl_halfstar_args});");
                                break;
                            case NooitAltijdAanUitEnum.SchAan:
                            case NooitAltijdAanUitEnum.SchUit:
                                sb.AppendLine($"{ts}{set_ym_pl_halfstar}({_fcpf}{fc.Naam}, ({c.GetBoolV()})(SCH[{_schpf}{_schmv}{fc.Naam}]){set_ym_pl_halfstar_args});");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

					return sb.ToString();
				
				case CCOLCodeTypeEnum.HstCSynchronisaties:
					sb.AppendLine($"{ts}for (fc = 0; fc < FCMAX; ++fc)");
					sb.AppendLine($"{ts}{ts}YV[fc] &= ~YV_KOP_HALFSTAR;");
					sb.AppendLine();
					sb.AppendLine($"{ts}for (fc = 0; fc < FCMAX; ++fc)");
					sb.AppendLine($"{ts}{{");
					sb.AppendLine($"{ts}{ts}RR[fc]&= ~(BIT1 | BIT2 | BIT3 | RR_KOP_HALFSTAR | RR_VS_HALFSTAR);");
					sb.AppendLine($"{ts}{ts}RW[fc]&= ~(BIT3 | RW_KOP_HALFSTAR);");
					sb.AppendLine($"{ts}{ts}YV[fc]&= ~(BIT1 | YV_KOP_HALFSTAR);");
					sb.AppendLine($"{ts}{ts}YM[fc]&= ~(BIT3 | YM_KOP_HALFSTAR);");
					sb.AppendLine($"{ts}{ts} X[fc]&= ~(BIT1 | BIT2 |BIT3 | X_GELIJK_HALFSTAR | X_VOOR_HALFSTAR | X_DEELC_HALFSTAR);");
                    if(c.InterSignaalGroep.Gelijkstarten.Any() || c.InterSignaalGroep.Voorstarten.Any())
                    {
					    sb.AppendLine($"{ts}{ts}KR[fc]&= ~(BIT0 | BIT1 |BIT2 | BIT3 |BIT4 |BIT5 |BIT6 | BIT7);");
                    }
					sb.AppendLine($"{ts}}}");
					sb.AppendLine();
					
					foreach (var nl in c.InterSignaalGroep.Nalopen)
					{
						if (nl.Type == NaloopTypeEnum.EindeGroen ||
						    nl.Type == NaloopTypeEnum.CyclischVerlengGroen)
						{
							var t = nl.Type == NaloopTypeEnum.EindeGroen ? _tnleg : _tnlcv;
                            var dt = "NG";
                            if (nl.DetectieAfhankelijk)
                            {
                                dt = nl.Type == NaloopTypeEnum.EindeGroen ? $"{_tpf}{_tnlegd}{nl.FaseVan}{nl.FaseNaar}" : $"{_tpf}{_tnlcvd}{nl.FaseVan}{nl.FaseNaar}";
                            }
                            var xnl = "NG";
                            if (nl.MaximaleVoorstart.HasValue)
                            {
                                xnl = $"{_prmpf}{_prmxnl}{nl.FaseVan}{nl.FaseNaar}";
                            }
							sb.AppendLine($"{ts}naloopEG_CV_halfstar(TRUE, {_fcpf}{nl.FaseVan}, {_fcpf}{nl.FaseNaar}, {xnl}, {dt}, {_tpf}{t}{nl.FaseVan}{nl.FaseNaar});");
						}

                        if(nl.Type == NaloopTypeEnum.StartGroen)
                        {
                            if (nl.DetectieAfhankelijk && nl.Detectoren.Any())
                            {
                                sb.AppendLine($"{ts}naloopSG_halfstar({_fcpf}{nl.FaseVan}, {_fcpf}{nl.FaseNaar}, {_dpf}{nl.Detectoren[0].Detector}, {_hpf}{_hnla}{nl.Detectoren[0].Detector}, {_tpf}{_tnlsgd}{nl.FaseVan}{nl.FaseNaar});");
                            }
                            else
                            {
                                sb.AppendLine($"{ts}naloopSG_halfstar({_fcpf}{nl.FaseVan}, {_fcpf}{nl.FaseNaar}, TRUE, {_tpf}{_tnlsg}{nl.FaseVan}{nl.FaseNaar});");
                            }
                        }
					}
					sb.AppendLine();

                    if (c.InterSignaalGroep.Gelijkstarten.Any())
                    {
                        var gelijkstarttuples = CCOLCodeHelper.GetFasenWithGelijkStarts(c);
                        foreach (var gs in gelijkstarttuples)
                        {
                            var hxpl = _hxpl + string.Join(string.Empty, gs.Item2);
                            sb.Append($"{ts}gelijkstart_va_arg_halfstar({_hpf}{hxpl}, NG, FALSE, ");
                            foreach(var fc in gs.Item2)
                            {
                                sb.Append($"{_fcpf}{fc}, ");
                            }
                            sb.AppendLine("END);");
                            sb.AppendLine($"{ts}if (IH[{_hpf}{hxpl}])");
                            sb.AppendLine($"{ts}{{");
                            foreach(var fc in gs.Item2)
                            {
                                sb.Append($"{ts}{ts}if ((");
                                var first = true;
                                foreach (var fc2 in gs.Item2)
                                {
                                    if (fc == fc2) continue;
                                    if (!first) sb.Append(" || ");
                                    sb.Append($"A[{_fcpf}{fc2}]");
                                    first = false;
                                }
                                sb.AppendLine($") && !G[{_fcpf}{fc}]) X[{_fcpf}{fc}] |= X_GELIJK_HALFSTAR;");
                            }
                            sb.AppendLine($"{ts}}}");
                        }
                    }

                    return sb.ToString();
				
				case CCOLCodeTypeEnum.HstCAlternatief:
                    var gelijkstarttuples2 = CCOLCodeHelper.GetFasenWithGelijkStarts(c);

                    sb.AppendLine($"{ts}/* PAR correctie: PRIO alternatieven enkel voor richtingen met actieve PRIO ingreep */");
                    sb.AppendLine($"{ts}for (fc = 0; fc < FCMAX; ++fc)");
                    sb.AppendLine($"{ts}{{");
                    sb.AppendLine($"{ts}{ts}char hasOV = FALSE;");
                    sb.AppendLine($"{ts}{ts}for (ov = 0; ov < prioFCMAX; ++ov)");
                    sb.AppendLine($"{ts}{ts}{{");
                    sb.AppendLine($"{ts}{ts}{ts}if (iAantalInmeldingen[ov] > 0 && iFC_PRIOix[ov] == fc)");
                    sb.AppendLine($"{ts}{ts}{ts}{{");
                    sb.AppendLine($"{ts}{ts}{ts}{ts}hasOV = TRUE;");
                    sb.AppendLine($"{ts}{ts}{ts}{ts}break;");
                    sb.AppendLine($"{ts}{ts}{ts}}}");
                    sb.AppendLine($"{ts}{ts}}}");
                    sb.AppendLine($"{ts}{ts}if (!hasOV)");
                    sb.AppendLine($"{ts}{ts}{{");
                    sb.AppendLine($"{ts}{ts}{ts}PAR[fc] &= ~PRIO_PAR_BIT;");
                    sb.AppendLine($"{ts}{ts}}}");
                    sb.AppendLine($"{ts}}}");

                    foreach (var fc in c.ModuleMolen.FasenModuleData)
                    {
                        Tuple<string, List<string>> hasgs = null;
                        foreach (var gs in gelijkstarttuples2)
                        {
                            if (gs.Item1 == fc.FaseCyclus && gs.Item2.Count > 1)
                            {
                                hasgs = gs;
                                break;
                            }
                        }
                        if (hasgs != null)
                        {
                            sb.Append(
                                $"{ts}alternatief_halfstar({_fcpf}{fc.FaseCyclus}, PRM[{_prmpf}{_prmaltphst}");
                            foreach (var ofc in hasgs.Item2)
                            {
                                sb.Append(ofc);
                            }
                            sb.Append($"], SCH[{_schpf}{_schaltghst}");
                            foreach (var ofc in hasgs.Item2)
                            {
                                sb.Append(ofc);
                            }
                            sb.AppendLine("]);");
                        }
                        else
                        {
                            sb.AppendLine(
                                $"{ts}alternatief_halfstar({_fcpf}{fc.FaseCyclus}, PRM[{_prmpf}{_prmaltphst}{fc.FaseCyclus}], SCH[{_schpf}{_schaltghst}{fc.FaseCyclus}]);");
                        }
                    }
                    foreach (var nl in c.InterSignaalGroep.Nalopen)
                    {
                        if (nl.Type == NaloopTypeEnum.EindeGroen ||
                            nl.Type == NaloopTypeEnum.CyclischVerlengGroen)
                        {
                            var t = nl.Type == NaloopTypeEnum.EindeGroen ? $"{_tpf}{_tnleg}{nl.FaseVan}{nl.FaseNaar}" : $"{_tpf}{_tnlcv}{nl.FaseVan}{nl.FaseNaar}";
                            if (nl.DetectieAfhankelijk)
                            {
                                t = nl.Type == NaloopTypeEnum.EindeGroen ? $"{_tpf}{_tnlegd}{nl.FaseVan}{nl.FaseNaar}" : $"{_tpf}{_tnlcvd}{nl.FaseVan}{nl.FaseNaar}";
                            }
                            sb.AppendLine($"{ts}altcor_kop_halfstar({_fcpf}{nl.FaseVan}, {_fcpf}{nl.FaseNaar}, {t});");
                        }
                        if (nl.Type == NaloopTypeEnum.StartGroen)
                        {
                            if (nl.DetectieAfhankelijk && nl.Detectoren.Any())
                            {
                                sb.AppendLine($"{ts}altcor_naloopSG_halfstar({_fcpf}{nl.FaseVan}, {_fcpf}{nl.FaseNaar}, IH[{_hpf}{_hnla}{nl.Detectoren[0].Detector}], {_tpf}{_tnlsgd}{nl.FaseVan}{nl.FaseNaar}, TRUE);");
                            }
                            else
                            {
                                sb.AppendLine($"{ts}altcor_naloopSG_halfstar({_fcpf}{nl.FaseVan}, {_fcpf}{nl.FaseNaar}, TRUE, {_tpf}{_tnlsg}{nl.FaseVan}{nl.FaseNaar}, TRUE);");
                            }
                        }
                    }
                    
                    sb.AppendLine($"{ts}for (fc = 0; fc < FCMAX; ++fc)");
					sb.AppendLine($"{ts}{ts}RR[fc] &= ~RR_ALTCOR_HALFSTAR;");
					sb.AppendLine();
					
					if (c.HalfstarData.Hoofdrichtingen.Any())
					{
						sb.AppendLine($"{ts}/* hoofdrichtingen alleen tijdens periode alternatieve realisaties en koppeling uit */");
						sb.AppendLine($"{ts}if (!H[{_hpf}{_hperarh}] && H[{_hpf}kpact])");
						sb.AppendLine($"{ts}{{");
						foreach (var hfc in c.HalfstarData.Hoofdrichtingen)
						{
							sb.AppendLine($"{ts}{ts}if (!tussen_txa_en_txb({_fcpf}{hfc.FaseCyclus}) && !tussen_txb_en_txd({_fcpf}{hfc.FaseCyclus})) PAR[{_fcpf}{hfc.FaseCyclus}] &= ~BIT0;");
						}
						sb.AppendLine($"{ts}}}");
						sb.AppendLine();
					}

                    sb.AppendLine($"{ts}Alternatief_halfstar_Add();");
					sb.AppendLine();

                    sb.AppendLine($"{ts}/* retour rood wanneer richting AR heeft maar geen PAR meer */");
					sb.AppendLine($"{ts}/* -------------------------------------------------------- */");
					sb.AppendLine($"{ts}reset_altreal_halfstar();");
                    sb.AppendLine();

					sb.AppendLine($"{ts}");
                    sb.AppendLine($"{ts}signaalplan_alternatief();");
					
					return sb.ToString();
				
				case CCOLCodeTypeEnum.HstCRealisatieAfhandeling:
					sb.AppendLine($"{ts}for (fc = 0; fc < FCMAX; ++fc)");
					sb.AppendLine($"{ts}{{");
					sb.AppendLine($"{ts}{ts}PP[fc] &= ~BIT4;");
					sb.AppendLine($"{ts}{ts}YM[fc] &= ~BIT5;");
					sb.AppendLine($"{ts}{ts}RS[fc] &= ~RS_HALFSTAR;");
					sb.AppendLine($"{ts}{ts}PP[fc] |= GL[fc] ? BIT4 : 0; /* i.v.m. overslag door conflicten */");
					sb.AppendLine($"{ts}}}");
					sb.AppendLine($"{ts}");

                    #region Dubbele realisaties

                    if (c.HalfstarData.SignaalPlannen.SelectMany(x => x.Fasen).Any(x => x.B2.HasValue && x.B2 != 0 && x.D2.HasValue && x.D2 != null))
                    {
                        sb.AppendLine($"{ts}/* Tweede realisaties (middels parameters) */");
                        sb.AppendLine();
                        foreach (var pl in c.HalfstarData.SignaalPlannen)
                        {
                            sb.AppendLine($"{ts}/* {pl.Naam} */");
                            foreach (var fcpl in pl.Fasen)
                            {
                                if (fcpl.B2.HasValue && fcpl.D2.HasValue)
                                {
                                    sb.AppendLine($"{ts}set_2real({_fcpf}{fcpl.FaseCyclus}, {_prmpf}{_prmtx}A1{pl.Naam}_{fcpl.FaseCyclus}, {_prmpf}{_prmtx}A2{pl.Naam}_{fcpl.FaseCyclus}, {pl.Naam}, ({c.GetBoolV()})(IH[{_hpf}{_hplact}]));");
                                }
                            }
                            sb.AppendLine();
                        }
                    }

                    #endregion // Dubbele realisaties

                    if (c.HalfstarData.FaseCyclusInstellingen.Any(x => x.PrivilegePeriodeOpzetten))
                    {
                        sb.AppendLine($"{ts}/* PP opzetten */");
                        foreach (var sg in c.HalfstarData.FaseCyclusInstellingen.Where(x => x.PrivilegePeriodeOpzetten))
                        {
                            sb.AppendLine($"{ts}set_pp_halfstar({_fcpf}{sg.FaseCyclus}, IH[{_hpf}{_hkpact}], BIT4);");
                        }
                        sb.AppendLine();
                    }

                    sb.AppendLine($"{ts}for (fc = 0; fc < FCMAX; ++fc)");
                    sb.AppendLine($"{ts}{{");
                    sb.AppendLine($"{ts}{ts}/* Voorstartgroen tijdens voorstart t.o.v. sg-plan, alleen als gekoppeld wordt geregeld */");
#warning TODO: functie vs_ple() moet worden nagelopen en mogelijk herzien
                    sb.AppendLine($"{ts}{ts}vs_ple(fc, {_prmpf}{_prmrstotxa}, IH[{_hpf}{_hkpact}]);");
					sb.AppendLine($"");
					sb.AppendLine($"{ts}{ts}/* opzetten van YS en YW tijdens halfstar bedrijf */");
					sb.AppendLine($"{ts}{ts}/* resetten */");
					sb.AppendLine($"{ts}{ts}RW[fc] &= ~RW_WG_HALFSTAR;");
					sb.AppendLine($"{ts}{ts}YW[fc] &= ~YW_PL_HALFSTAR;");
					sb.AppendLine($"{ts}{ts}/* vasthouden wachtgroen functie bij PL-bedrijf */");
					sb.AppendLine($"{ts}{ts}RW[fc] |= YW_PL[fc] && tussen_txb_en_txc(fc) && (TXC[PL][fc] > 0) ? RW_WG_HALFSTAR : 0; /* TXC-afhandeling */");
					sb.AppendLine($"{ts}{ts}YW[fc] |= YW_PL[fc] && tussen_txb_en_txc(fc) && (TXC[PL][fc] > 0) ? YW_PL_HALFSTAR : 0; /* TXC-afhandeling */");
					sb.AppendLine($"{ts}}}");
					sb.AppendLine($"");
					sb.AppendLine($"{ts}/* primaire realisaties signaalplansturing */");
					sb.AppendLine($"{ts}/* --------------------------------------- */");
					if (c.PrioData.PrioIngreepType == PrioIngreepTypeEnum.Geen)
					{
						sb.AppendLine($"{ts}signaalplan_primair();");
					}
					else
					{
						sb.AppendLine($"{ts}if (SCH[{_schpf}{_schovpriople}])");
						sb.AppendLine($"{ts}{{");
						sb.AppendLine($"{ts}{ts}signaalplan_primair_PRIO_ple();");
						sb.AppendLine($"{ts}}}");
						sb.AppendLine($"{ts}else");
						sb.AppendLine($"{ts}{{");
						sb.AppendLine($"{ts}{ts}signaalplan_primair();");
						sb.AppendLine($"{ts}}}");
					}
					sb.AppendLine();
					sb.AppendLine($"{ts}/* afsluiten primaire aanvraaggebieden */");
					sb.AppendLine($"{ts}/* ----------------------------------- */");
					if (c.PrioData.PrioIngreepType == PrioIngreepTypeEnum.Geen)
					{
						sb.AppendLine($"{ts}set_pg_primair_fc();");
					}
					else
					{
						sb.AppendLine($"{ts}if (SCH[{_schpf}{_schovpriople}])");
						sb.AppendLine($"{ts}{{");
						sb.AppendLine($"{ts}{ts}set_pg_primair_fc_PRIO_ple();");
						sb.AppendLine($"{ts}}}");
						sb.AppendLine($"{ts}else");
						sb.AppendLine($"{ts}{{");
						sb.AppendLine($"{ts}{ts}set_pg_primair_fc();");
						sb.AppendLine($"{ts}}}");
					}
					sb.AppendLine();
					sb.AppendLine($"{ts}/* reset PG bij planwisseling */");
					sb.AppendLine($"{ts}/* -------------------------- */");
					sb.AppendLine($"{ts}/* anders kan PG op blijven staan, waardoor richting eenmaal wordt overgeslagen en de regeling kan vastlopen */");
					sb.AppendLine($"{ts}if (SH[{_hpf}{_hmlact}] || SH[{_hpf}{_hplact}] || SPL)");
					sb.AppendLine($"{ts}{{");
					sb.AppendLine($"{ts}{ts}for (fc = 0; fc < FCMAX; ++fc)");
					sb.AppendLine($"{ts}{ts}{{");
					sb.AppendLine($"{ts}{ts}{ts}PG[fc] = FALSE;");
					sb.AppendLine($"{ts}{ts}}}");
					sb.AppendLine($"{ts}}}");

                    return sb.ToString();
				
				case CCOLCodeTypeEnum.HstCPostApplication:
					sb.AppendLine($"{ts}/* Knipperpuls generator */");
					sb.AppendLine($"{ts}/* --------------------- */");
					sb.AppendLine($"{ts}RT[{_tpf}{_tleven}] = !T[{_tpf}{_tleven}]; /* timer herstarten */");
					sb.AppendLine($"{ts}if (ST[{_tpf}{_tleven}])  IH[{_hpf}{_hleven}] = !IH[{_hpf}{_hleven}];   /* hulpwaarde aan/uit zetten */");
					sb.AppendLine();
					foreach (var kp in c.HalfstarData.GekoppeldeKruisingen)
					{
						sb.AppendLine($"{ts}/* Levensignaal van {kp.KruisingNaam} */");

                        var ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}leven", KoppelSignaalRichtingEnum.In);
                        sb.AppendLine($"{ts}RT[{_tpf}{_tleven}{kp.KruisingNaam}] = SH[{_hpf}{kp.PTPKruising}{_hiks}{ipl:00}];");
						sb.AppendLine($"{ts}MM[{_mpf}{_mleven}{kp.KruisingNaam}] = T[{_tpf}{_tleven}{kp.KruisingNaam}];");
					}

					sb.AppendLine($"{ts}/* herstart fasebewakingstimers bij wisseling tussen ML/PL en SPL */");
					sb.AppendLine($"{ts}/* -------------------------------------------------------------- */");
					sb.AppendLine($"{ts}RTFB &= ~RTFB_PLVA_HALFSTAR;");
					sb.AppendLine($"{ts}RTFB |= (SH[{_hpf}{_hplact}] || SH[{_hpf}{_hmlact}] || (SPL && IH[{_hpf}{_hplact}])) ? RTFB_PLVA_HALFSTAR : 0;");
					return sb.ToString();
				
				case CCOLCodeTypeEnum.HstCPreSystemApplication:
                    if (c.HalfstarData.PlantijdenInParameters)
                    {

                        sb.AppendLine($"{ts}/* kopieer signaalplantijden - vanuit parameter lijst */");
                        sb.AppendLine($"{ts}/* -------------------------------------------------- */");
                        sb.AppendLine($"{ts}if (SCH[{_schpf}{_schinstprm}])");
                        sb.AppendLine($"{ts}{{");
                        sb.AppendLine($"{ts}{ts}short pl = 0;");
                        sb.AppendLine($"{ts}{ts}short error = FALSE;");
                        for (var pl = 0; pl < c.HalfstarData.SignaalPlannen.Count; pl++)
                        {
                            var ppl = c.HalfstarData.SignaalPlannen[pl];
                            if (!c.HalfstarData.SignaalPlannen[pl].Fasen.Any()) continue;
                            sb.AppendLine($"{ts}{ts}if (!error)");
                            sb.AppendLine($"{ts}{ts}{{");
                            sb.AppendLine($"{ts}{ts}{ts}error = CheckSignalplanPrms({ppl.Naam}, TX_max[{pl}], {_prmpf}{_prmtx}A1{ppl.Naam}_{ppl.Fasen.First().FaseCyclus});");
                            sb.AppendLine($"{ts}{ts}}}");
                        }
                        sb.AppendLine($"{ts}{ts}if (!error)");
                        sb.AppendLine($"{ts}{ts}{{");
                        for (var pl = 0; pl < c.HalfstarData.SignaalPlannen.Count; pl++)
                        {
                            var ppl = c.HalfstarData.SignaalPlannen[pl];
                            if (!c.HalfstarData.SignaalPlannen[pl].Fasen.Any()) continue;
                            sb.AppendLine($"{ts}{ts}{ts}SignalplanPrmsToTx({ppl.Naam}, {_prmpf}{_prmtx}A1{ppl.Naam}_{ppl.Fasen.First().FaseCyclus});");
                        }
                        sb.AppendLine($"{ts}{ts}}}");

                        sb.AppendLine($"{ts}{ts}if (!error)");
                        sb.AppendLine($"{ts}{ts}{{");
                        sb.AppendLine($"{ts}{ts}{ts}copy_signalplan(PL);");
                        if(c.Data.CCOLVersie >= CCOLVersieEnum.CCOL95)
                        {
                            sb.AppendLine($"{ts}{ts}{ts}create_trig();        /* creëer nieuwe TIG-tabel na wijzigingen geel-, ontruimingstijden */");
                            sb.AppendLine($"{ts}{ts}{ts}correction_trig();    /* correcties TIG-tabel a.g.v. koppelingen e.d. */");
                        }
                        else
                        {
                            sb.AppendLine($"{ts}{ts}{ts}create_tig();        /* creëer nieuwe TIG-tabel na wijzigingen geel-, ontruimingstijden */");
                            sb.AppendLine($"{ts}{ts}{ts}correction_tig();    /* correcties TIG-tabel a.g.v. koppelingen e.d. */");
                        }
                        sb.AppendLine($"{ts}{ts}{ts}check_signalplans(); /* check signalplans */");
                        sb.AppendLine($"{ts}{ts}}}");
                        sb.AppendLine($"{ts}{ts}SCH[{_schpf}{_schinstprm}] = 0;");
                        sb.AppendLine($"{ts}{ts}CIF_PARM1WIJZAP = (s_int16) (&SCH[{_schpf}{_schinstprm}] - CIF_PARM1);");
                        sb.AppendLine($"{ts}}}");
                        sb.AppendLine();
                    }
                    sb.AppendLine($"{ts}/* kopieer signaalplantijden - na wijziging */");
					sb.AppendLine($"{ts}/* ---------------------------------------- */");
                    sb.AppendLine($"{ts}#if (CCOL_V >= 95)");
					sb.AppendLine($"{ts}{ts}if (SCH[{_schpf}{_schinst}] || COPY_2_TRIG)");
                    sb.AppendLine($"{ts}#else");
					sb.AppendLine($"{ts}{ts}if (SCH[{_schpf}{_schinst}] || COPY_2_TIG)");
					sb.AppendLine($"{ts}#endif");
                    sb.AppendLine($"{ts}{{");
					sb.AppendLine($"{ts}{ts}copy_signalplan(PL);");
                    if (c.Data.CCOLVersie >= CCOLVersieEnum.CCOL95)
                    {
                        sb.AppendLine($"{ts}{ts}create_trig();        /* creëer nieuwe TIG-tabel na wijzigingen geel-, ontruimingstijden */");
                        sb.AppendLine($"{ts}{ts}correction_trig();    /* correcties TIG-tabel a.g.v. koppelingen e.d. */");
                    }
                    else
                    {
                        sb.AppendLine($"{ts}{ts}create_tig();        /* creëer nieuwe TIG-tabel na wijzigingen geel-, ontruimingstijden */");
                        sb.AppendLine($"{ts}{ts}correction_tig();    /* correcties TIG-tabel a.g.v. koppelingen e.d. */");
                    }
                    sb.AppendLine($"{ts}{ts}check_signalplans(); /* check signalplans */");
					sb.AppendLine($"{ts}{ts}SCH[{_schpf}{_schinst}] = 0;");
					sb.AppendLine($"{ts}{ts}#if (CCOL_V >= 95)");
					sb.AppendLine($"{ts}{ts}{ts}COPY_2_TRIG = FALSE;");
					sb.AppendLine($"{ts}{ts}#else");
					sb.AppendLine($"{ts}{ts}{ts}COPY_2_TIG = FALSE;");
					sb.AppendLine($"{ts}{ts}#endif");
                    sb.AppendLine($"{ts}{ts}CIF_PARM1WIJZAP = (s_int16) (&SCH[{_schpf}{_schinst}] - CIF_PARM1);");
					sb.AppendLine($"{ts}}}");
					sb.AppendLine($"{ts}RTX = FALSE;");
					sb.AppendLine($"{ts}");
					sb.AppendLine($"{ts}if (IH[{_hpf}{_hplact}]) /* Code alleen bij PL-bedrijf */");
					sb.AppendLine($"{ts}{{");
					if (master != null && c.HalfstarData.Type != HalfstarTypeEnum.Master)
					{
						#warning TODO need code for running single appl.
                        var ipl = CCOLElementCollector.GetKoppelSignaalCount(master.PTPKruising, $"{master.KruisingNaam}kpuls", KoppelSignaalRichtingEnum.In);
						sb.AppendLine($"{ts}{ts}RT[{_tpf}{_toffset}] = SH[{_hpf}{master.PTPKruising}{_hiks}{ipl:00}]; /* offset starten op start koppelpuls */");
						sb.AppendLine($"{ts}{ts}SYN_TXS = ET[{_tpf}offset]; /* synchronisatie einde offset timer */");
						sb.AppendLine($"{ts}{ts}synchronization_timer(SAPPLPROG, T_max[{_tpf}xmarge]);");
					}
					sb.AppendLine($"{ts}{ts}FTX = HTX = FALSE;  /* reset instructievariabelen van TX */");
					sb.AppendLine($"{ts}{ts}");
					sb.AppendLine($"{ts}{ts}if (!IH[{_hpf}{_hkpact}] && !IH[{_hpf}{_hmlact}])");
					sb.AppendLine($"{ts}{ts}{{");
					sb.AppendLine($"{ts}{ts}{ts}/* ongekoppelde voertuigafhankelijke signaalplansturing */");
					sb.AppendLine($"{ts}{ts}{ts}/* ---------------------------------------------------- */");
					sb.AppendLine($"{ts}{ts}{ts}for (fc = 0; fc < FC_MAX; ++fc)  ");
					sb.AppendLine($"{ts}{ts}{ts}{ts}YW_PL[fc] = FALSE;");
					sb.AppendLine($"{ts}{ts}{ts}");
					sb.AppendLine($"{ts}{ts}{ts}FTX = !H[{_hpf}{_homschtegenh}] &&");
					sb.AppendLine($"{ts}{ts}{ts}      versnel_tx(TRUE); /* voertuigafhankelijk */");
					sb.AppendLine($"{ts}{ts}}}");
					if (master != null && c.HalfstarData.Type != HalfstarTypeEnum.Master)
					{
						sb.AppendLine($"{ts}{ts}else");
						sb.AppendLine($"{ts}{ts}{{");
						sb.AppendLine($"{ts}{ts}{ts}/* gekoppelde signaalplansturing */");
						sb.AppendLine($"{ts}{ts}{ts}/* ----------------------------- */");
						sb.AppendLine($"{ts}{ts}{ts}/* als TXS_SYNC, en daarmee ook TXS_OKE, de regeling zacht of hard synchroniseren afhankelijk van ");
						sb.AppendLine($"{ts}{ts}{ts}{ts} positie ten opzichte van de master */");
						sb.AppendLine($"{ts}{ts}{ts}if (MM[{_mpf}{_mleven}{master.KruisingNaam}] && TXS_OKE && TXS_SYNC && (TXS_delta > 0) && (PL==APL))");
						sb.AppendLine($"{ts}{ts}{ts}{{");
						sb.AppendLine($"{ts}{ts}{ts}{ts}/* regeling loopt iets vooruit (2 x marge) */");
						sb.AppendLine($"{ts}{ts}{ts}{ts}if (TXS_delta > 0 && (TXS_delta <= (2 * T_max[{_tpf}{_txmarge}])))");
						sb.AppendLine($"{ts}{ts}{ts}{ts}{{");
						sb.AppendLine($"{ts}{ts}{ts}{ts}{ts}HTX = TRUE;");
						sb.AppendLine($"{ts}{ts}{ts}{ts}}}");
						sb.AppendLine($"{ts}{ts}{ts}{ts}else");
						sb.AppendLine($"{ts}{ts}{ts}{ts}/* regeling loop iets achter (2 x marge) */");
						sb.AppendLine($"{ts}{ts}{ts}{ts}if (TXS_delta > 0 && (TXS_delta >= (TX_max[PL] - (2 * T_max[{_tpf}{_txmarge}]))))");
						sb.AppendLine($"{ts}{ts}{ts}{ts}{{");
						sb.AppendLine($"{ts}{ts}{ts}{ts}{ts}FTX = versnel_tx(FALSE);");
						sb.AppendLine($"{ts}{ts}{ts}{ts}}}");
						sb.AppendLine($"{ts}{ts}{ts}{ts}/* in alle andere gevallen is de afwijking te groot en moet, om lange synchronisatietijden te voorkomen,");
						sb.AppendLine($"{ts}{ts}{ts}{ts}{ts} de regeling hard worden gesynschroniseerd */");
						sb.AppendLine($"{ts}{ts}{ts}{ts}else");
						sb.AppendLine($"{ts}{ts}{ts}{ts}{{");
						sb.AppendLine($"{ts}{ts}{ts}{ts}{ts}if (!H[{_hpf}{_homschtegenh}]) /* koppelingen en pelotons mogen niet worden afgekapt     */");
						sb.AppendLine($"{ts}{ts}{ts}{ts}{ts}{ts}TX_timer = TXS_timer; /* TX_timer gelijk maken aan de cyclustijd van de master */");
						sb.AppendLine($"{ts}{ts}{ts}{ts}}}");
						sb.AppendLine($"{ts}{ts}{ts}}}");
						sb.AppendLine($"{ts}{ts}{ts}else");
						sb.AppendLine($"{ts}{ts}{ts}{{");
						sb.AppendLine($"{ts}{ts}{ts}{ts}/* do nothing */");
						sb.AppendLine($"{ts}{ts}{ts}}}");
						sb.AppendLine($"{ts}{ts}}}");
					}
					sb.AppendLine($"{ts}}} /* Einde code PL-bedrijf */");
					
					if (master != null && c.HalfstarData.Type != HalfstarTypeEnum.Master)
					{
						sb.AppendLine($"{ts}/* tijdens VA bedrijf hard synchroniseren */");
						sb.AppendLine($"{ts}else");
						sb.AppendLine($"{ts}{{");
                        var ipl = CCOLElementCollector.GetKoppelSignaalCount(master.PTPKruising, $"{master.KruisingNaam}kpuls", KoppelSignaalRichtingEnum.In);
						sb.AppendLine($"{ts}{ts}RTX = SH[{_hpf}{master.PTPKruising}{_hiks}{ipl:00}];");
                        sb.AppendLine($"{ts}}}");
					}

					sb.AppendLine();

					foreach (var kp in c.HalfstarData.GekoppeldeKruisingen)
					{
						int ipl;
						switch (kp.Type)
						{
							// If the coupled intersection is the master of this one
							case HalfstarGekoppeldTypeEnum.Master:
								// Receive: leven, koppelpuls, pervar, perarh, actief plan (op FALSE zetten indien geen leven)
								sb.AppendLine($"{ts}/* Koppelsignalen (PTP) van {kp.KruisingNaam} */");
                                
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}leven", KoppelSignaalRichtingEnum.In);
                                sb.AppendLine($"{ts}GUS[{_uspf}in{kp.KruisingNaam}{_usleven}] = IH[{_hpf}{kp.PTPKruising}{_hiks}{ipl:00}];");
								sb.AppendLine($"{ts}if (MM[{_mpf}{_mleven}{kp.KruisingNaam}])");
								sb.AppendLine($"{ts}{{");
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}kpuls", KoppelSignaalRichtingEnum.In);
								sb.AppendLine($"{ts}{ts}GUS[{_uspf}in{kp.KruisingNaam}{_uskpuls}] = IH[{_hpf}{kp.PTPKruising}{_hiks}{ipl:00}];");
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}pervar", KoppelSignaalRichtingEnum.In);
								sb.AppendLine($"{ts}{ts}GUS[{_uspf}in{kp.KruisingNaam}{_uspervar}] = IH[{_hpf}{kp.PTPKruising}{_hiks}{ipl:00}];");
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}perarh", KoppelSignaalRichtingEnum.In);
								sb.AppendLine($"{ts}{ts}GUS[{_uspf}in{kp.KruisingNaam}{_usperarh}] = IH[{_hpf}{kp.PTPKruising}{_hiks}{ipl:00}];");
								foreach (var pl in c.HalfstarData.SignaalPlannen)
								{
                                    ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}pl{pl.Naam}", KoppelSignaalRichtingEnum.In);
									sb.AppendLine($"{ts}{ts}GUS[{_uspf}in{kp.KruisingNaam}{pl.Naam}] = IH[{_hpf}{kp.PTPKruising}{_hiks}{ipl:00}];");
								}
								sb.AppendLine($"{ts}}}");
								sb.AppendLine($"{ts}else");
								sb.AppendLine($"{ts}{{");
								sb.AppendLine($"{ts}{ts}GUS[{_uspf}in{kp.KruisingNaam}{_uskpuls}] = FALSE;");
								sb.AppendLine($"{ts}{ts}GUS[{_uspf}in{kp.KruisingNaam}{_uspervar}] = FALSE;");
								sb.AppendLine($"{ts}{ts}GUS[{_uspf}in{kp.KruisingNaam}{_usperarh}] = FALSE;");
								foreach (var pl in c.HalfstarData.SignaalPlannen)
								{
									sb.AppendLine($"{ts}{ts}GUS[{_uspf}in{kp.KruisingNaam}{pl.Naam}] = FALSE;");
								}
								sb.AppendLine($"{ts}}}");
								sb.AppendLine();
                                // Send: leven, synch, txs
								sb.AppendLine($"{ts}/* Koppelsignalen (PTP) naar {kp.KruisingNaam} */");
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}leven", KoppelSignaalRichtingEnum.Uit);
								sb.AppendLine($"{ts}GUS[{_uspf}uit{kp.KruisingNaam}{_usleven}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl++:00}] = IH[{_hpf}{_hleven}];");
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}syncok", KoppelSignaalRichtingEnum.Uit);
								sb.AppendLine($"{ts}GUS[{_uspf}uit{kp.KruisingNaam}{_ussyncok}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl++:00}] = REG && (MM[{_mpf}{_mleven}{kp.KruisingNaam}] && (TXS_delta == 0) && TXS_OKE);");
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}txsok", KoppelSignaalRichtingEnum.Uit);
								sb.AppendLine($"{ts}GUS[{_uspf}uit{kp.KruisingNaam}{_ustxsok}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl:00}] = REG && MM[{_mpf}{_mleven}{kp.KruisingNaam}] && TXS_OKE;");
								sb.AppendLine();
								break;
							// Otherwise, the coupled intersection is a slave of this one
							case HalfstarGekoppeldTypeEnum.Slave:
								sb.AppendLine($"{ts}/* Koppelsignalen (PTP) naar {kp.KruisingNaam} */");
								var mts2 = ts;
								// If fallback: send master if master alive, otherwise determine by own judgement
								if (c.HalfstarData.Type == HalfstarTypeEnum.FallbackMaster)
								{
									mts2 = ts + ts;
									sb.AppendLine($"{ts}if (MM[{_mpf}{_mleven}{master.KruisingNaam}])");
									sb.AppendLine($"{ts}{{");
                                    ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}leven", KoppelSignaalRichtingEnum.Uit);
                                    var ipli = CCOLElementCollector.GetKoppelSignaalCount(master.PTPKruising, $"{master.KruisingNaam}leven", KoppelSignaalRichtingEnum.In);
									sb.AppendLine($"{mts2}GUS[{_uspf}uit{kp.KruisingNaam}{_usleven}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl:00}] = IH[{_hpf}{master.PTPKruising}{_hiks}{ipli:00}]; /* uitgaand levensignaal naar alle aangesloten kp's */");
                                    ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}kpuls", KoppelSignaalRichtingEnum.Uit);
                                    ipli = CCOLElementCollector.GetKoppelSignaalCount(master.PTPKruising, $"{master.KruisingNaam}kpuls", KoppelSignaalRichtingEnum.In);
									sb.AppendLine($"{mts2}GUS[{_uspf}uit{kp.KruisingNaam}{_uskpuls}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl:00}] = IH[{_hpf}{master.PTPKruising}{_hiks}{ipli:00}]; /* koppelpuls master doorsturen */");
                                    ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}pervar", KoppelSignaalRichtingEnum.Uit);
                                    ipli = CCOLElementCollector.GetKoppelSignaalCount(master.PTPKruising, $"{master.KruisingNaam}pervar", KoppelSignaalRichtingEnum.In);
									sb.AppendLine($"{mts2}GUS[{_uspf}uit{kp.KruisingNaam}{_uspervar}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl:00}] = IH[{_hpf}{master.PTPKruising}{_hiks}{ipli:00}]; /* periode var master doorsturen */");
                                    ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}perarh", KoppelSignaalRichtingEnum.Uit);
                                    ipli = CCOLElementCollector.GetKoppelSignaalCount(master.PTPKruising, $"{master.KruisingNaam}perarh", KoppelSignaalRichtingEnum.In);
									sb.AppendLine($"{mts2}GUS[{_uspf}uit{kp.KruisingNaam}{_usperarh}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl:00}] = IH[{_hpf}{master.PTPKruising}{_hiks}{ipli:00}]; /* periode arh master doorsturen */");
									foreach (var pl in c.HalfstarData.SignaalPlannen)
									{
                                        ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}pl{pl.Naam}", KoppelSignaalRichtingEnum.Uit);
                                        ipli = CCOLElementCollector.GetKoppelSignaalCount(master.PTPKruising, $"{master.KruisingNaam}pl{pl.Naam}", KoppelSignaalRichtingEnum.In);
										sb.AppendLine($"{mts2}GUS[{_uspf}uit{kp.KruisingNaam}{pl.Naam}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl:00}] = IH[{_hpf}{master.PTPKruising}{_hiks}{ipli:00}];");
									}
									sb.AppendLine($"{ts}}}");
									sb.AppendLine($"{ts}else");
									sb.AppendLine($"{ts}{{");
								}

								// For master and fallback, send data to coupled slave: leven, koppelpuls, pervar, perarh, actief plan
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}leven", KoppelSignaalRichtingEnum.Uit);
								sb.AppendLine($"{mts2}GUS[{_uspf}uit{kp.KruisingNaam}{_usleven}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl:00}] = IH[{_hpf}{_hleven}]; /* uitgaand levensignaal naar alle aangesloten kp's */");
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}kpuls", KoppelSignaalRichtingEnum.Uit);
								sb.AppendLine($"{mts2}GUS[{_uspf}uit{kp.KruisingNaam}{_uskpuls}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl:00}] = ((TX_timer <= 1)); /* koppelpuls master */");
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}pervar", KoppelSignaalRichtingEnum.Uit);
								sb.AppendLine($"{mts2}GUS[{_uspf}uit{kp.KruisingNaam}{_uspervar}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl:00}] = (IH[{_hpf}{_hpervar}] || SCH[{_schpf}{_schvarstreng}]); /* periode var master */");
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}perarh", KoppelSignaalRichtingEnum.Uit);
								sb.AppendLine($"{mts2}GUS[{_uspf}uit{kp.KruisingNaam}{_usperarh}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl:00}] = (IH[{_hpf}{_hperarh}]); /* periode arh master */");
								foreach (var pl in c.HalfstarData.SignaalPlannen)
								{
                                    ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}pl{pl.Naam}", KoppelSignaalRichtingEnum.Uit);
									sb.AppendLine($"{mts2}GUS[{_uspf}uit{kp.KruisingNaam}{pl.Naam}] = IH[{_hpf}{kp.PTPKruising}{_huks}{ipl:00}] = ((APL == {pl.Naam}));");
								}
								if (c.HalfstarData.Type == HalfstarTypeEnum.FallbackMaster)
								{
									sb.AppendLine($"{ts}}}");
								}
								sb.AppendLine();
								// Receive from slave: leven, synch, txs
								sb.AppendLine($"{ts}/* Koppelsignalen via (PTP) van {kp.KruisingNaam} */");
								ipl = 1;
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}leven", KoppelSignaalRichtingEnum.In);
								sb.AppendLine($"{ts}GUS[{_uspf}in{kp.KruisingNaam}{_usleven}] = IH[{_hpf}{kp.PTPKruising}{_hiks}{ipl:00}];");
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}syncok", KoppelSignaalRichtingEnum.In);
								sb.AppendLine($"{ts}GUS[{_uspf}in{kp.KruisingNaam}{_ussyncok}] = IH[{_hpf}{kp.PTPKruising}{_hiks}{ipl:00}];");
                                ipl = CCOLElementCollector.GetKoppelSignaalCount(kp.PTPKruising, $"{kp.KruisingNaam}txsok", KoppelSignaalRichtingEnum.In);
								sb.AppendLine($"{ts}GUS[{_uspf}in{kp.KruisingNaam}{_ustxsok}] = IH[{_hpf}{kp.PTPKruising}{_hiks}{ipl:00}];");
								sb.AppendLine();
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}
					
					// Outpus for all halfstar intersections
					sb.AppendLine($"{ts}GUS[{_uspf}{_usplact}] = IH[{_hpf}{_hplact}];");
					sb.AppendLine($"{ts}GUS[{_uspf}{_usmlact}] = IH[{_hpf}{_hmlact}];");
					sb.AppendLine($"{ts}GUS[{_uspf}{_uskpact}] = IH[{_hpf}{_hkpact}];");
                    if (c.Data.MultiModuleReeksen)
                    {
					    sb.AppendLine($"{ts}GUS[{_uspf}{_usmlpl}] = IH[{_hpf}{_hplact}] ? (s_int16)(PL+1): 0;");
                    }
                    else
                    {
                        sb.AppendLine($"{ts}GUS[{_uspf}{_usmlpl}] = IH[{_hpf}{_hplact}] ? (s_int16)(PL+1): (s_int16)(ML+1);");
                    }
                    foreach(var pl in c.HalfstarData.SignaalPlannen)
                    {
					    sb.AppendLine($"{ts}GUS[{_uspf}{_uspl}{pl.Naam}] = PL == {pl.Naam};");
                    }
					sb.AppendLine($"{ts}GUS[{_uspf}{_ustxtimer}] = IH[{_hpf}{_hplact}] ? (s_int16)(TX_timer): 0;");
					sb.AppendLine($"{ts}GUS[{_uspf}{_usklok}] = MM[{_mpf}{_mklok}];");
					sb.AppendLine($"{ts}GUS[{_uspf}{_ushand}] = MM[{_mpf}{_mhand}];");
					if (c.HalfstarData.Type != HalfstarTypeEnum.Master)
					{
						sb.AppendLine($"{ts}GUS[{_uspf}{_usmaster}] = MM[{_mpf}{_mmaster}];");
                        sb.AppendLine($"{ts}GUS[{_uspf}{_usslave}] = MM[{_mpf}{_mslave}];");									
					}
					sb.AppendLine();

					return sb.ToString();
				
				case CCOLCodeTypeEnum.HstCPostSystemApplication:
                    return sb.ToString();
				
				case CCOLCodeTypeEnum.HstCPostDumpApplication:
					return sb.ToString();

                case CCOLCodeTypeEnum.HstCPrioHalfstarSettings:
                    var enter = false;
                    if (c.HalfstarData.IsHalfstar && c.HasPT())
                    {
                        sb.AppendLine($"{ts}/* Bepalen tijd na TXD t.b.v. verlengen bij OV ingreep */");
                        if (c.PrioData.PrioIngreepType != PrioIngreepTypeEnum.Geen)
                        {
                            foreach (var ov in c.PrioData.PrioIngrepen)
                            {
                                sb.AppendLine($"{ts}iExtraGroenNaTXD[prioFC{CCOLCodeHelper.GetPriorityName(ov)}] = PRM[{_prmpf}{_prmnatxdhst}{CCOLCodeHelper.GetPriorityName(ov)}];");
                            }
                        }
                        else if (c.PrioData.PrioIngreepType == PrioIngreepTypeEnum.GeneriekePrioriteit)
                        {
                            foreach (var ov in c.PrioData.PrioIngrepen)
                            {
                                sb.AppendLine($"{ts}iExtraGroenNaTXD[prioFC{CCOLCodeHelper.GetPriorityName(ov)}] = PRM[{_prmpf}{_prmnatxdhst}{CCOLCodeHelper.GetPriorityName(ov)}];");
                            }
                        }
                        enter = true;
                    }
                    if (c.HalfstarData.Hoofdrichtingen.Any())
                    {
                        if (enter) sb.AppendLine();
                        sb.AppendLine($"{ts}/* PRIO opties hoofdrichtingen */");
                        sb.Append($"{ts}PrioHalfstarBepaalHoofdrichtingOpties(NG, ");
                        var first = true;
                        foreach (var hr in c.HalfstarData.Hoofdrichtingen)
                        {
                            if (!first)
                            {
                                sb.Append($"{ts}                                        ");
                            }
                            first = false;
                            sb.AppendLine($"(va_count){_fcpf}{hr.FaseCyclus}, (va_mulv)SCH[{_schpf}{_schtegenov}{hr.FaseCyclus}], (va_mulv)SCH[{_schpf}{_schafkwgov}{hr.FaseCyclus}], (va_mulv)SCH[{_schpf}{_schafkvgov}{hr.FaseCyclus}], TFG_max[{_fcpf}{hr.FaseCyclus}],");
                        }
                        sb.Append($"{ts}                                        ");
                        sb.AppendLine($"(va_count)END);");
                    }
                    return sb.ToString();

                #endregion // hst.c     

                #region prio.c

                case CCOLCodeTypeEnum.PrioCInitPrio:
                    sb.AppendLine($"{ts}PrioHalfstarInit();");
                    return sb.ToString();

                case CCOLCodeTypeEnum.PrioCInstellingen:
                    sb.AppendLine($"{ts}PrioHalfstarSettings();");
                    return sb.ToString();

                case CCOLCodeTypeEnum.PrioCPrioriteitsOpties:
                    if (c.PrioData.HDIngrepen.Any())
                    {
                        sb.AppendLine($"{ts}/* bijhouden of een hulpdienstingreep plaatsvindt */");
                        sb.AppendLine($"{ts}IH[{_hpf}{_hplhd}] = FALSE;");
                        sb.AppendLine($"{ts}for (fc = 0; fc < prioFCMAX; ++fc)");
                        sb.AppendLine($"{ts}{{");
                        sb.AppendLine($"{ts}{ts}if (iPrioriteitsOpties[fc] & poNoodDienst)");
                        sb.AppendLine($"{ts}{ts}{ts}IH[{_hpf}{_hplhd}] |= TRUE;");
                        sb.AppendLine($"{ts}}}");
                        sb.AppendLine();
                    }
                    if (c.PrioData.PrioIngrepen.Any())
                    {
                        sb.AppendLine($"{ts}/* tijdens halfstar bedrijf alleen optie aanvraag voor OV richtingen */");
                        sb.AppendLine($"{ts}if (IH[{_hpf}{_hplact}] && SCH[{_schpf}{_schovpriople}])");
                        sb.AppendLine($"{ts}{{");
                        foreach (var prio in c.PrioData.PrioIngrepen)
                        {
                            sb.AppendLine($"{ts}{ts}iPrioriteitsOpties[prioFC{CCOLCodeHelper.GetPriorityName(prio)}] |= PrioHalfstarBepaalPrioriteitsOpties({_prmpf}{_prmpriohst}{CCOLCodeHelper.GetPriorityName(prio)});");
                        }
                        sb.AppendLine($"{ts}}}");
                        sb.AppendLine();
                    }

                    sb.AppendLine();
                    sb.AppendLine($"{ts}/* Geen prioriteit indien voorwaarden tegenhouden omschakelen waar zijn */");
                    sb.AppendLine($"{ts}if (IH[{_hpf}{_homschtegenh}] && IH[{_hpf}{_hplact}] && SCH[{_schpf}{_schovpriople}])");
                    sb.AppendLine($"{ts}{{");
                    sb.AppendLine($"{ts}{ts}for (fc = 0; fc < prioFCMAX; ++fc)");
                    sb.AppendLine($"{ts}{ts}{ts}iXPrio[fc] |= BIT6;");
                    sb.AppendLine($"{ts}}}");
                    sb.AppendLine($"{ts}else");
                    sb.AppendLine($"{ts}{{");
                    sb.AppendLine($"{ts}{ts}for (fc = 0; fc < prioFCMAX; ++fc)");
                    sb.AppendLine($"{ts}{ts}{ts}iXPrio[fc] &= ~BIT6;");
                    sb.AppendLine($"{ts}}}");
                    sb.AppendLine();
                    return sb.ToString();

                case CCOLCodeTypeEnum.PrioCOnderMaximum:
                    sb.AppendLine($"{ts}if (SCH[{_schpf}{_schovpriople}]) PrioHalfstarOnderMaximum();");
                    return sb.ToString();
                case CCOLCodeTypeEnum.PrioCAfkapGroen:
                    sb.AppendLine($"{ts}if (SCH[{_schpf}{_schovpriople}]) PrioHalfstarAfkapGroen();");
                    return sb.ToString();
                case CCOLCodeTypeEnum.PrioCStartGroenMomenten:
                    sb.AppendLine($"{ts}if (SCH[{_schpf}{_schovpriople}]) PrioHalfstarStartGroenMomenten();");
                    return sb.ToString();
                case CCOLCodeTypeEnum.PrioCTegenhoudenConflicten:
                    sb.AppendLine($"{ts}if (SCH[{_schpf}{_schovpriople}]) PrioHalfstarTegenhouden();");
                    return sb.ToString();
                case CCOLCodeTypeEnum.PrioCAfkappen:
                    sb.AppendLine($"{ts}if (SCH[{_schpf}{_schovpriople}]) PrioHalfstarAfkappen();");
                    return sb.ToString();
                case CCOLCodeTypeEnum.PrioCTerugkomGroen:
                    sb.AppendLine($"{ts}if (SCH[{_schpf}{_schovpriople}]) PrioHalfstarTerugkomGroen();");
                    return sb.ToString();
                case CCOLCodeTypeEnum.PrioCGroenVasthouden:
                    sb.AppendLine($"{ts}if (SCH[{_schpf}{_schovpriople}]) PrioHalfstarGroenVasthouden();");
                    return sb.ToString();
                case CCOLCodeTypeEnum.PrioCMeetkriterium:
                    sb.AppendLine($"{ts}if (SCH[{_schpf}{_schovpriople}]) PrioHalfstarMeetKriterium();");
                    return sb.ToString();

                #endregion // prio.c

                default:
					return null;
			}
		}
			
		public override bool SetSettings(CCOLGeneratorClassWithSettingsModel settings, ICCOLGeneratorSettingsProvider settingsProvider)
		{
			_mperiod = settingsProvider.GetElementName("mperiod");
			_cvc = settingsProvider.GetElementName("cvc");
			_schmv = settingsProvider.GetElementName("schmv");
			_schwg = settingsProvider.GetElementName("schwg");
			_schca = settingsProvider.GetElementName("schca");
            _tnlsg = settingsProvider.GetElementName("tnlsg");
			_tnlsgd = settingsProvider.GetElementName("tnlsgd");
			_tnlcv = settingsProvider.GetElementName("tnlcv");
			_tnlcvd = settingsProvider.GetElementName("tnlcvd");
			_tnleg = settingsProvider.GetElementName("tnleg");
			_tnlegd = settingsProvider.GetElementName("tnlegd");
			_huks = settingsProvider.GetElementName("huks");
            _hiks = settingsProvider.GetElementName("hiks");
            _prmxnl = settingsProvider.GetElementName("prmxnl");
            _hnla = settingsProvider.GetElementName("hnla");

            _hprioin = settingsProvider.GetElementName("hprioin");
            _hpriouit = settingsProvider.GetElementName("hpriouit");

            return base.SetSettings(settings, settingsProvider);
		}
	}
}
