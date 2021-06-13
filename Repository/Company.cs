using System;

namespace Repository
{
    public class Company
    {
        private string _name { get; set; } = default!; // nimi [0]
        private int _code { get; set; } = default!; // ariregistri_kood [1]
        private string _legalForm { get; set; } = default!; // ettevotja_oiguslik_vorm [2]
        private string _legalFormSubType { get; set; } = default!; // ettevotja_oigusliku_vormi_alaliik [3]
        private string _vatId { get; set; } = default!; // kmkr_nr [4]
        private string _status { get; set; } = default!; // ettevotja_staatus [5]
        private string _statusString { get; set; } = default!; // ettevotja_staatus_tekstina [6]
        private DateTime _firstEntry { get; set; } = default!; // ettevotja_esmakande_kpv [7]
        private string _fullAddress { get; set; } = default!; // ads_normaliseeritud_taisaadress [15]
        private int _postalCode { get; set; } = default!; // indeks_ettevotja_aadressis [12]
        private string _link { get; set; } = default!; // teabesysteemi_link [16]
    }
}