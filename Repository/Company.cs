using System;

namespace Repository
{
    public class Company
    {
        public Company(string name, int code, string legalForm, string status, DateTime firstEntry, string fullAddress, int postalCode, string link)
        {
            Name = name;
            _code = code;
            _legalForm = legalForm;
            _status = status;
            _firstEntry = firstEntry;
            _fullAddress = fullAddress;
            _postalCode = postalCode;
            _link = link;
        }

        public Company()
        {
            
        }
        
        public string Name { get; set; } = default!; // nimi [0]
        private int _code { get; set; } = default!; // ariregistri_kood [1]
        private string _legalForm { get; set; } = default!; // ettevotja_oiguslik_vorm [2]
        private string _legalFormSubType { get; set; } = default!; // ettevotja_oigusliku_vormi_alaliik [3]
        private string _vatId { get; set; } = default!; // kmkr_nr [4]
        private string _status { get; set; } = default!; // ettevotja_staatus_tekstina [6]
        private DateTime _firstEntry { get; set; } = default!; // ettevotja_esmakande_kpv [7]
        private string _fullAddress { get; set; } = default!; // ads_normaliseeritud_taisaadress [15]
        private int _postalCode { get; set; } = default!; // indeks_ettevotja_aadressis [12]
        private string _link { get; set; } = default!; // teabesysteemi_link [16]

        public override string ToString()
        {
            return $"{Name}, {_code}";
        }
    }
}