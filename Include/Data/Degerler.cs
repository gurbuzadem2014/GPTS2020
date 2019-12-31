using System;
using System.Collections.Generic;
using System.Text;


using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace GPTS.Include.Data
{
    public class Degerler
    {
        public static string mesajbaslik = "İşletme Takip Sistemi";
        public static string YedekSaati = "17:59:01";
        public static string GunlukKurSaati = "15:32:01";
        public static string YedekYolu = "";
        public static string SirketAdi = "HititYazilim";
        public static string SirketVkn = "9000068418";//"1111111104";
        public static bool ilktarih_devir_sontarih = false;
        public static string SirketVDaire = "GEBZE";
        public static string BilgisayarAdi = "BilgisayarAdi";
        public static string ip_adres = "126.0.0.1";
        public static string Versiyon = "20.08.2016";
        public static int iAktifForm = 0;
        public static bool isProf = true;

        public static string eposta = "";
        public static string epostacc = "";
        public static string epostabcc = "";
        
        public static int iFaturaTipiAlis = 0;
        public static int fkSatisDurumu = 2;
        public static bool AnaBilgisayar = false;
        public static int select_top_firma = 20;
        public static int select_top_tedarikci = 20;

        public static int fkDepolar = 1;
        public static int fkSube = 1;
        public static bool DepoKullaniyorum = false;
        public static bool StokKartiDizayn = false;

        public static string fkKullaniciGruplari = "1";
        public static int fkSatisFiyatlariBaslik = 1;
        


        public static string AracdaSatis = "1";

        public static bool acilista_hatirlatma_ekrani = false;
        public static bool caller_id_sirket = false;
        public static bool caller_id_kullanici = false;

        public static string smskullaniciadi = "demo";
        public static string smssifre = "demo";
        public static string smsbaslik = "baslik";
        public static string smsKullaniciNo = "0";
        
        public static int kdvorani = 18;
        public static int kdvorani_alis = 18; 
        public static string stokbirimi = "ADET";
        public static short CopyAdetYazdirmaAdedi = 1;
        public static string OzelSifre = "9";
        public static string OzelSifreYazilim = "9999*";
        public static bool stokkartisescal = true;
        public static bool Uruneklendisescal = false;
        public static bool Satiskopyala = false;
        public static bool EnableSsl = false;
        public static bool OncekiFiyatHatirla = false;
        public static bool OncekiFiyatHatirla_teda = false;
        public static bool MusteriZorunluUyari = false;

        public static string odemesekli = "Nakit";
        public static DateTime LisansBitisTarih = DateTime.Today.AddYears(10);
        public static int fkDonemler = 1;
        public static int fkKasalar = 1;
        
        public static int sessiotimeoutsuresi_ = 10;//300; //60*5;
        public static int sessiotimeout_ = 0; //60*5;

        public static int fkilKoduDefault = 41;//il
        public static int fkilceAltGrupDefault = 7;//ilçe
        public static bool giris_yapildi = false;
        public static bool isYenile = false;
        
        public static string dakikam = "0";

        public static string e_fatura_kul = "Uyumsoft";
        public static string e_fatura_sifre = "Uyumsoft";
        public static string e_fatura_url = "http://efatura-test.uyumsoft.com.tr/Services/BasicIntegration";
        public static string ConvertToXml(object obj)
        {
            if (obj == null) { return null; }
            try
            {
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.Encoding = Encoding.UTF8;
                xws.Indent = true;
                xws.IndentChars = " ";
                xws.NewLineHandling = NewLineHandling.Replace;
                XmlSerializer xs = new XmlSerializer(obj.GetType());
                using (MemoryStream s = new MemoryStream())
                {
                    using (XmlWriter xw = XmlTextWriter.Create(s, xws)) { xs.Serialize(xw, obj); }
                    s.Seek(0, SeekOrigin.Begin);
                    using (StreamReader sr = new StreamReader(s)) { return sr.ReadToEnd(); }
                }

            }
            catch { return null; }
        }

        public static bool isKurlariAl = false;
        public static bool isHatirlatmaUyar = false;
        public static bool isHatirlatmaUyarEkran = false;
        public static int AnimsatmaSaniyeInterval = 2000;
        public static bool isHatirlatmaAcikmi = false;
        public static bool StokKartiKapatEkle = false;
        public static bool StokKartiF7Ekle = false;

        public static int fkBirimler = 1;
        public static bool makbuzyazdir = false;
        #region Sabit Degerler

        public enum SatisDurumlari
        {
            Teklif = 1,
            Satış = 2,
            İrsaliye = 3,
            Fatura = 4,
            Değişim = 5,
            Devir = 6,
            Bos = 7,
            İptal = 8,
            İade = 9,
            Sipariş = 10,
            SFatura = 11,
            Tevkifat = 12,
            EFatura = 13,
        }

        public enum OdemeSekli
        {
            Nakit=0,
            KrediKarti =1,
            AcikHesap=2,
            Diger = 3,
            Banka=4,
            Sodexo=5
        }

        public enum islemDurumu
        {
            Basarili = 1,
            Basarisiz = 2,
            Bulunamadi = 3
        }
        public enum KasaTipi
        {
            KasaGirisi = 1,
            KasaCikisi = 2,
            BankaGirisi = 3,
            AidatGirisi=10
        }
        public enum GelirGider
        {
            Gelir = 1,
            Gider = 2
        }
        public enum BelgeDurumu
        {
            Yeni = 1,
            Düzenleme = 2,
            Yazdırma = 3,
            Gönderilme = 4,
            iade = 5,
            Kapatma = 6,
            Tahsil = 7,
            iptal=8
        }

        public enum Moduller
        {
            Satis = 1,
            StokKarti = 2, 
            Kasa = 3,
            FirmaTakip = 4,
            PersonelTakip = 5,
            Alis = 11,
            SatisRaporlari=16,
            SatisRaporlariTumu = 161
        }

        public enum Renkler
        { 
            Yok = 0,
            Beyaz = 1,
            Kırmızı = 2,
            Sarı = 3,
            Yeşil = 4,
            Mavi = 5,
            Siyah = 6
        }

        public enum ZamanDilimi
        {
            Saniye = 1,
            Dakika = 2,
            Saat = 3,
            Gun = 4,
            Ay = 5,
            Yil = 6
        }
        #endregion
    }

    public static class loggedIn
    {
        public static String username = "48082719380";
        public static String password = "123456";
        public static String representedHealthcareOrganizationID = "14555";
        public static String organizationUserID = "19315957022";
    }

    public enum FormAdlari
    {
        SatisFormu = 1,
        SenetFormu = 2,
        CekFormu = 3
    }

    public enum ActionCodes
    {
        Normal = 1,//Normal Zaman
        Closed = 2,//Kapalý
        WeekendHoliday = 4,//Hafta Sonu tatili
        Permission = 5,//	Izin
        Turn = 6,//Nöbet
        NewYear = 7,//	Yilbasi
        NationalFestival = 8,//	Ulusal Bayram
        RreligiousFestival = 9//	Dini Bayram
    }

    public enum AppointmentRequestTypes
    {
        DoctorSelection = 1,//	Doktor Seçimi
        CitySelection = 2,//	Il Seçimi
        EnterpriseSelection = 3,//	Kurum Seçimi
        None = 4,//	Hiçbiri
    }

    public enum AppointmentStates
    {
        DidNotCome = 2,//	Gelmedi
        Came = 1,//	Geldi
        Pending = 3,//	Beklemede
        Cancel = 4	//Iptal
    }

    public enum PhoneTypes
    {
        Home = 1,//	Ev Telefonu
        Work = 2,//	Is Telefonu
        Mobile = 3,//	Cep Telefonu
        ParentsPhone = 4//	Velisinin\Yakininin Telefonu
    }

    public static class CodeIds
    {
        public const string IdentityNumber =
            "2.16.840.1.113883.3.129.1.1.1";
        public const string RegistrationNumber =
            "2.16.840.1.113883.3.129.1.1.2";
        public const string SGKRationBookId =
            "2.16.840.1.113883.3.129.1.1.3";
        public const string ProtocolNumber =
            "2.16.840.1.113883.3.129.1.1.4";
        public const string Application =
            "2.16.840.1.113883.3.129.1.1.5";
        public const string Organizations =
            "2.16.840.1.113883.3.129.1.1.6";

        public const string SUT =
            "2.16.840.1.113883.3.129.1.2.2";
        public const string Medications =
            "2.16.840.1.113883.3.129.1.2.3";
        public const string Occupations =
            "2.16.840.1.113883.3.129.1.2.4";
        public const string Addresses =
            "2.16.840.1.113883.3.129.1.2.5";
        public const string DefiniteCaseDiagnosisCriterion =
            "2.16.840.1.113883.3.129.1.2.6";
        public const string AdmissionType =
            "2.16.840.1.113883.3.129.1.2.7";
        public const string ReportType =
            "2.16.840.1.113883.3.129.1.2.8";
        public const string DischargeType =
            "2.16.840.1.113883.3.129.1.2.9";
        public const string MobileServiceState =
            "2.16.840.1.113883.3.129.1.2.10";

        public const string SocialSecurity =
            "2.16.840.1.113883.3.129.1.2.11";
        public const string BloodType =
            "2.16.840.1.113883.3.129.1.2.12";
        public const string DisabilityState =
            "2.16.840.1.113883.3.129.1.2.13";
        public const string EducationState =
            "2.16.840.1.113883.3.129.1.2.14";
        public const string WorkState =
            "2.16.840.1.113883.3.129.1.2.15";
        public const string Smooking =
            "2.16.840.1.113883.3.129.1.2.16";
        public const string MaritalState =
            "2.16.840.1.113883.3.129.1.2.17";
        public const string AddressType =
            "2.16.840.1.113883.3.129.1.2.18";
        public const string CommunicationType =
            "2.16.840.1.113883.3.129.1.2.19";
        public const string ToothCode =
            "2.16.840.1.113883.3.129.1.2.20";

        public const string DeathLocation =
            "2.16.840.1.113883.3.129.1.2.22";
        public const string DeathReasonType =
            "2.16.840.1.113883.3.129.1.2.23";
        public const string MohterDeath =
            "2.16.840.1.113883.3.129.1.2.24";
        public const string APGAR1 =
            "2.16.840.1.113883.3.129.1.2.25";
        public const string APGAR5 =
            "2.16.840.1.113883.3.129.1.2.26";
        public const string BabyHealthOperation =
            "2.16.840.1.113883.3.129.1.2.27";
        public const string CongenitalAnomalyBirth =
            "2.16.840.1.113883.3.129.1.2.28";
        public const string WomanHealthActivity =
            "2.16.840.1.113883.3.129.1.2.29";
        public const string Hemoglobin =
            "2.16.840.1.113883.3.129.1.2.30";

        public const string ContraceptiveMethod =
            "2.16.840.1.113883.3.129.1.2.31";
        public const string NotUsageOfContraceptiveReason =
            "2.16.840.1.113883.3.129.1.2.32";
        public const string PregnancyPuerperantDangerSign =
            "2.16.840.1.113883.3.129.1.2.33";
        public const string BirthMethod =
            "2.16.840.1.113883.3.129.1.2.34";
        public const string PlaceOfBirth =
            "2.16.840.1.113883.3.129.1.2.35";
        public const string ProteinAtUrine =
            "2.16.840.1.113883.3.129.1.2.36";
        public const string FoetusHeartSound =
            "2.16.840.1.113883.3.129.1.2.37";
        public const string RiskFactorAtPregnancy =
            "2.16.840.1.113883.3.129.1.2.38";
        public const string BirthHelper =
            "2.16.840.1.113883.3.129.1.2.39";
        public const string PregnancyResult =
            "2.16.840.1.113883.3.129.1.2.40";

        public const string TreatmentStory =
            "2.16.840.1.113883.3.129.1.2.41";
        public const string EpidemiologicalRelationWithOtherCase =
            "2.16.840.1.113883.3.129.1.2.42";
        public const string DiseaseMajorLocation =
            "2.16.840.1.113883.3.129.1.2.43";
        public const string DiseaseMinorLocation =
            "2.16.840.1.113883.3.129.1.2.44";
        public const string DrugResistance =
            "2.16.840.1.113883.3.129.1.2.45";
        public const string HResistance =
            "2.16.840.1.113883.3.129.1.2.46";
        public const string MedicationRoute =
            "2.16.840.1.113883.3.129.1.2.47";
        public const string PrescriptionType =
            "2.16.840.1.113883.3.129.1.2.48";
        public const string Urgency =
            "2.16.840.1.113883.3.129.1.2.49";
        public const string PaymentType =
            "2.16.840.1.113883.3.129.1.2.50";

        public const string OuterAgentPlace =
            "2.16.840.1.113883.3.129.1.2.51";
        public const string Nationality =
            "2.16.840.1.113883.3.129.1.2.52";
        public const string CaseType =
            "2.16.840.1.113883.3.129.1.2.53";
        public const string Vaccine =
            "2.16.840.1.113883.3.129.1.2.54";
        public const string MedicalNutritionAdaptation =
            "2.16.840.1.113883.3.129.1.2.55";
        public const string ThyroidExamination =
            "2.16.840.1.113883.3.129.1.2.56";
        public const string AdditionalDiseasesWithDiabetes =
            "2.16.840.1.113883.3.129.1.2.57";
        public const string Exercise =
            "2.16.840.1.113883.3.129.1.2.58";
        public const string DiagnosisMethod =
            "2.16.840.1.113883.3.129.1.2.59";
        public const string TumourPlace =
            "2.16.840.1.113883.3.129.1.2.60";

        public const string HistologicalType =
            "2.16.840.1.113883.3.129.1.2.61";
        public const string SeerSummaryPhase =
            "2.16.840.1.113883.3.129.1.2.62";
        public const string TreatmentMethod =
            "2.16.840.1.113883.3.129.1.2.63";
        public const string Laterality =
            "2.16.840.1.113883.3.129.1.2.64";
        public const string RisksAtBrainProgress =
            "2.16.840.1.113883.3.129.1.2.65";
        public const string TrainingForPsychoRisks =
            "2.16.840.1.113883.3.129.1.2.66";
        public const string ProceduresForRiskFactors =
            "2.16.840.1.113883.3.129.1.2.67";
        public const string ParentActForPsychoPrg =
            "2.16.840.1.113883.3.129.1.2.68";
        public const string FollowForRiskyCase =
            "2.16.840.1.113883.3.129.1.2.69";
        public const string QueryForProgressTable =
            "2.16.840.1.113883.3.129.1.2.70";

        public const string I71 =
            "2.16.840.1.113883.3.129.1.2.71";
        public const string PluralPrimaryStatus =
            "2.16.840.1.113883.3.129.1.2.72";
        public const string AlcoholUsage =
            "2.16.840.1.113883.3.129.1.2.73";
        public const string DrugAddiction =
            "2.16.840.1.113883.3.129.1.2.74";
        public const string SurgeryHistory =
            "2.16.840.1.113883.3.129.1.2.75";
        public const string InjuryHistory =
            "2.16.840.1.113883.3.129.1.2.76";
        public const string ConfidentialityCode =
            "2.16.840.1.113883.3.129.1.2.77";
        public const string LotNumber =
            "2.16.840.1.113883.3.129.1.2.78";
        public const string AlternativeHemodialysisTreatment =
            "2.16.840.1.113883.3.129.1.2.79";
        public const string AnaemiaTreatment =
            "2.16.840.1.113883.3.129.1.2.80";

        public const string DialysisTreatment =
            "2.16.840.1.113883.3.129.1.2.81";
        public const string DialysisFrequency =
            "2.16.840.1.113883.3.129.1.2.82";
        public const string TreatmentCourse =
            "2.16.840.1.113883.3.129.1.2.83";
        public const string VitaminDTreatment =
            "2.16.840.1.113883.3.129.1.2.84";

        public const string RoleCode =
            "2.16.840.1.113883.5.111";

        public const string ICD10 =
            "2.16.840.1.113883.6.3";

        public const string Klinikler =
            "2.16.840.1.113883.3.129.1.2.1";
        public const string Cinsiyet =
            "2.16.840.1.113883.3.129.1.2.21";
        public const string Iller =
            "2.16.840.1.113883.3.129.1.2.85";
        public const string Ilceler =
            "2.16.840.1.113883.3.129.1.2.86";

        public const string TalepSekli =
            "2.16.840.1.113883.3.129.4.1";
        public const string RandevuDurumKodu =
            "2.16.840.1.113883.3.129.4.2";
        public const string AksiyonKodu =
            "2.16.840.1.113883.3.129.4.3";
        public const string HastaTipi =
            "2.16.840.1.113883.3.129.4.4";
        public const string SablonDurumu =
            "2.16.840.1.113883.3.129.4.5";
        public const string RandevuKayitDurum =
            "2.16.840.1.113883.3.129.4.6";
        public const string RandevuVerilisSekli =
            "2.16.840.1.113883.3.129.4.7";
        public const string RandevuTipi =
            "2.16.840.1.113883.3.129.4.8";

        public const string Interaction =
            "2.16.840.1.113883.3.129.3.1.1";
        public const string Mesaj =
            "2.16.840.1.113883.3.129.3.1.2";
        public const string SistemHata =
            "2.16.840.1.113883.3.129.3.1.3";
        public const string UygulamaHata =
            "2.16.840.1.113883.3.129.3.1.4";
        public const string HRN =
            "2.16.840.1.113883.3.129.3.2.1";
        public const string SablonId =
            "2.16.840.1.113883.3.129.3.2.2";
        public const string SablonDetayId =
            "2.16.840.1.113883.3.129.3.2.3";
        public const string PoliklinikDetayId =
            "2.16.840.1.113883.3.129.3.2.4";
        public const string PoliklinikDetayKod =
            "2.16.840.1.113883.3.129.3.2.5";
        public const string CalismaCetveliId =
            "2.16.840.1.113883.3.129.3.2.6";

    }

    public static class GPTSConfig
    {

        private static string usbs = "USBS";
        public static string USBS
        {
            get { return usbs; }
            set { usbs = value; }
        }

        private static string ministryOfHealth = "5881";
        public static string MinistryOfHealth
        {
            get { return ministryOfHealth; }
            set { ministryOfHealth = value; }
        }

        private static string companyName = "İSTANBUL ÜMRANİYE EĞİTİM VE ARAŞTIRMA HASTANESİ";
        public static string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        private static string companyCode = "";
        public static string CompanyCode
        {
            get { return companyCode; }
            set { companyCode = value; }
        }

        private static string applicationName = "BilbestGPTS";
        public static string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }

        private static string applicationCode = "BilbestGPTS";
        public static string ApplicationCode
        {
            get { return applicationCode; }
            set { applicationCode = value; }
        }

        private static string application = "BilbestGPTS";
        public static string Application
        {
            get { return application; }
            set { application = value; }
        }

        private static string serviceAddress = "GPTSws.sagliknet.saglik.gov.tr";
        public static string ServiceAddress
        {
            get { return serviceAddress; }
            set { serviceAddress = value; }
        }

        private static string serviceCertificate = "sagliknet_1.crt"; // sagliknet.saglik.gov.tr.crt, sagliknet.cer
        public static string ServiceCertificate
        {
            get { return serviceCertificate; }
            set { serviceCertificate = value; }
        }

        private static bool debugInfo = false;
        public static bool DebugInfo
        {
            get { return debugInfo; }
            set { debugInfo = value; }
        }

        static GPTSConfig()
        {

        }

    }
}
