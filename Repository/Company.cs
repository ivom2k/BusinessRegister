using System;

namespace Repository
{
    public class Company
    {
        private string _name { get; set; } = default!; // nimi
        private int _code { get; set; } = default!; // ariregistri_kood
        private string _legalForm { get; set; } = default!; // ettevotja_oiguslik_vorm
        private string _legalFormSubType { get; set; } = default!; // ettevotja_oigusliku_vormi_alaliik
        private string _vatId { get; set; } = default!; // kmkr_nr
        private string _status { get; set; } = default!; // ettevotja_staatus
        private string _statusString { get; set; } = default!; // ettevotja_staatus_tekstina
        private DateTime _firstEntry { get; set; } = default!; // ettevotja_esmakande_kpv
        private string _fullAddress { get; set; } = default!; // ads_normaliseeritud_taisaadress
        private int _postalCode { get; set; } = default!; // indeks_ettevotja_aadressis
        private string _link { get; set; } = default!; // teabesysteemi_link
    }
}