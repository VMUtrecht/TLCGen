﻿using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TLCGen.Helpers;
using TLCGen.Plugins.RIS.Models;
using RelayCommand = GalaSoft.MvvmLight.CommandWpf.RelayCommand;
using System;
using TLCGen.Messaging.Messages;
using System.Linq;
using TLCGen.Extensions;
using TLCGen.ModelManagement;
using TLCGen.Models;

namespace TLCGen.Plugins.RIS
{
    public class RISTabViewModel : ViewModelBase
    {
        #region Fields

        private RISPlugin _plugin;

        #endregion // Fields

        #region Properties

        private RISFaseCyclusDataViewModel _selectedRISFase;
        public RISFaseCyclusDataViewModel SelectedRISFase
        {
            get => _selectedRISFase;
            set
            {
                _selectedRISFase = value;
                RaisePropertyChanged();
            }
        }

        private RISDataModel _RISModel;
        public RISDataModel RISModel
        {
            get => _RISModel;
            set
            {
                _RISModel = value;
                RISFasen = new ObservableCollectionAroundList<RISFaseCyclusDataViewModel, RISFaseCyclusDataModel>(_RISModel.RISFasen);
                RISRequestExtendLanes = new ObservableCollectionAroundList<RISLaneRequestExtendDataViewModel, RISLaneRequestExtendDataModel>(_RISModel.RISRequestExtendLanes);
            }
        }

        public ObservableCollectionAroundList<RISFaseCyclusDataViewModel, RISFaseCyclusDataModel> RISFasen { get; private set; }
        public ObservableCollectionAroundList<RISLaneRequestExtendDataViewModel, RISLaneRequestExtendDataModel> RISRequestExtendLanes { get; private set; }

        public ObservableCollection<RISFaseCyclusLaneDataViewModel> RISLanes { get; } = new ObservableCollection<RISFaseCyclusLaneDataViewModel>();

        public bool RISToepassen
        {
            get => _RISModel.RISToepassen;
            set
            {
                _RISModel.RISToepassen = value;
                RaisePropertyChanged<object>(broadcast: true);
                if (string.IsNullOrWhiteSpace(SystemITF)) SystemITF = _plugin.Controller.Data.Naam;
                foreach (var fc in RISFasen)
                {
                    var sg = _plugin.Controller.Fasen.First(x => x.Naam == fc.FaseCyclus);
                    if (sg != null && fc.Lanes.Any())
                    {
                        foreach (var l in fc.Lanes)
                        {
                            if (l.SimulatedStations.Any())
                            {
                                switch (sg.Type)
                                {
                                    case TLCGen.Models.Enumerations.FaseTypeEnum.Auto:
                                        if (l.SimulatedStations[0].Type != RISStationTypeSimEnum.PASSENGERCAR) l.SimulatedStations[0].Type = RISStationTypeSimEnum.PASSENGERCAR;
                                        break;
                                    case TLCGen.Models.Enumerations.FaseTypeEnum.Fiets:
                                        if (l.SimulatedStations[0].Type != RISStationTypeSimEnum.CYCLIST) l.SimulatedStations[0].Type = RISStationTypeSimEnum.CYCLIST;
                                        break;
                                    case TLCGen.Models.Enumerations.FaseTypeEnum.Voetganger:
                                        if (l.SimulatedStations[0].Type != RISStationTypeSimEnum.PEDESTRIAN) l.SimulatedStations[0].Type = RISStationTypeSimEnum.PEDESTRIAN;
                                        break;
                                    case TLCGen.Models.Enumerations.FaseTypeEnum.OV:
                                        if (l.SimulatedStations[0].Type != RISStationTypeSimEnum.BUS) l.SimulatedStations[0].Type = RISStationTypeSimEnum.BUS;
                                        break;
                                }
                            }
                        }
                    }
                }
                UpdateRISLanes();
                for (int l = 0; l < RISLanes.Count; l++)
                {
                    RISLanes[l].LaneID = l;
                }
            }
        }

        public string SystemITF
        {
            get => _RISModel.SystemITF;
            set
            {
                _RISModel.SystemITF = value;
                RaisePropertyChanged<object>(broadcast: true);
            }
        }
        
        private AddRemoveItemsManager<RISLaneRequestExtendDataViewModel, RISLaneRequestExtendDataModel, string> _lanesRequestExtendManager;
        public AddRemoveItemsManager<RISLaneRequestExtendDataViewModel, RISLaneRequestExtendDataModel, string> LanesRequestExtendManager =>
            _lanesRequestExtendManager ??
            (_lanesRequestExtendManager = new AddRemoveItemsManager<RISLaneRequestExtendDataViewModel, RISLaneRequestExtendDataModel, string>(
                RISRequestExtendLanes,
                x =>
                {
                    if (!RISFasen.Any()) return null;
                    var lre = new RISLaneRequestExtendDataViewModel(new RISLaneRequestExtendDataModel()
                    {
                        SignalGroupName = RISRequestExtendLanes.Any() ? RISRequestExtendLanes.Last().SignalGroupName : RISFasen.First().FaseCyclus,
                        RijstrookIndex = 1,
                        Type = GetTypeForFase(null, RISRequestExtendLanes.Any() ? RISRequestExtendLanes.Last().SignalGroupName : RISFasen.First().FaseCyclus)
                    });
                    return lre;
                },
                (x, y) => false
                ));

        #endregion // Properties

        #region Commands

        private GalaSoft.MvvmLight.CommandWpf.RelayCommand _addDefaultRequestExtendLanesCommand;
        public ICommand AddDefaultRequestExtendLanesCommand => _addDefaultRequestExtendLanesCommand ?? (_addDefaultRequestExtendLanesCommand = new RelayCommand(AddDefaultRequestExtendLanesCommand_executed));

        private void AddDefaultRequestExtendLanesCommand_executed()
        {
            foreach(var fc in _plugin.Controller.Fasen)
            {
                var t = GetTypeForFase(fc);
                for (int i = 0; i < fc.AantalRijstroken; i++)
                {
                    if (RISRequestExtendLanes.All(x => x.SignalGroupName != fc.Naam || x.SignalGroupName == fc.Naam && x.RijstrookIndex != i + 1 || x.SignalGroupName == fc.Naam && x.RijstrookIndex == i + 1 && x.Type != t))
                    {
                        RISRequestExtendLanes.Add(new RISLaneRequestExtendDataViewModel(new RISLaneRequestExtendDataModel
                        {
                            SignalGroupName = fc.Naam,
                            RijstrookIndex = i + 1,
                            Type = t
                        }));
                    }
                }
            }
            RISRequestExtendLanes.BubbleSort();
        }

        #endregion // Commands

        private RISStationTypeEnum GetTypeForFase(FaseCyclusModel fc, string faseName = null)
        {
            if (fc == null)
            {
                fc = _plugin.Controller.Fasen.FirstOrDefault(x => x.Naam == faseName);
            }
            RISStationTypeEnum t = RISStationTypeEnum.UNKNOWN;
            if (fc == null) return t;
            switch (fc.Type)
            {
                case TLCGen.Models.Enumerations.FaseTypeEnum.Auto:
                    t = RISStationTypeEnum.MOTORVEHICLES;
                    break;
                case TLCGen.Models.Enumerations.FaseTypeEnum.Fiets:
                    t = RISStationTypeEnum.CYCLIST;
                    break;
                case TLCGen.Models.Enumerations.FaseTypeEnum.Voetganger:
                    t = RISStationTypeEnum.PEDESTRIAN;
                    break;
                case TLCGen.Models.Enumerations.FaseTypeEnum.OV:
                    t = RISStationTypeEnum.MOTORVEHICLES;
                    break;
            }
            return t;
        }

        #region TLCGen messaging

        private void OnFasenChanged(FasenChangedMessage msg)
        {
            if(msg.RemovedFasen != null && msg.RemovedFasen.Any())
            {
                foreach(var fc in msg.RemovedFasen)
                {
                    var RISFc = RISFasen.FirstOrDefault(x => x.FaseCyclus == fc.Naam);
                    if(RISFc != null)
                    {
                        RISFasen.Remove(RISFc);
                    }
                }
            }
            if (msg.AddedFasen != null && msg.AddedFasen.Any())
            {
                foreach (var fc in msg.AddedFasen)
                {
                    var risfc = new RISFaseCyclusDataViewModel(
                                new RISFaseCyclusDataModel { FaseCyclus = fc.Naam });
                    for (int i = 0; i < fc.AantalRijstroken; i++)
                    {
                        var l = new RISFaseCyclusLaneDataViewModel(new RISFaseCyclusLaneDataModel() { SignalGroupName = fc.Naam, RijstrookIndex = i + 1 });
                        risfc.Lanes.Add(l);
                    }
                    RISFasen.Add(risfc);
                    RISRequestExtendLanes.Add(new RISLaneRequestExtendDataViewModel(new RISLaneRequestExtendDataModel
                    {
                        SignalGroupName = fc.Naam,
                        RijstrookIndex = 1,
                        Type = GetTypeForFase(fc)
                    }));
                    MessengerInstance.Send(new ControllerDataChangedMessage());
                }
            }
            RISFasen.BubbleSort();
            UpdateRISLanes();
        }

        private void OnNameChanged(NameChangedMessage msg)
        {
            if(msg.ObjectType == TLCGen.Models.Enumerations.TLCGenObjectTypeEnum.Fase)
            {
                TLCGenModelManager.Default.ChangeNameOnObject(RISModel, msg.OldName, msg.NewName);
                RISFasen.Rebuild();
                RISFasen.BubbleSort();
            }
        }

        private void OnAantalRijstrokenChanged(FaseAantalRijstrokenChangedMessage obj)
        {
            var risfc = RISFasen.FirstOrDefault(x => x.FaseCyclus == obj.Fase.Naam);
            if (risfc != null)
            {
                if(obj.AantalRijstroken > risfc.Lanes.Count)
                {
                    var i = risfc.Lanes.Count;
                    for (; i < obj.AantalRijstroken; i++)
                    {
                        var l = new RISFaseCyclusLaneDataViewModel(new RISFaseCyclusLaneDataModel() { SignalGroupName = obj.Fase.Naam, RijstrookIndex = i + 1 });
                        risfc.Lanes.Add(l);
                    }
                }
                else if (obj.AantalRijstroken < risfc.Lanes.Count)
                {
                    var i = risfc.Lanes.Count - obj.AantalRijstroken;
                    for (int j = 0; j < i; j++)
                    {
                        if (risfc.Lanes.Any())
                        {
                            risfc.Lanes.Remove(risfc.Lanes.Last());
                        }
                    }
                    var rem = RISRequestExtendLanes.Where(x => x.SignalGroupName == obj.Fase.Naam && x.RijstrookIndex >= obj.AantalRijstroken).ToList();
                    foreach (var r in rem) RISRequestExtendLanes.Remove(r);
                }
            }
            foreach (var lre in RISRequestExtendLanes.Where(x => x.SignalGroupName == obj.Fase.Naam)) lre.UpdateRijstroken();
            UpdateRISLanes();
        }

        #endregion // TLCGen messaging

        #region Private Methods 

        internal void UpdateRISLanes()
        {
            RISLanes.Clear();
            foreach (var fc in RISFasen)
            {
                foreach (var l in fc.Lanes)
                {
                    RISLanes.Add(l);
                }
            }
        }

        #endregion // Private Methods 

        #region Public Methods

        public void UpdateMessaging()
        {
            MessengerInstance.Register<FasenChangedMessage>(this, OnFasenChanged);
            MessengerInstance.Register<NameChangedMessage>(this, OnNameChanged);
            MessengerInstance.Register<FaseAantalRijstrokenChangedMessage>(this, OnAantalRijstrokenChanged);
        }

        #endregion // Public Methods

        #region Constructor

        public RISTabViewModel(RISPlugin plugin)
        {
            _plugin = plugin;
        }

        #endregion // Constructor
    }
}
