﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GPTS.skrs2 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ws.esaglik.surat.com.tr/", ConfigurationName="skrs2.WSSKRSSistemler")]
    public interface WSSKRSSistemler {
        
        // CODEGEN: Parameter 'sistemlerOutput' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="sistemlerOutput")]
        GPTS.skrs2.SistemlerResponse Sistemler(GPTS.skrs2.SistemlerRequest request);
        
        // CODEGEN: Parameter 'sistemKodlariOutput' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="sistemKodlariOutput")]
        GPTS.skrs2.SistemKodlariResponse SistemKodlari(GPTS.skrs2.SistemKodlariRequest request);
        
        // CODEGEN: Parameter 'sistemKodlariOutput' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="sistemKodlariOutput")]
        GPTS.skrs2.SistemKodlariSayfaGetirResponse SistemKodlariSayfaGetir(GPTS.skrs2.SistemKodlariSayfaGetirRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.esaglik.surat.com.tr/")]
    public partial class wsskrsSistemlerResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private hata hataField;
        
        private responseSistemler[] sistemlerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public hata hata {
            get {
                return this.hataField;
            }
            set {
                this.hataField = value;
                this.RaisePropertyChanged("hata");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sistemler", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=1)]
        public responseSistemler[] sistemler {
            get {
                return this.sistemlerField;
            }
            set {
                this.sistemlerField = value;
                this.RaisePropertyChanged("sistemler");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.esaglik.surat.com.tr/")]
    public partial class hata : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string yerelHataAciklamasiField;
        
        private hataTanimi hataTanimiField;
        
        private hataOgesi[] yerelHataListesiField;
        
        private string throwableStackTraceField;
        
        private string yerelHataKapsamiField;
        
        private string yerelHataKoduField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string yerelHataAciklamasi {
            get {
                return this.yerelHataAciklamasiField;
            }
            set {
                this.yerelHataAciklamasiField = value;
                this.RaisePropertyChanged("yerelHataAciklamasi");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public hataTanimi hataTanimi {
            get {
                return this.hataTanimiField;
            }
            set {
                this.hataTanimiField = value;
                this.RaisePropertyChanged("hataTanimi");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("hata", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public hataOgesi[] yerelHataListesi {
            get {
                return this.yerelHataListesiField;
            }
            set {
                this.yerelHataListesiField = value;
                this.RaisePropertyChanged("yerelHataListesi");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string throwableStackTrace {
            get {
                return this.throwableStackTraceField;
            }
            set {
                this.throwableStackTraceField = value;
                this.RaisePropertyChanged("throwableStackTrace");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string yerelHataKapsami {
            get {
                return this.yerelHataKapsamiField;
            }
            set {
                this.yerelHataKapsamiField = value;
                this.RaisePropertyChanged("yerelHataKapsami");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string yerelHataKodu {
            get {
                return this.yerelHataKoduField;
            }
            set {
                this.yerelHataKoduField = value;
                this.RaisePropertyChanged("yerelHataKodu");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.esaglik.surat.com.tr/")]
    public partial class hataTanimi : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string genelHataAciklamasiField;
        
        private string genelHataKapsamiField;
        
        private string genelHataKoduField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string genelHataAciklamasi {
            get {
                return this.genelHataAciklamasiField;
            }
            set {
                this.genelHataAciklamasiField = value;
                this.RaisePropertyChanged("genelHataAciklamasi");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string genelHataKapsami {
            get {
                return this.genelHataKapsamiField;
            }
            set {
                this.genelHataKapsamiField = value;
                this.RaisePropertyChanged("genelHataKapsami");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string genelHataKodu {
            get {
                return this.genelHataKoduField;
            }
            set {
                this.genelHataKoduField = value;
                this.RaisePropertyChanged("genelHataKodu");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.esaglik.surat.com.tr/")]
    public partial class kodDegeriKolonIcerigi : object, System.ComponentModel.INotifyPropertyChanged {
        
        private kodDegeriKolonIcerigiTipi kodDegeriKolonIcerigiTipiField;
        
        private bool kodDegeriKolonIcerigiTipiFieldSpecified;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public kodDegeriKolonIcerigiTipi kodDegeriKolonIcerigiTipi {
            get {
                return this.kodDegeriKolonIcerigiTipiField;
            }
            set {
                this.kodDegeriKolonIcerigiTipiField = value;
                this.RaisePropertyChanged("kodDegeriKolonIcerigiTipi");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool kodDegeriKolonIcerigiTipiSpecified {
            get {
                return this.kodDegeriKolonIcerigiTipiFieldSpecified;
            }
            set {
                this.kodDegeriKolonIcerigiTipiFieldSpecified = value;
                this.RaisePropertyChanged("kodDegeriKolonIcerigiTipiSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.esaglik.surat.com.tr/")]
    public enum kodDegeriKolonIcerigiTipi {
        
        /// <remarks/>
        NUMBER,
        
        /// <remarks/>
        BOOLEAN,
        
        /// <remarks/>
        TEXT,
        
        /// <remarks/>
        DATE,
        
        /// <remarks/>
        NULL,
        
        /// <remarks/>
        UNKNOWN,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.esaglik.surat.com.tr/")]
    public partial class kodDegeriKolonu : object, System.ComponentModel.INotifyPropertyChanged {
        
        private kodDegeriKolonIcerigi kolonIcerigiField;
        
        private string kolonAdiField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public kodDegeriKolonIcerigi kolonIcerigi {
            get {
                return this.kolonIcerigiField;
            }
            set {
                this.kolonIcerigiField = value;
                this.RaisePropertyChanged("kolonIcerigi");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string kolonAdi {
            get {
                return this.kolonAdiField;
            }
            set {
                this.kolonAdiField = value;
                this.RaisePropertyChanged("kolonAdi");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.esaglik.surat.com.tr/")]
    public partial class kodDegerleriResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private hata hataField;
        
        private kodDegeriKolonu[][] kodDegerleriField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public hata hata {
            get {
                return this.hataField;
            }
            set {
                this.hataField = value;
                this.RaisePropertyChanged("hata");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("kodDegeriSatirlari", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("kodDegeriKolonlari", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, NestingLevel=1)]
        public kodDegeriKolonu[][] kodDegerleri {
            get {
                return this.kodDegerleriField;
            }
            set {
                this.kodDegerleriField = value;
                this.RaisePropertyChanged("kodDegerleri");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.esaglik.surat.com.tr/")]
    public partial class responseSistemler : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string adiField;
        
        private bool aktifField;
        
        private bool aktifFieldSpecified;
        
        private System.DateTime guncellenmeTarihiField;
        
        private bool guncellenmeTarihiFieldSpecified;
        
        private hata hataField;
        
        private string hl7KoduField;
        
        private string koduField;
        
        private System.DateTime olusturulmaTarihiField;
        
        private bool olusturulmaTarihiFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string adi {
            get {
                return this.adiField;
            }
            set {
                this.adiField = value;
                this.RaisePropertyChanged("adi");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public bool aktif {
            get {
                return this.aktifField;
            }
            set {
                this.aktifField = value;
                this.RaisePropertyChanged("aktif");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool aktifSpecified {
            get {
                return this.aktifFieldSpecified;
            }
            set {
                this.aktifFieldSpecified = value;
                this.RaisePropertyChanged("aktifSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public System.DateTime guncellenmeTarihi {
            get {
                return this.guncellenmeTarihiField;
            }
            set {
                this.guncellenmeTarihiField = value;
                this.RaisePropertyChanged("guncellenmeTarihi");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool guncellenmeTarihiSpecified {
            get {
                return this.guncellenmeTarihiFieldSpecified;
            }
            set {
                this.guncellenmeTarihiFieldSpecified = value;
                this.RaisePropertyChanged("guncellenmeTarihiSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public hata hata {
            get {
                return this.hataField;
            }
            set {
                this.hataField = value;
                this.RaisePropertyChanged("hata");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string hl7Kodu {
            get {
                return this.hl7KoduField;
            }
            set {
                this.hl7KoduField = value;
                this.RaisePropertyChanged("hl7Kodu");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string kodu {
            get {
                return this.koduField;
            }
            set {
                this.koduField = value;
                this.RaisePropertyChanged("kodu");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public System.DateTime olusturulmaTarihi {
            get {
                return this.olusturulmaTarihiField;
            }
            set {
                this.olusturulmaTarihiField = value;
                this.RaisePropertyChanged("olusturulmaTarihi");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool olusturulmaTarihiSpecified {
            get {
                return this.olusturulmaTarihiFieldSpecified;
            }
            set {
                this.olusturulmaTarihiFieldSpecified = value;
                this.RaisePropertyChanged("olusturulmaTarihiSpecified");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.esaglik.surat.com.tr/")]
    public partial class hataOgesi : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string hataAciklamasiField;
        
        private string hataKapsamiField;
        
        private string hataKoduField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string hataAciklamasi {
            get {
                return this.hataAciklamasiField;
            }
            set {
                this.hataAciklamasiField = value;
                this.RaisePropertyChanged("hataAciklamasi");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string hataKapsami {
            get {
                return this.hataKapsamiField;
            }
            set {
                this.hataKapsamiField = value;
                this.RaisePropertyChanged("hataKapsami");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string hataKodu {
            get {
                return this.hataKoduField;
            }
            set {
                this.hataKoduField = value;
                this.RaisePropertyChanged("hataKodu");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Sistemler", WrapperNamespace="http://ws.esaglik.surat.com.tr/", IsWrapped=true)]
    public partial class SistemlerRequest {
        
        public SistemlerRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SistemlerResponse", WrapperNamespace="http://ws.esaglik.surat.com.tr/", IsWrapped=true)]
    public partial class SistemlerResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.esaglik.surat.com.tr/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public GPTS.skrs2.wsskrsSistemlerResponse sistemlerOutput;
        
        public SistemlerResponse() {
        }
        
        public SistemlerResponse(GPTS.skrs2.wsskrsSistemlerResponse sistemlerOutput) {
            this.sistemlerOutput = sistemlerOutput;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SistemKodlari", WrapperNamespace="http://ws.esaglik.surat.com.tr/", IsWrapped=true)]
    public partial class SistemKodlariRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.esaglik.surat.com.tr/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sistemKoduInput;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.esaglik.surat.com.tr/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime tarihInput;
        
        public SistemKodlariRequest() {
        }
        
        public SistemKodlariRequest(string sistemKoduInput, System.DateTime tarihInput) {
            this.sistemKoduInput = sistemKoduInput;
            this.tarihInput = tarihInput;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SistemKodlariResponse", WrapperNamespace="http://ws.esaglik.surat.com.tr/", IsWrapped=true)]
    public partial class SistemKodlariResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.esaglik.surat.com.tr/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public GPTS.skrs2.kodDegerleriResponse sistemKodlariOutput;
        
        public SistemKodlariResponse() {
        }
        
        public SistemKodlariResponse(GPTS.skrs2.kodDegerleriResponse sistemKodlariOutput) {
            this.sistemKodlariOutput = sistemKodlariOutput;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SistemKodlariSayfaGetir", WrapperNamespace="http://ws.esaglik.surat.com.tr/", IsWrapped=true)]
    public partial class SistemKodlariSayfaGetirRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.esaglik.surat.com.tr/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sistemKoduInput;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.esaglik.surat.com.tr/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime tarihInput;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.esaglik.surat.com.tr/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int skrsKod;
        
        public SistemKodlariSayfaGetirRequest() {
        }
        
        public SistemKodlariSayfaGetirRequest(string sistemKoduInput, System.DateTime tarihInput, int skrsKod) {
            this.sistemKoduInput = sistemKoduInput;
            this.tarihInput = tarihInput;
            this.skrsKod = skrsKod;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SistemKodlariSayfaGetirResponse", WrapperNamespace="http://ws.esaglik.surat.com.tr/", IsWrapped=true)]
    public partial class SistemKodlariSayfaGetirResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.esaglik.surat.com.tr/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public GPTS.skrs2.kodDegerleriResponse sistemKodlariOutput;
        
        public SistemKodlariSayfaGetirResponse() {
        }
        
        public SistemKodlariSayfaGetirResponse(GPTS.skrs2.kodDegerleriResponse sistemKodlariOutput) {
            this.sistemKodlariOutput = sistemKodlariOutput;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WSSKRSSistemlerChannel : GPTS.skrs2.WSSKRSSistemler, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WSSKRSSistemlerClient : System.ServiceModel.ClientBase<GPTS.skrs2.WSSKRSSistemler>, GPTS.skrs2.WSSKRSSistemler {
        
        public WSSKRSSistemlerClient() {
        }
        
        public WSSKRSSistemlerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WSSKRSSistemlerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WSSKRSSistemlerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WSSKRSSistemlerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GPTS.skrs2.SistemlerResponse GPTS.skrs2.WSSKRSSistemler.Sistemler(GPTS.skrs2.SistemlerRequest request) {
            return base.Channel.Sistemler(request);
        }
        
        public GPTS.skrs2.wsskrsSistemlerResponse Sistemler() {
            GPTS.skrs2.SistemlerRequest inValue = new GPTS.skrs2.SistemlerRequest();
            GPTS.skrs2.SistemlerResponse retVal = ((GPTS.skrs2.WSSKRSSistemler)(this)).Sistemler(inValue);
            return retVal.sistemlerOutput;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GPTS.skrs2.SistemKodlariResponse GPTS.skrs2.WSSKRSSistemler.SistemKodlari(GPTS.skrs2.SistemKodlariRequest request) {
            return base.Channel.SistemKodlari(request);
        }
        
        public GPTS.skrs2.kodDegerleriResponse SistemKodlari(string sistemKoduInput, System.DateTime tarihInput) {
            GPTS.skrs2.SistemKodlariRequest inValue = new GPTS.skrs2.SistemKodlariRequest();
            inValue.sistemKoduInput = sistemKoduInput;
            inValue.tarihInput = tarihInput;
            GPTS.skrs2.SistemKodlariResponse retVal = ((GPTS.skrs2.WSSKRSSistemler)(this)).SistemKodlari(inValue);
            return retVal.sistemKodlariOutput;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GPTS.skrs2.SistemKodlariSayfaGetirResponse GPTS.skrs2.WSSKRSSistemler.SistemKodlariSayfaGetir(GPTS.skrs2.SistemKodlariSayfaGetirRequest request) {
            return base.Channel.SistemKodlariSayfaGetir(request);
        }
        
        public GPTS.skrs2.kodDegerleriResponse SistemKodlariSayfaGetir(string sistemKoduInput, System.DateTime tarihInput, int skrsKod) {
            GPTS.skrs2.SistemKodlariSayfaGetirRequest inValue = new GPTS.skrs2.SistemKodlariSayfaGetirRequest();
            inValue.sistemKoduInput = sistemKoduInput;
            inValue.tarihInput = tarihInput;
            inValue.skrsKod = skrsKod;
            GPTS.skrs2.SistemKodlariSayfaGetirResponse retVal = ((GPTS.skrs2.WSSKRSSistemler)(this)).SistemKodlariSayfaGetir(inValue);
            return retVal.sistemKodlariOutput;
        }
    }
}
