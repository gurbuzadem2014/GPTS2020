using GPTS.EFaturaBasic;
using GPTS.Include.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GPTS.Entegrasyon
{
    public class FaturaGonder
    {
        private Satislar _Fatura;
        private SatisDetay[] _FaturaKalemleri;
        public InvoiceInfo CreateInvoice(Satislar Fatura, SatisDetay[] FaturaDetay)
        {
            DataTable dtSirket =  DB.GetData("select * from Sirketler with(nolock)");
            string sirketadi = dtSirket.Rows[0]["sirket"].ToString();
            string VergiDairesi = dtSirket.Rows[0]["VergiDairesi"].ToString();
            string VergiNo = dtSirket.Rows[0]["VergiNo"].ToString();
            string WebSitesi = dtSirket.Rows[0]["WebSitesi"].ToString();
            string gonderici_il="KOCAELİ";
            string gonderici_ilceAdi = "GEBZE";
            string gonderici_caddeSokak = "Köşklü Çeşme Mah. Yeni Bağdat Cad.";
            string gonderici_KapiNo = "160A";
            string gonderici_DaireNo = "1";

            _Fatura = Fatura;
            _FaturaKalemleri = FaturaDetay;
            //string svn= Degerler.SirketVkn;
            //string svd = Degerler.SirketVDaire;

            _Fatura.kalem_sayisi = _FaturaKalemleri.Length;

            var invoice = new InvoiceType
            {
                #region Genel Fatura Bilgileri
                ProfileID = new ProfileIDType { Value = _Fatura.FaturaTuru },//{ Value = _Fatura.FaturaTuru },
                CopyIndicator = new CopyIndicatorType { Value = false },//varsayılan şablon mu? hayır ney
                UUID = new UUIDType { Value = Guid.NewGuid().ToString() }, //Set edilmediğinde sistem tarafından otomatik verilir. 
                IssueDate = new IssueDateType { Value = _Fatura.FaturaTarihi },
                IssueTime = new IssueTimeType { Value = _Fatura.FaturaTarihi },
                InvoiceTypeCode = new InvoiceTypeCodeType { Value = _Fatura.FaturaTipi },
                Note = new NoteType[] { new NoteType { Value = _Fatura.Aciklama },
                    new NoteType { Value = _Fatura.Aciklama },
                    new NoteType { Value = "Bayi No: 112221" },
                    new NoteType { Value = "Fiş No=" + _Fatura.pkSatislar.ToString() } },
                DocumentCurrencyCode = new DocumentCurrencyCodeType { Value = "TRY" },
                PricingCurrencyCode = new PricingCurrencyCodeType { Value = "TRY" },
                LineCountNumeric = new LineCountNumericType { Value = 2 },
                //PaymentTerms = new PaymentTermsType { Note = new NoteType { Value = "30 gün vadeli" }, Amount = new AmountType1 { Value = 100, currencyID = "TRY" } },
                //PaymentMeans = new PaymentMeansType[] { new PaymentMeansType { PaymentDueDate = new PaymentDueDateType { Value = DateTime.Now.AddDays(15) }, PaymentMeansCode = new PaymentMeansCodeType1 { Value = "42" } } },
                //e-arşiv için mi
                //Delivery = new DeliveryType { DeliveryParty = new PartyType { };
                //PricingExchangeRate = new ExchangeRateType{ SourceCurrencyCode= "TRY",}
                #endregion

                #region SGK fatura alanları
                //AccountingCost = cmbInvoicetypeCode.Text == "SGK" ? new AccountingCostType { Value = cmbSgkInvoicetype.Text } : null,
                //InvoicePeriod = new PeriodType { StartDate = new StartDateType { Value = dpInvoicePeriodStart.Value }, EndDate = new EndDateType { Value = dpInvoicePeriodEnd.Value } },
                #endregion

                //Bayi İskontosu açıklama alanı gibi
                AllowanceCharge = new AllowanceChargeType[]
                {
                    new AllowanceChargeType {
                        ChargeIndicator = new ChargeIndicatorType { Value=true },
                        Amount = new AmountType2 { currencyID="TRY",Value=0 },
                        AllowanceChargeReason = new AllowanceChargeReasonType { Value= "Bayi İskontosu" },   }
                },

                //  BillingReference = new BillingReferenceType {   BillingReferenceLine = new BillingReferenceLineType[] { new BillingReferenceLineType {  } } }

                // AllowanceCharge = new AllowanceChargeType[] { new AllowanceChargeType { AllowanceChargeReason="Sigorta", ChargeIndicator = true },  }

                #region İrsaliye Bilgileri
                //Irsaliye dosyasi               
                //DespatchDocumentReference = new DocumentReferenceType[]
                //{ new DocumentReferenceType
                //{ IssueDate= new IssueDateType
                //{ Value=DateTime.Now},
                //    DocumentType = new DocumentTypeType
                //    {  Value = "Irsaliye" },
                //    ID = new IDType{Value="IRS000000001"}},
                //     new DocumentReferenceType{IssueDate= new IssueDateType{ Value=DateTime.Now},
                //         DocumentType = new DocumentTypeType{  Value = "Irsaliye" },
                //         ID = new IDType{Value="IRS000000002"}}},

                #endregion

                #region Xslt ve Ek belgeler
                //Fatura içerisinde görünüm dosyasını set etme. Değer geçilmediğinde varsayılan xslt kullanılır. 
                //dosyadan yükle
                //AdditionalDocumentReference = GetXsltAndDocumentsCustom("XSLTFile.xslt"),
                //gömülü yükle
                //AdditionalDocumentReference = GetXsltAndDocuments(),

                ////               AdditionalDocumentReference = new DocumentReferenceType { DocumentType= new DocumentTypeType{ Value="SATINALAMA BELGESİ"}, IssueDate=new IssueDateType{ Value= DateTime.Now},ID= new IDType{ Value="12345"}};
                //#endregion

                //#region Additional Document Reference
                //new DocumentReferenceType[]{
                //    new  DocumentReferenceType {
                //    ID = new IDType{ Value = new Guid().ToString()},
                //    IssueDate = new IssueDateType{ Value = DateTime.Now},
                //    Attachment= new AttachmentType{ 
                //                                    EmbeddedDocumentBinaryObject= new EmbeddedDocumentBinaryObjectType{ 
                //                                                                                                       filename="customxslt.xslt", 
                //                                                                                                        encodingCode= "Base64",
                //                                                                                                         mimeCode= BinaryObjectMimeCodeContentType.applicationxml,
                //                                                                                                        format="", 
                //                                                                                                        characterSetCode="UTF-8",
                //                                                                                                        Value = Encoding.UTF8.GetBytes(Properties.Resources.xslt) }}},


                // },
                #endregion

                #region Order Document Reference-sipariş ise
                //OrderReference = GetOrderReference(),
                #endregion

                #region Fatura Seri ve numarası
                ID = new IDType { Value = Fatura.FaturaNo }, //Set edilmediğinde sistem tarafından otomatik verilir. 
                #endregion

                #region Gönderici Bilgileri - AccountingSupplierParty              
                AccountingSupplierParty = new SupplierPartyType
                {

                    Party = new PartyType
                    {
                        PartyName = new PartyNameType { Name = new NameType1 { Value = sirketadi } },
                        PartyIdentification = new PartyIdentificationType[] { new PartyIdentificationType() { ID = new IDType { Value = VergiNo, schemeID = "VKN" } },
                            new PartyIdentificationType() {
                                ID = new IDType { Value = "12345669-111", schemeID = "MERSISNO" } }, new PartyIdentificationType() {
                                ID = new IDType { Value = "12345669-111", schemeID = "TICARETSICILNO" } } },

                        PostalAddress = new AddressType
                        {
                            CityName = new CityNameType { Value = gonderici_il },
                            StreetName = new StreetNameType { Value = gonderici_caddeSokak },
                            Country = new CountryType { Name = new NameType1 { Value = "TÜRKİYE" } },
                            Room = new RoomType { Value = gonderici_KapiNo },
                            BuildingNumber = new BuildingNumberType { Value = gonderici_DaireNo },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = gonderici_ilceAdi },
                        },
                        //PartyIdentification = new PartyIdentificationType[] { new PartyIdentificationType() { ID = new IDType { Value = "77777777701", schemeID = "TCKN" } } },
                        // Person = new PersonType{ FirstName= new FirstNameType{ Value="Ahmet"}, FamilyName= new FamilyNameType{ Value="Altınordu"} },
                        //PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = "Esenler" } } },
                        PartyTaxScheme = new PartyTaxSchemeType {
                            TaxScheme = new TaxSchemeType {
                                Name = new NameType1 { Value = VergiDairesi } }
                        },

                    }
                },
                #endregion

                //alıcı bilgileri
                AccountingCustomerParty = GetAccountingCustomerParty(),
                //ihracat  bilgileri
                //BuyerCustomerParty = GetBuyerCustomerParty(),
                //alıcının adresi gibi veya şirket adresi
                //TaxRepresentativeParty = GetTaxRepresantiveParty(),

                #region Fatura Satırları - InvoiceLines

                //Fatura Satırları
                InvoiceLine = GetInvoiceLines(),
                #endregion

                #region Vergi Alt Toplamları - TaxTotal

                //Fatura Genel KDV  Toplam Vergi
                TaxTotal = new TaxTotalType[]{
                                                    new  TaxTotalType{
                                                                         TaxSubtotal = new TaxSubtotalType[]
                                                                         {  new  TaxSubtotalType{
                                                                                                                            Percent = new PercentType1{
                                                                                                                                Value =Math.Round(Convert.ToDecimal(_FaturaKalemleri[0].KdvOrani),2)},
                                                                                                                                //Value =0,},
                                                                                                                             TaxCategory = new TaxCategoryType{
                                                                                                                               TaxScheme = new TaxSchemeType{
                                                                                                                             TaxTypeCode = new TaxTypeCodeType{  Value = "0015"},
                                                                                                                                    Name = new NameType1{ Value="KDV"} },
                                                                                                                     // TaxExemptionReason = new TaxExemptionReasonType { Value="11/1-a Mal ihracatı" },
                                                                                                                  //TaxExemptionReasonCode = new TaxExemptionReasonCodeType { Value= "301" }

                                                                                                                             },

                                                                                                                              TaxAmount = new TaxAmountType{ Value =Math.Round(Convert.ToDecimal(_Fatura.KdvToplamTutari),2), currencyID= "TRY" },


                                                                                                                            },



                                                                         },
                                                                         //TaxAmount: 0 değeri yazılacaktır.
                                                                         TaxAmount = new TaxAmountType{ Value =Math.Round(Convert.ToDecimal(_Fatura.KdvToplamTutari),2), currencyID= "TRY" },
                                                                         //TaxAmount = new TaxAmountType{ Value =0, currencyID= "TRY" },

                                                                        }

                    },

                #endregion
                
                #region Tevkifatlar

                    // WithholdingTaxTotal = new TaxTotalType[] { new TaxTotalType { TaxSubtotal,taxamo     } }

                    #endregion

                #region Yasal Alt Toplamlar - Legal Monetary Total

                    LegalMonetaryTotal = new MonetaryTotalType
                    {
                        //MAL HİZMET TOPLAM TUTAR
                        //LineExtensionAmount = new LineExtensionAmountType { Value = TotalLineExtentionAmount, currencyID = "TRY" },
                        //Satış Bedeli - TotalLineExtentionAmount = s1ToplamTutar + s2ToplamTutar;
                        LineExtensionAmount = new LineExtensionAmountType { Value = _Fatura.AraToplam - _Fatura.Toplamiskonto-_Fatura.KdvToplamTutari, currencyID = "TRY" },
                        //aratutar-aratoplam-iskonto
                        //TaxExclusiveAmount = new TaxExclusiveAmountType { Value = TotalTaxExculisiveAmount, currencyID = "TRY" },
                        TaxExclusiveAmount = new TaxExclusiveAmountType { Value = _Fatura.AraToplam - _Fatura.Toplamiskonto - _Fatura.KdvToplamTutari, currencyID = "TRY" },
                        //TaxInclusiveAmount = new TaxInclusiveAmountType { Value = TotalTaxInclusiveAmount, currencyID = "TRY" },
                        //aratoplam-iskonto+kdvtoplam
                        TaxInclusiveAmount = new TaxInclusiveAmountType { Value = _Fatura.AraToplam - _Fatura.Toplamiskonto, currencyID = "TRY" },
                        //AllowanceTotalAmount = new AllowanceTotalAmountType { Value = TotalAllowanceCharge, currencyID = "TRY" },
                        //iskonto toplam tutar mı
                        AllowanceTotalAmount = new AllowanceTotalAmountType { Value =_Fatura.Toplamiskonto, currencyID = "TRY" },
                        //PayableAmount = new PayableAmountType { Value = TotalTaxInclusiveAmount, currencyID = "TRY" },
                        //Ödenebilir miktar
                        PayableAmount = new PayableAmountType { Value = _Fatura.AraToplam-_Fatura.Toplamiskonto, currencyID = "TRY" },
                    }
                    #endregion
                    
            };

            #region e-Arşiv Fatura Bilgileri
            //Bu alanda eğer fatura bir e-arşiv faturası ise doldurulması gerkene alanlar doldurulmalıdır.
            EArchiveInvoiceInformation earchiveinfo = new EArchiveInvoiceInformation
            {
                //DeliveryType = rbtnEArchiveElectronic.Checked ? InvoiceDeliveryType.Electronic : InvoiceDeliveryType.Paper, //kağıt ortamda olduğunda Paper değeri set edilmelidir.

                //Eğer ilgili fatura bir internet satışına ait ise InternetSalesInfo nesnesinde gerekli değerler dolu olmalıdır. 
                InternetSalesInfo = new InternetSalesInformation
                {
                    PaymentDate = DateTime.Now, //Ödeme Tarihi
                    PaymentMidierName = "EFT/HAVALE",//txtEArchivePaymentMidierName.Text == "" ? null : txtEArchivePaymentMidierName.Text, //Ödeme Şekli
                    PaymentType ="EFT/HAVALE",// cmbEArchivePaymentType.Text == "" ? null : cmbEArchivePaymentType.Text == "DIGER - " ? cmbEArchivePaymentType.Text + txtEArchivePaymentDesc.Text : cmbEArchivePaymentType.Text, //Ödeme Şekli 

                    //Gönderi Bilgileri
                    ShipmentInfo = new ShipmentInformation
                    {
                        //Taşıyıcı Firma Bilgileri
                        Carier = new ShipmentCarier
                        {
                            SenderName = "ARAS", //txtEArchiveSenderTitle.Text == "" ? null : txtEArchiveSenderTitle.Text, //Taşıyıcı(Kargo) Şirketi Adı
                            SenderTcknVkn = "11111111111", //txtEArchiveSenderVKN.Text == "" ? null : txtEArchiveSenderVKN.Text, //Taşıyıcı(Kargo) Şirketi VKN
                        },
                        SendDate = DateTime.Now,//dtpEArchiveSendDate.Value == new DateTime(2500, 1, 1) ? DateTime.MinValue : dtpEArchiveSendDate.Value, //Gönderim-Kargo Tarihi
                    },

                    WebAddress = WebSitesi,//txtEArchiveWebAddress.Text == "" ? null : txtEArchiveWebAddress.Text, //Satışın yapıldığı internet sitesi adresi 

                },

            };
            #endregion


            return new InvoiceInfo
            {
                EArchiveInvoiceInfo = earchiveinfo,
                LocalDocumentId = _Fatura.pkSatislar.ToString(),
                Invoice = invoice,
                TargetCustomer = new CustomerInfo { VknTckn= "11111111111", Alias = "defaultpk" },//hedaf müşteri
                Scenario = InvoiceScenarioChoosen.Automated,//senaryo
                //ExtraInformation = txtExtraInformation.Text == "" ? null : txtExtraInformation.Text,

                //Notification = new NotificationInformation { 

                //    new MailingInformation { //Birden fazla bilgilendirme yapısı desteklenmiştir. Örneği muhasebeciye attachment olan diğer kişilere link olan mail gönderimi yapılmak istenirse yeni bir instance oluşturulup farklı gönderimler yapılabilir. 
                //    EnableNotification = true, //Mail gönderilecek mi bilgisi? 
                //    Attachment = new MailAttachmentInformation { Xml=true,Pdf=true }, //Mailde attachment olacaksa hangi tipte attachment olacak. 
                //    //EmailAccountIdentifier = "127ADE38-0BCB-4AC3-9830-B30A939AA8E9", //Bu Id canlı sistemde ayrıca sizinle paylaşılacaktır. Bir firmanın 1'den fazla mail sunucusu kullanılaiblir. Hangi sunucu ise o sunucu buradan belirtilecek
                //    To = "faruk.kaygisiz@uyumsoft.com.tr", //mail kime/kimlere gönderilecek
                //   // BodyXsltIdentifier = "C5A2BD86-4054-4387-9499-831AC6B108CA", // Bu Id canlı sistemde bizim tarafımızdan size sağlanacaktır. 
                //    Subject = "1234567689 abone numaranıza ait faturanız" // Mailin Subjecti ne olacak. 

                //    }
                //}

            };
        }
        
        public OrderReferenceType GetOrderReference()
        {
            OrderReferenceType orderref = new OrderReferenceType();
            orderref = new OrderReferenceType { ID = new IDType { Value = "ORD1234567" }, IssueDate = new IssueDateType { Value = DateTime.Now } };

            return orderref;
        }

        public DocumentReferenceType[] GetXsltAndDocuments()
        {
            DocumentReferenceType[] docs = new DocumentReferenceType[3];
            //if (chkXsltSet.Checked)
            //{


            docs[0] = new DocumentReferenceType
            {
                ID = new IDType { Value = new Guid().ToString() },
                IssueDate = new IssueDateType { Value = DateTime.Now },
                DocumentType = new DocumentTypeType { Value = "123456" },
                DocumentTypeCode = new DocumentTypeCodeType { Value = "MUKELLEF_KODU" },
                DocumentDescription = new DocumentDescriptionType[] { new DocumentDescriptionType { Value = "Kurum Adı" } },
            };

            docs[1] = new DocumentReferenceType
            {
                ID = new IDType { Value = new Guid().ToString() },
                IssueDate = new IssueDateType { Value = DateTime.Now },
                DocumentType = new DocumentTypeType { Value = "123456" },
                DocumentTypeCode = new DocumentTypeCodeType { Value = "MUKELLEF_ADI" },
                DocumentDescription = new DocumentDescriptionType[] { new DocumentDescriptionType { Value = "Kurum Kodu" } },
            };
            docs[2] = new DocumentReferenceType
            {
                ID = new IDType { Value = new Guid().ToString() },
                IssueDate = new IssueDateType { Value = DateTime.Now },
                DocumentType = new DocumentTypeType { Value = "123456" },
                DocumentTypeCode = new DocumentTypeCodeType { Value = "DOSYA_NO" },
                DocumentDescription = new DocumentDescriptionType[] { new DocumentDescriptionType { Value = "DOSYA NO" } },
            };

            docs[0] = new DocumentReferenceType
            {
                ID = new IDType { Value = new Guid().ToString() },
                IssueDate = new IssueDateType { Value = DateTime.Now },
                DocumentType = new DocumentTypeType { Value = "xslt" },
                Attachment = new AttachmentType
                {
                    EmbeddedDocumentBinaryObject = new EmbeddedDocumentBinaryObjectType
                    {
                        filename = "customxslt.xslt",
                        encodingCode = "Base64",
                        mimeCode = "applicationxml",
                        format = "",
                        characterSetCode = "UTF-8",
                        Value = Encoding.UTF8.GetBytes(Properties.Resources.XSLTFile)
                    }
                }
            };


            return docs;
            // };


            //if (chkSetInvoiceXslt.Checked)
            //{
            //    DocumentReferenceType doc = new DocumentReferenceType();
            //    //doc.ID = new IDType { Value = new Guid().ToString() };
            //   // doc.IssueDate = new IssueDateType { Value = DateTime.Now };
            //    AttachmentType atc = new AttachmentType { };
            //    EmbeddedDocumentBinaryObjectType emb = new EmbeddedDocumentBinaryObjectType();
            //    emb.filename = "customxslt.xslt";
            //    emb.encodingCode = "Base64";
            //    emb.mimeCode = "applicationxml";
            //    emb.format = "";
            //    emb.characterSetCode = "UTF-8";
            //    emb.Value = Encoding.UTF8.GetBytes(txtInvoiceXslt.Text);

            //    atc.EmbeddedDocumentBinaryObject = emb;
            //    doc.Attachment = atc;
            //    docs[0] = doc;

            //    return docs;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public DocumentReferenceType[] GetXsltAndDocumentsCustom(string xsltdosyasi)
        {
            DocumentReferenceType[] docs = new DocumentReferenceType[3];
            //if (chkXsltSet.Checked)
            //{


            //docs[0] = new DocumentReferenceType
            //{
            //    ID = new IDType { Value = new Guid().ToString() },
            //    IssueDate = new IssueDateType { Value = DateTime.Now },
            //    DocumentType = new DocumentTypeType { Value = "123456" },
            //    DocumentTypeCode = new DocumentTypeCodeType { Value = "MUKELLEF_KODU" },
            //    DocumentDescription = new DocumentDescriptionType[] { new DocumentDescriptionType { Value = "Kurum Adı" } },
            //};

            //docs[1] = new DocumentReferenceType
            //{
            //    ID = new IDType { Value = new Guid().ToString() },
            //    IssueDate = new IssueDateType { Value = DateTime.Now },
            //    DocumentType = new DocumentTypeType { Value = "123456" },
            //    DocumentTypeCode = new DocumentTypeCodeType { Value = "MUKELLEF_ADI" },
            //    DocumentDescription = new DocumentDescriptionType[] { new DocumentDescriptionType { Value = "Kurum Kodu" } },
            //};
            //docs[2] = new DocumentReferenceType
            //{
            //    ID = new IDType { Value = new Guid().ToString() },
            //    IssueDate = new IssueDateType { Value = DateTime.Now },
            //    DocumentType = new DocumentTypeType { Value = "123456" },
            //    DocumentTypeCode = new DocumentTypeCodeType { Value = "DOSYA_NO" },
            //    DocumentDescription = new DocumentDescriptionType[] { new DocumentDescriptionType { Value = "DOSYA NO" } },
            //};

            //docs[0] = new DocumentReferenceType
            //{
            //    ID = new IDType { Value = new Guid().ToString() },
            //    IssueDate = new IssueDateType { Value = DateTime.Now },
            //    DocumentType = new DocumentTypeType { Value = "xslt" },
            //    Attachment = new AttachmentType
            //    {
            //        EmbeddedDocumentBinaryObject = new EmbeddedDocumentBinaryObjectType
            //        {
            //            filename = "customxslt.xslt",
            //            encodingCode = "Base64",
            //            mimeCode = "applicationxml",
            //            format = "",
            //            characterSetCode = "UTF-8",
            //            Value = Encoding.UTF8.GetBytes(Properties.Resources.XSLTFile)
            //        }
            //    }
            //};


            //return docs;
            // };


            //if (chkSetInvoiceXslt.Checked)
            //{
                DocumentReferenceType doc = new DocumentReferenceType();
                //doc.ID = new IDType { Value = new Guid().ToString() };
                // doc.IssueDate = new IssueDateType { Value = DateTime.Now };
                AttachmentType atc = new AttachmentType { };
                EmbeddedDocumentBinaryObjectType emb = new EmbeddedDocumentBinaryObjectType();
                emb.filename = "customxslt.xslt";
                emb.encodingCode = "Base64";
                emb.mimeCode = "applicationxml";
                emb.format = "";
                emb.characterSetCode = "UTF-8";
                emb.Value = Encoding.UTF8.GetBytes(xsltdosyasi);//txtInvoiceXslt.Text);

                atc.EmbeddedDocumentBinaryObject = emb;
                doc.Attachment = atc;
                docs[0] = doc;

                return docs;
            //}
            //else
            //{
            //    return null;
            //}


        }

        public CustomerPartyType GetAccountingCustomerParty()
        {
            CustomerPartyType customer;

            PersonType person = new PersonType {
                FamilyName = new FamilyNameType { Value = _Fatura.AliciSoyad },
                FirstName = new FirstNameType { Value = _Fatura.AliciAdi }
            };

            if (_Fatura.FaturaTuru == "IHRACAT" || _Fatura.FaturaTuru == "YOLCUBERABERFATURA")
            {
                #region Gümrük Ticaret Bakanlığı Bilgileri - AccountingCustomerParty
                customer = new CustomerPartyType
                {

                    Party = new PartyType
                    {

                        PartyName = new PartyNameType { Name = new NameType1 { Value = "GÜMRÜK VE TİCARET BAKANLIĞI BİLGİ İŞLEM DAİRESİ BAŞKANLIĞI" } },
                        PartyIdentification = new PartyIdentificationType[1] { new PartyIdentificationType() { ID = new IDType { Value = "1460415308", schemeID = "VKN" } } },
                        PostalAddress = new AddressType
                        {
                            CityName = new CityNameType { Value = "Ankara" },
                            StreetName = new StreetNameType { Value = ">Üniversiteler Mahallesi Dumlupınar Bulvar" },
                            Country = new CountryType { Name = new NameType1 { Value = "Türkiye" } },

                            BuildingNumber = new BuildingNumberType { Value = "151" },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = "Çankaya" }

                        },

                        PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = "Ulus" } } },

                    }
                };

                #endregion

                return customer;
            }
            else
            {
                #region Alıcı Bilgileri - AccountingCustomerParty
                customer = new CustomerPartyType
                {

                    Party = new PartyType
                    {

                        //PartyName = new PartyNameType { Name = new NameType1 { Value = "hitit yazılım" } },//_Fatura.AliciAdi } },
                        PartyIdentification = new PartyIdentificationType[1] { new PartyIdentificationType() { ID = new IDType { Value = _Fatura.AliciVergiNo, schemeID = _Fatura.AliciVergiNo.Length == 10 ? "VKN" : "TCKN" } } },
                        PostalAddress = new AddressType
                        {
                            CityName = new CityNameType { Value = _Fatura.Aliciil },
                            StreetName = new StreetNameType { Value = _Fatura.AliciCaddeSokak },
                            Country = new CountryType { Name = new NameType1 { Value = "TÜRKİYE"} },
                            Room = new RoomType { Value = _Fatura.AliciKapiNo.ToString() },
                            BuildingNumber = new BuildingNumberType { Value = _Fatura.AliciKapiNo.ToString() },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = _Fatura.Aliciilce }

                            },
                        Contact = new ContactType {
                            Telefax = new TelefaxType { Value = _Fatura.Alici_Fax},
                            ElectronicMail = new ElectronicMailType { Value = _Fatura.Alici_ePosta },
                            Telephone = new TelephoneType { Value = _Fatura.Alici_Tel} },
                        WebsiteURI = new WebsiteURIType { Value = _Fatura.Alici_Web},
                        PartyTaxScheme = new PartyTaxSchemeType {
                            TaxScheme = new TaxSchemeType {
                                Name = new NameType1 { Value = _Fatura.AliciVergiDairesi } } },
                        Person = _Fatura.AliciVergiNo.Length == 11 ? person : null
                    }
                };

                #endregion

                return customer;
            }
        }

        public CustomerPartyType GetBuyerCustomerParty()
        {
            CustomerPartyType customer;
            #region İhracatçı Bilgileri - BuyerCustomerParty
            if (_Fatura.FaturaTuru == "IHRACAT")
            {
                customer = new CustomerPartyType
                {

                    Party = new PartyType
                    {

                        PartyName = new PartyNameType { Name = new NameType1 { Value = "txtAliciUnvan.Text" } },
                        PartyIdentification = new PartyIdentificationType[1] { new PartyIdentificationType() { ID = new IDType { Value = "EXPORT", schemeID = "PARTYTYPE" } } },
                        PostalAddress = new AddressType
                        {
                            CityName = new CityNameType { Value = "txtAliciIl.Text" },
                            StreetName = new StreetNameType { Value = "txtAliciCaddeSokak.Text" },
                            Country = new CountryType { Name = new NameType1 { Value = "txtAliciUlke.Text" } },
                            Room = new RoomType { Value = "txtAliciKapiNo.Text" },
                            BuildingNumber = new BuildingNumberType { Value = "txtAliciKapiNo.Text" },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = "txtGoncericiIlce.Text" }

                        },


                        PartyLegalEntity = new PartyLegalEntityType[] { new PartyLegalEntityType { RegistrationName = new RegistrationNameType { Value = "txtAliciUnvan.Text"}, CompanyID = new CompanyIDType { Value = "txtAliciVkn.Text" } } },
                        //Contact = new ContactType { Telefax = new TelefaxType { Value = "22111222" }, ElectronicMail = new ElectronicMailType { Value = "test@xyz.com" }, Telephone = new TelephoneType { Value = "0212200022" } },
                        //WebsiteURI = new WebsiteURIType { Value = "Web Sitesi" },

                        //PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = txtAliciVergiDairesi.Text } } },
                        //Person = new PersonType { FirstName = new FirstNameType { Value = "Ahmet" }, FamilyName = new FamilyNameType { Value = "Altınordu" } },
                    }
                };
                return customer;
            }
            #endregion
            #region Turist Bilgileri - BuyerCustomerParty
            //if (_Fatura.FaturaTuru == "YOLCUBERABERFATURA")
            //{



            //    customer = new CustomerPartyType
            //    {

            //        Party = new PartyType
            //        {

            //            Person = new PersonType
            //            {
            //                FirstName = new FirstNameType { Value = "adem" },
            //                FamilyName = new FamilyNameType { Value = "gürbüz" },
            //                NationalityID = new NationalityIDType { Value = "TR" },
            //                IdentityDocumentReference = new DocumentReferenceType { ID = new IDType { Value = "PSPTNO1234567" }, IssueDate = new IssueDateType { Value = new DateTime(2005, 1, 2) } }

            //            },
            //            PartyIdentification = new PartyIdentificationType[1] { new PartyIdentificationType() { ID = new IDType { Value = "TAXFREE", schemeID = "PARTYTYPE" } } },
            //            PostalAddress = new AddressType
            //            {
            //                CityName = new CityNameType { Value = _Fatura.Aliciil},
            //                StreetName = new StreetNameType { Value = _Fatura.AliciCaddeSokak },
            //                Country = new CountryType { Name = new NameType1 { Value = "Türkiye" } },
            //                Room = new RoomType { Value = _Fatura.AliciKapiNo.ToString() },
            //                BuildingNumber = new BuildingNumberType { Value = _Fatura.AliciKapiNo.ToString() },
            //                CitySubdivisionName = new CitySubdivisionNameType { Value = _Fatura.GoncericiIlce }

            //            },

            //            PartyLegalEntity = new PartyLegalEntityType[] { new PartyLegalEntityType { RegistrationName = new RegistrationNameType { Value = _Fatura.AliciSoyad }, CompanyID = new CompanyIDType { Value = _Fatura.AliciVergiNo } } },
            //            //Contact = new ContactType { Telefax = new TelefaxType { Value = "22111222" }, ElectronicMail = new ElectronicMailType { Value = "test@crssoft.com" }, Telephone = new TelephoneType { Value = "0212200022" } },
            //            //WebsiteURI = new WebsiteURIType { Value = "Web Sitesi" },

            //            //PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = txtAliciVergiDairesi.Text } } },
            //            //Person = new PersonType { FirstName = new FirstNameType { Value = "Ahmet" }, FamilyName = new FamilyNameType { Value = "Altınordu" } },
            //        }
            //    };
            //    return customer;
            //}
            #endregion

            else
            {
                return null;
            }
        }

        public PartyType GetTaxRepresantiveParty()
        {
            PartyType customer;

            if (_Fatura.FaturaTuru == "YOLCUBERABERFATURA")
            {
                #region Tax Free Aracı kurum Bilgileri - TaxRepresantiveParty
                customer = new PartyType
                {


                    PartyName = new PartyNameType { Name = new NameType1 { Value = "Hitit Yazılım kurum A.Ş." } },
                    PartyIdentification = new PartyIdentificationType[2] { new PartyIdentificationType() { ID = new IDType { Value = "1234567891", schemeID = "ARACIKURUMVKN" } }, new PartyIdentificationType() { ID = new IDType { Value = "urn:mail:yolcuberaberpk@aracikurum.com", schemeID = "ARACIKURUMETIKET" } } },
                    PostalAddress = new AddressType
                    {
                        CityName = new CityNameType { Value = "KOCAELİ" },
                        StreetName = new StreetNameType { Value = "Kazımkarabekir Mah. No:6 " },
                        Country = new CountryType { Name = new NameType1 { Value = "Türkiye" } },
                        CitySubdivisionName = new CitySubdivisionNameType { Value = "Darıca" }

                    },
                };
                return customer;
            }
            #endregion

            else
            {
                return null;
            }

        }

        public InvoiceLineType[] GetInvoiceLines()
        {
            InvoiceLineType[] invoiceLines = new InvoiceLineType[_Fatura.kalem_sayisi];
            for (int i = 0; i < _Fatura.kalem_sayisi; i++)
            {
                string birimi = _FaturaKalemleri[i].Birimi;
                if (birimi == "ADET") birimi = "NIU";//"C62";
                else if (birimi == "KG") birimi = "KGM";
                else if (birimi == "LİTRE") birimi = "LTR";
                else if (birimi == "METRE") birimi = "MTR";
                else birimi = "NIU";

                invoiceLines[i] = new InvoiceLineType
                {
                Item = new ItemType
                {
                  //ID  = new IDType { Value = "1"},
                  Name = new NameType1 { Value = _FaturaKalemleri[i].UrunAdi },
                  //marka
                  BrandName = new BrandNameType { Value = _FaturaKalemleri[i].Marka },
                  //txtAliciKodu1
                  BuyersItemIdentification = new ItemIdentificationType { ID = new IDType { Value = _FaturaKalemleri[i].pkSatisDetay.ToString() } },//alıcının kodu
                  ModelName = new ModelNameType { Value = _FaturaKalemleri[i].Model },
                  Description = new DescriptionType { Value = _FaturaKalemleri[i].Aciklama },
                  //txtUreticiKodu1
                  ManufacturersItemIdentification = new ItemIdentificationType { ID = new IDType { Value = _FaturaKalemleri[i].Barkod } },
                  //txtSaticiKodu1
                  SellersItemIdentification = new ItemIdentificationType { ID = new IDType { Value = _FaturaKalemleri[i].Barkod } },
                },
                    //İskonto
                    AllowanceCharge = new AllowanceChargeType[]
                    { new AllowanceChargeType
                    { Amount = new AmountType2 { Value = Convert.ToDecimal(_FaturaKalemleri[i].iskontoTutar), currencyID = "TRY" },
                      ChargeIndicator = new ChargeIndicatorType { Value = true },
                        PerUnitAmount = new PerUnitAmountType {
                           currencyID = "TRY", Value = Convert.ToDecimal(_FaturaKalemleri[i].iskontoOrani) }
                    } },
                ///Birim Fiyat
                Price = new PriceType {PriceAmount = new PriceAmountType { Value = Math.Round(Convert.ToDecimal(_FaturaKalemleri[i].Fiyat),2), currencyID = "TRY" },
                },
                //Miktar
                 InvoicedQuantity = new InvoicedQuantityType
                 {
                   unitCode = birimi,
                   Value = Convert.ToDecimal(_FaturaKalemleri[i].Miktar)
                 },
                //Not
                Note = new NoteType[] { new NoteType { Value = _FaturaKalemleri[i].Aciklama } },
                    // KDV ve Diğer Vergiler  KDV Oranı
            TaxTotal = new TaxTotalType
            {
                TaxSubtotal = new TaxSubtotalType[]{
                    //Vergi 1 KDV
                    new TaxSubtotalType{
                        //Verginin  üzerinden  hesaplandığı  tutar  (matrah)  bilgisi girilecektir
                        TaxableAmount= new TaxableAmountType {Value=Convert.ToDecimal(_FaturaKalemleri[i].ToplamTutar), currencyID= "TRY" },
                      //TaxAmount = new TaxAmountType{ Value = ((100+Convert.ToDecimal(_FaturaKalemleri[i].KdvOrani))/100) * Convert.ToDecimal(_FaturaKalemleri[i].Fiyat) * (Convert.ToDecimal(_FaturaKalemleri[i].Miktar)), currencyID= "TRY" },
                      //Hesaplanan vergi tutarıdır.
                        TaxAmount = new TaxAmountType{ Value = Convert.ToDecimal(_FaturaKalemleri[i].KdvTutar), currencyID= "TRY" },
                        Percent = new PercentType1 {Value =Convert.ToDecimal(_FaturaKalemleri[i].KdvOrani)},
                           TaxCategory = new TaxCategoryType{TaxScheme = new TaxSchemeType
                           {
                           TaxTypeCode = new TaxTypeCodeType{Value = "0015"},
                           Name =new NameType1{ Value="KDV"}
                           },
                           //501 değeri yazılacaktır. 
                           //TaxExemptionReason =new TaxExemptionReasonType{ Value="12345 sayılı kanuna istinaden" }
                        },
                          //TaxExemptionReason=new TaxExemptionReasonType{ Value="Promosyon Ürün"},
                        //PerUnitAmount = new PerUnitAmountType{Value = ((100+Convert.ToDecimal(_FaturaKalemleri[i].KdvOrani))/100)* Convert.ToDecimal(_FaturaKalemleri[i].Fiyat), currencyID= "TRY"},
                        }},
                TaxAmount = new TaxAmountType { Value = Math.Round(Convert.ToDecimal(_FaturaKalemleri[i].KdvOrani), 2), currencyID = "TRY" },
            },
            //satır toplamı 
            ID = new IDType { Value = (i+1).ToString() },
            LineExtensionAmount = new LineExtensionAmountType { Value = Math.Round(Convert.ToDecimal(_FaturaKalemleri[i].ToplamTutar- _FaturaKalemleri[i].KdvTutar),2), currencyID = "TRY" },
                    //Delivery 
                    //Shipment

                    //Sipariş Hattı Referansı
                    // OrderLineReference = new OrderLineReferenceType[] { new OrderLineReferenceType { OrderReference = new OrderReferenceType { ID = new IDType { Value = "a" } } } }

                };
            }
            return invoiceLines;
        }
    }
}
