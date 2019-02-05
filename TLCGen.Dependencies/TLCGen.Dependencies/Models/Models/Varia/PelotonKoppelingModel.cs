﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using TLCGen.Models;
using TLCGen.Models.Enumerations;

namespace TLCGen.Models
{
    public class PelotonKoppelingModel
    {
        [HasDefault(false)]
        public string KruisingNaam { get; set; }

        [RefersTo]
        [HasDefault(false)]
        public string GekoppeldeSignaalGroep { get; set; }

        public int Meetperiode { get; set; }
        public int MaximaalHiaat { get; set; }
        public int MinimaalAantalVoertuigen { get; set; }
        public int TijdTotAanvraag { get; set; }
        public int TijdTotRetourWachtgroen { get; set; }
        public int TijdRetourWachtgroen { get; set; }
        public NooitAltijdAanUitEnum ToepassenAanvraag { get; set; }
        public NooitAltijdAanUitEnum ToepassenMeetkriterium { get; set; }
        public NooitAltijdAanUitEnum ToepassenRetourWachtgroen { get; set; }

        [RefersTo]
        [HasDefault(false)]
        public string PTPKruising { get; set; }

        public HalfstarGekoppeldWijzeEnum KoppelWijze { get; set; }

        public PelotonKoppelingRichtingEnum Richting { get; set; }

        [XmlArray(ElementName = "PelotonKoppelingDetector")]
        public List<PelotonKoppelingDetectorModel> Detectoren { get; set; }

	    [IOElement("pel", BitmappedItemTypeEnum.Uitgang, "GekoppeldeSignaalGroep", "IsInkomend")]
        public BitmapCoordinatenDataModel InkomendVerklikking { get; set; }

        [Browsable(false)]
        public bool IsInkomend => Richting == PelotonKoppelingRichtingEnum.Inkomend;

        public PelotonKoppelingModel()
        {
            Detectoren = new List<PelotonKoppelingDetectorModel>();
            InkomendVerklikking = new BitmapCoordinatenDataModel();
        }
    }

    public class PelotonKoppelingDetectorModel
    {
        [RefersTo]
        public string DetectorNaam { get; set; }
        public int KoppelSignaal { get; set; }
    }
}