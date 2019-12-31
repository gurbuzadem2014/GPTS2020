using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GPTS.Entegrasyon
{
    public class Satislar
    {

        [DisplayName("Id")]
        public int pkSatislar { get; set; }

        [DisplayName("Fatura No"), Required, StringLength(60)]
        public string FaturaNo { get; set; }

        [DisplayName("Fatura Tarihi")]//, Required, StringLength(60)]
        public DateTime FaturaTarihi { get; set; }

        [DisplayName("Fatura Turu")]//, Required, StringLength(2000)]
        public string FaturaTuru { get; set; }

        [DisplayName("Alici Kapi No")]
        public int AliciKapiNo { get; set; }

        [DisplayName("Alici Daire No")]
        public int AliciDaireNo { get; set; }

        [DisplayName("Aciklama")]
        public string Aciklama { get; set; }

        //[DisplayName("Tutar")]
        //public int Tutar { get; set; }

        [DisplayName("FaturaTuru")]//, StringLength(2000)]
        public string FaturaTipi { get; set; }

        [DisplayName("Alici Soyad")]
        public string AliciSoyad { get; set; }
        
        [DisplayName("Alici Adı")]
        public string AliciAdi { get; set; }

        [DisplayName("Vergi No")]
        public string AliciVergiNo { get; set; }

        [DisplayName("Vergi Dairesi")]
        public string AliciVergiDairesi { get; set; }

        [DisplayName("Alici il")]
        public string Aliciil { get; set; }

        [DisplayName("Alici ilçe")]
        public string Aliciilce { get; set; }

        [DisplayName("AliciCaddeSokak")]
        public string AliciCaddeSokak { get; set; }

        [DisplayName("ePosta")]
        public string  Alici_ePosta { get; set; }

        [DisplayName("Tel")]
        public string  Alici_Tel { get; set; }
        [DisplayName("Web")]
        public string  Alici_Web { get; set; }
        [DisplayName("Fax")]
        public string  Alici_Fax { get; set; }


        [DisplayName("AraToplam")]
        public decimal AraToplam { get; set; }

        [DisplayName("Toplamiskonto")]
        public decimal Toplamiskonto { get; set; }

        [DisplayName("KdvToplamTutari")]
        public decimal KdvToplamTutari { get; set; }
        

        [DisplayName("Göncerici ilçe")]
        public string GoncericiIlce { get; set; }

        public virtual int kalem_sayisi { get; set; }
    }
}
