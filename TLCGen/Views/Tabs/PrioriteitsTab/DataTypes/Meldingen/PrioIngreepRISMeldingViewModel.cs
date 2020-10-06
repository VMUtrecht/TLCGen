﻿using GalaSoft.MvvmLight;
using TLCGen.Models.Enumerations;

namespace TLCGen.ViewModels
{
    public class PrioIngreepRISMeldingViewModel : ViewModelBase
    {
        #region Fields
        #endregion // Fields
        
        #region Properties
        
        public PrioIngreepInUitMeldingViewModel Parent { get; }

        public PrioIngreepInUitMeldingTypeEnum InUit => Parent.InUit;

        public int RisStart
        {
            get => Parent.PrioIngreepInUitMelding.RisStart;
            set 
            { 
                Parent.PrioIngreepInUitMelding.RisStart = value; 
                RaisePropertyChanged();
            }
        }

        public int RisEnd
        {
            get => Parent.PrioIngreepInUitMelding.RisEnd;
            set 
            { 
                Parent.PrioIngreepInUitMelding.RisEnd = value; 
                RaisePropertyChanged();
            }
        }

        #endregion // Properties

        #region Constructor

        public PrioIngreepRISMeldingViewModel(PrioIngreepInUitMeldingViewModel parent)
        {
            Parent = parent;
        }
        
        #endregion // Constructor
    }
}