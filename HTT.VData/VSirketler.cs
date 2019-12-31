using HTT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTT.VData
{
    public class VSirketler
    {

        private Sirketler _Sirketler;
        private Sirketler Sirketler
        {
            get
            {
                if (_Sirketler == null)
                    _Sirketler = new Sirketler();
                return _Sirketler;
            }
            set
            {
                _Sirketler = value;
            }
        }
        public VSirketler() { }
        public VSirketler(Sirketler Sirket)
        {
            this.Sirketler = Sirket;
        }

        //[Display(Name = "Kullanıcı ID")]
        public int pkSirket
        {
            get
            {
                return Sirketler.pkSirket;
            }
            set
            {
                Sirketler.pkSirket = value;
            }
        }

        //[Display(Name = "Kullanıcı Kodu")]
        //[Required]
        public string Sirket
        {
            get
            {
                return Sirketler.Sirket;
            }
            set
            {
                Sirketler.Sirket = value;
            }
        }

        //[Display(Name = "Kullanıcı Kodu")]
        //[Required]
        public string Yetkili
        {
            get
            {
                return Sirketler.Yetkili;
            }
            set
            {
                Sirketler.Yetkili = value;
            }
        }

    }
}
