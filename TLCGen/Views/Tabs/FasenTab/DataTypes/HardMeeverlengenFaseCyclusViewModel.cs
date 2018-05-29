﻿using GalaSoft.MvvmLight;
using TLCGen.Helpers;
using TLCGen.Models;
using TLCGen.Models.Enumerations;

namespace TLCGen.ViewModels
{
    public class HardMeeverlengenFaseCyclusViewModel : ViewModelBase, IViewModelWithItem
    {
        #region Fields

        #endregion // Fields

        #region Properties

        public HardMeeverlengenFaseCyclusModel HardMeeverlengenFaseCyclus { get; }

        public string FaseCyclus => HardMeeverlengenFaseCyclus.FaseCyclus;

        public HardMeevelengenTypeEnum Type
        {
            get => HardMeeverlengenFaseCyclus.Type;
            set
            {
                HardMeeverlengenFaseCyclus.Type = value;
                RaisePropertyChanged<object>(broadcast: true);
            }
        }

        #endregion // Properties

        #region IViewModelWithItem

        public object GetItem()
        {
            return HardMeeverlengenFaseCyclus;
        }

        #endregion // IViewModelWithItem

        #region Constructor

        public HardMeeverlengenFaseCyclusViewModel(HardMeeverlengenFaseCyclusModel hardMeeverlengenFaseCyclus)
        {
            HardMeeverlengenFaseCyclus = hardMeeverlengenFaseCyclus;
        }

        #endregion // Constructor

    }
}
