using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPTS.Entegrasyon
{
    public class SatisDetay
    {
        [DisplayName("Id")]
        public int pkSatisDetay { get; set; }

        [DisplayName("UrunAdi")]
        public string UrunAdi { get; set; }

        [DisplayName("Birimi")]
        public string Birimi { get; set; }

        [DisplayName("Marka")]
        public string Marka { get; set; }

        [DisplayName("Model")]
        public string Model { get; set; }

        [DisplayName("Aciklama")]
        public string Aciklama { get; set; }

        [DisplayName("Barkod")]
        public string Barkod { get; set; }

        [DisplayName("Fiyat")]
        public decimal Fiyat { get; set; }

        [DisplayName("Miktar")]
        public int Miktar { get; set; }

        [DisplayName("Kdv Orani")]
        public decimal KdvOrani { get; set; }

        [DisplayName("Kdv Tutar")]
        public decimal KdvTutar { get; set; }

        [DisplayName("Toplam Tutar")]
        public decimal ToplamTutar { get; set; }


        [DisplayName("iskontoOrani")]
        public decimal iskontoOrani { get; set; }

        [DisplayName("iskontoTutar")]
        public decimal iskontoTutar { get; set; }
    }
}
