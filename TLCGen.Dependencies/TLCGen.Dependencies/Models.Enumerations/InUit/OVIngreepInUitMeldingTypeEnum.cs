﻿using System.ComponentModel;
using TLCGen.Helpers;

namespace TLCGen.Models.Enumerations
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PrioIngreepInUitMeldingTypeEnum
    {
        Inmelding,
        Uitmelding
    }
}
