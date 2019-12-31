using GPTS.Uyumsoft;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GPTS
{
    public partial class frmUyumSoftEfatura : Form
    {
        List<InvoiceType> _Invoices;
        decimal s1ToplamTutar = 0;
        decimal s2ToplamTutar = 0;

        decimal s1KDVTutar = 0;
        decimal s2KDVTutar = 0;

        decimal s1IskontoTutar = 0;
        decimal s2IskontoTutar = 0;

        decimal TotalLineExtentionAmount = 0;

        decimal TotalAllowanceCharge = 0;

        decimal TotalTaxExculisiveAmount = 0;
        decimal TotalTaxInclusiveAmount = 0;

        public frmUyumSoftEfatura()
        {
            InitializeComponent();
        }

        private void btnSendInvoice_Click_1(object sender, EventArgs e)
        {
            if (rbtnLiveSystem.Checked)
            {
                DialogResult dialogresult = MessageBox.Show("Canlı sistemde fatura göndermek istediğinizden emin misiniz?", "", MessageBoxButtons.YesNo);
                if (dialogresult == DialogResult.No)
                    return;
            }
            //hesapla
            button3_Click(sender, e);

            var client = CreateClient();

            var invoiceInfo = CreateInvoice();

            InvoiceInfo[] invoices = new InvoiceInfo[1];
            invoices[0] = invoiceInfo;

            var response = client.SendInvoice(invoices);
            //InvoiceIdentitiesResponse response = client.SendInvoice(invoices);

            if (response.IsSucceded)
            {

                MessageBox.Show(
                    string.Format("Fatura Gönderildi\n UUID:{0} \n ID:{1} \n Fatura Tipi:{2} ",
                            response.Value[0].Id.ToString(),
                            response.Value[0].Number.ToString(),
                            response.Value[0].InvoiceScenario.ToString()
                            )
                            );
                txtSampleOutboxGuid.Text = response.Value[0].Id.ToString();
                txtSampleGuid.Text = response.Value[0].Id.ToString();
                // Clipboard.SetText(response.Value[0].Id.ToString());
            }
            else
            {
                MessageBox.Show(response.Message);
            }
        }

        public IntegrationClient CreateClient()
        {
            var username = "";
            var password = "";
            var serviceuri = "";
            if (rbtnTestSystem.Checked)
            {
                username = txtWebServiceTestUsername.Text;
                password = txtWebServiceTestPassword.Text;
                serviceuri = txtWebServiceTestUrl.Text;
            }
            if (rbtnLiveSystem.Checked)
            {
                username = txtWebServiceLiveUsername.Text;
                password = txtWebServiceLivePassword.Text;
                serviceuri = txtWebServiceLiveUrl.Text;
            }

            var client = new IntegrationClient();
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceuri);
            //  var client = new IntegrationClient();
            client.ClientCredentials.UserName.UserName = username;
            client.ClientCredentials.UserName.Password = password;
            //var response = client.IsEInvoiceUser("9000068418",string.Empty);
            return client;
        }

        public InvoiceInfo CreateInvoice()
        {
            var invoice = new InvoiceType
            {
                #region Genel Fatura Bilgileri
                ProfileID = new ProfileIDType { Value = cmbFaturaTuru.Text },
                CopyIndicator = new CopyIndicatorType { Value = false },
                UUID = new UUIDType { Value = Guid.NewGuid().ToString() }, //Set edilmediğinde sistem tarafından otomatik verilir. 
                IssueDate = new IssueDateType { Value = dpFaturaTarihi.Value },
                IssueTime = new IssueTimeType { Value = dpFaturaTarihi.Value },
                InvoiceTypeCode = new InvoiceTypeCodeType { Value = cmbInvoicetypeCode.Text },
                Note = new NoteType[] { new NoteType { Value = txtFaturaNotu.Text }, new NoteType { Value = txtFaturaNotu.Text }, new NoteType { Value = "Bayi No: 112221" }, new NoteType { Value = "Test Not alanı 3" } },
                DocumentCurrencyCode = new DocumentCurrencyCodeType { Value = "TRY" },
                PricingCurrencyCode = new PricingCurrencyCodeType { Value = "TRY" },
                LineCountNumeric = new LineCountNumericType { Value = 2 },
                //PaymentTerms = new PaymentTermsType { Note = new NoteType { Value = "30 gün vadeli" }, Amount = new AmountType1 { Value = 100, currencyID = "TRY" } },
                //PaymentMeans = new PaymentMeansType[] { new PaymentMeansType { PaymentDueDate = new PaymentDueDateType { Value = DateTime.Now.AddDays(15) }, PaymentMeansCode = new PaymentMeansCodeType1 { Value = "42" } } },
                //Delivery = new DeliveryType { DeliveryParty = new PartyType { };
                // PricingExchangeRate = new ExchangeRateType{ SourceCurrencyCode= "TRY",}
                #endregion

                #region SGK fatura alanları
                AccountingCost = cmbInvoicetypeCode.Text == "SGK" ? new AccountingCostType { Value = cmbSgkInvoicetype.Text } : null,
                InvoicePeriod = new PeriodType { StartDate = new StartDateType { Value = dpInvoicePeriodStart.Value }, EndDate = new EndDateType { Value = dpInvoicePeriodEnd.Value } },
                #endregion



                AllowanceCharge = new AllowanceChargeType[]
                {
                    new AllowanceChargeType { ChargeIndicator= new ChargeIndicatorType { Value=true }, Amount = new AmountType2 { currencyID="TRY",Value=100 }, AllowanceChargeReason = new AllowanceChargeReasonType { Value= "Bayi İskontosu" },   }
                },

                //  BillingReference = new BillingReferenceType {   BillingReferenceLine = new BillingReferenceLineType[] { new BillingReferenceLineType {  } } }

                // AllowanceCharge = new AllowanceChargeType[] { new AllowanceChargeType { AllowanceChargeReason="Sigorta", ChargeIndicator = true },  }

                #region İrsaliye Bilgileri
                //Irsaliye dosyasi               
                DespatchDocumentReference = new DocumentReferenceType[]{ new DocumentReferenceType{IssueDate= new IssueDateType{ Value=DateTime.Now},  DocumentType= new DocumentTypeType{  Value = "Irsaliye" }, ID= new IDType{Value="IRS000000001"}},
                                                                         new DocumentReferenceType{IssueDate= new IssueDateType{ Value=DateTime.Now},  DocumentType= new DocumentTypeType{  Value = "Irsaliye" }, ID= new IDType{Value="IRS000000002"}}},

                #endregion

                #region Xslt ve Ek belgeler
                //Fatura içerisinde görünüm dosyasını set etme. Değer geçilmediğinde varsayılan xslt kullanılır. 
                AdditionalDocumentReference = GetXsltAndDocuments(),

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

                #region Order Document Reference
                OrderReference = GetOrderReference(),
                #endregion

                #region Fatura Seri ve numarası
                ID = new IDType { Value = txtFaturaNumarasi.Text }, //Set edilmediğinde sistem tarafından otomatik verilir. 
                #endregion




                #region Gönderici Bilgileri - AccountingSupplierParty

                AccountingSupplierParty = new SupplierPartyType
                {

                    Party = new PartyType
                    {
                        PartyName = new PartyNameType { Name = new NameType1 { Value = txtGondericiUnvan.Text } },
                        PartyIdentification = new PartyIdentificationType[] { new PartyIdentificationType() { ID = new IDType { Value = txtGondericiVkn.Text, schemeID = "VKN" } }, new PartyIdentificationType() { ID = new IDType { Value = "12345669-111", schemeID = "MERSISNO" } }, new PartyIdentificationType() { ID = new IDType { Value = "12345669-111", schemeID = "TICARETSICILNO" } } },

                        PostalAddress = new AddressType
                        {
                            CityName = new CityNameType { Value = txtGondericiIl.Text },
                            StreetName = new StreetNameType { Value = txtGondericiCaddeSokak.Text },
                            Country = new CountryType { Name = new NameType1 { Value = txtGondericiUlke.Text } },
                            Room = new RoomType { Value = txtGondericiKapiNo.Text },
                            BuildingNumber = new BuildingNumberType { Value = txtGondericiKapiNo.Text },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = txtGoncericiIlce.Text },



                        },
                        //PartyIdentification = new PartyIdentificationType[] { new PartyIdentificationType() { ID = new IDType { Value = "77777777701", schemeID = "TCKN" } } },
                        // Person = new PersonType{ FirstName= new FirstNameType{ Value="Ahmet"}, FamilyName= new FamilyNameType{ Value="Altınordu"} },
                        //PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = "Esenler" } } },
                        PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = txtGondericiVergiDairesi.Text } } },

                    }
                },
                #endregion

                AccountingCustomerParty = GetAccountingCustomerParty(),
                BuyerCustomerParty = GetBuyerCustomerParty(),
                TaxRepresentativeParty = GetTaxRepresantiveParty(),

                #region Fatura Satırları - InvoiceLines
                //Fatura Satırları
                InvoiceLine = //GetInvoiceLines(),
                new InvoiceLineType[1]
                {
                    //Fatura Satır 1
                    new InvoiceLineType
                    {   
                        //Ürün Açıklamaları
                        Item = new ItemType
                             {
                                  Name = new NameType1{ Value = txtUrunAdi1.Text},
                                  BrandName = new BrandNameType{  Value = txtMarka1.Text },
                                  BuyersItemIdentification  = new ItemIdentificationType { ID = new IDType{ Value = txtAliciKodu1.Text}},
                                  ModelName  = new ModelNameType{ Value = txtModel1.Text},
                                  Description = new DescriptionType{ Value = txtAciklama1.Text},
                                  ManufacturersItemIdentification = new ItemIdentificationType { ID = new IDType{ Value =txtUreticiKodu1.Text}},
                                  SellersItemIdentification =  new ItemIdentificationType{ ID = new IDType{Value = txtSaticiKodu1.Text}},


                             },

                        //İskonto
                         //AllowanceCharge = new AllowanceChargeType{ 
                         //                                              ChargeIndicator= new ChargeIndicatorType{  Value=true},MultiplierFactorNumeric= new MultiplierFactorNumericType{ Value=Math.Round(Convert.ToDecimal(txtIskontoOrani1.Text),2)/100},Amount= new AmountType2 { Value = Math.Round(Convert.ToDecimal(txtIskontoOrani1.Text),2), currencyID= "TRY"},
                         //                                           },
                         AllowanceCharge = new AllowanceChargeType[] { new AllowanceChargeType { Amount = new AmountType2 { Value = 100, currencyID = "TRY" }, ChargeIndicator= new ChargeIndicatorType { Value= false }, PerUnitAmount =   new PerUnitAmountType { currencyID="TRY", Value=100 } } },
                        //Birim Fiyat
                         Price = new PriceType { PriceAmount = new PriceAmountType{ Value= Convert.ToDecimal(txtFiyat1.Text), currencyID = "TRY" }},
                         

                         //Miktar
                         InvoicedQuantity = new InvoicedQuantityType{ unitCode = "NIU" , Value=Math.Round(Convert.ToDecimal(txtMiktar1.Text),2)}, //NIU =Adet
                         
                         
                         //Not
                         Note = new NoteType[]{ new NoteType { Value = txtSatirNotu1.Text } },

                         // KDV ve Diğer Vergiler
                          TaxTotal =  new TaxTotalType{  TaxSubtotal = new TaxSubtotalType[]{ 
                                                                                                //Vergi 1 KDV
                                                                                                new TaxSubtotalType{
                                                                                                    Percent = new PercentType1 { Value=Convert.ToDecimal(txtKdvOrani1.Text) }  ,             
                                                                                                    //Percent =   //new PercentType{ Value=Math.Round(Convert.ToDecimal(txtKdvOrani1.Text),2)},
                                                                                                                    TaxCategory = new TaxCategoryType{TaxScheme = new TaxSchemeType{ TaxTypeCode = new TaxTypeCodeType{  Value = "0015"}, Name =new NameType1{ Value="KDV"} }, TaxExemptionReason=new TaxExemptionReasonType{ Value="12345 sayılı kanuna istinaden" }},
                                                                                                                    TaxAmount = new TaxAmountType{ Value = Math.Round(Convert.ToDecimal(txtKdvTutar1.Text),2), currencyID= "TRY" },


                                                                                                                        }
                                                                                                },
                                                         TaxAmount=new TaxAmountType{ Value= Math.Round(Convert.ToDecimal(txtKdvTutar1.Text),2), currencyID="TRY"}

                           },
                         ID = new IDType {Value = "1"},
                          LineExtensionAmount = new LineExtensionAmountType{ Value=Math.Round(Convert.ToDecimal(txtToplamTutar1.Text),2) , currencyID ="TRY"},
                          Delivery = new DeliveryType[] {
                              new DeliveryType {

                                  DeliveryTerms = new DeliveryTermsType[] {
                                      new DeliveryTermsType {
                                          ID = new IDType {
                                              schemeID = "INCOTERMS",
                                              Value = "CIF"
                                          }

                                      }
                                  },
                                  Shipment = new ShipmentType {


                                      ID = new IDType { Value="1" },
                                      TransportHandlingUnit = new TransportHandlingUnitType[] { new TransportHandlingUnitType {

                                                 ActualPackage = new PackageType[] {
                                                     new PackageType { PackagingTypeCode= new PackagingTypeCodeType { Value="CK" },
                                                         ID = new IDType { Value="KapNo12345" },
                                                         Quantity = new QuantityType2 { Value= 3, unitCode= "CK" },
                                                         ContainedPackage = new PackageType[] { new PackageType {  }  }
                                                      } }

                                       }
                                       },
                                      ShipmentStage = new ShipmentStageType[] { new ShipmentStageType { TransportModeCode = new TransportModeCodeType { Value = "1" },  } },

                                       GoodsItem = new GoodsItemType[] { new GoodsItemType { DeclaredCustomsValueAmount = new DeclaredCustomsValueAmountType { Value=15, currencyID="USD" },
                                           FreeOnBoardValueAmount = new FreeOnBoardValueAmountType {  Value= 12, currencyID="USD" }  ,  RequiredCustomsID=  new RequiredCustomsIDType { Value= "123556.AA" }    } }
                                     },
                                       DeliveryAddress = new AddressType {
                                       CityName = new CityNameType { Value= "Berlin" },
                                       BuildingName = new BuildingNameType {Value= "C1" },
                                       BuildingNumber = new BuildingNumberType { Value="155"},
                                       Country = new CountryType {  Name= new NameType1 {Value= "GERMANY" } },
                                       StreetName =new StreetNameType {Value= "Oberlandstraße 40-41" },
                                       CitySubdivisionName= new CitySubdivisionNameType { Value= "Oberland" },

                                   },



                              }
                          },
                            // OrderLineReference = new OrderLineReferenceType[] { new OrderLineReferenceType { OrderReference = new OrderReferenceType { ID = new IDType { Value = "a" } } } }






                    },
                },
                #endregion

                #region Vergi Alt Toplamları - TaxTotal

                //Fatura Genel KDV 
                TaxTotal = new TaxTotalType[]{
                                                new  TaxTotalType{
                                                                     TaxSubtotal = new TaxSubtotalType[]{  new  TaxSubtotalType{
                                                                                                                        Percent = new PercentType1{ Value=Math.Round(Convert.ToDecimal(txtFaturaKdvOran1.Text),2)},
                                                                                                                         TaxCategory = new TaxCategoryType{
                                                                                                                             TaxScheme = new TaxSchemeType{
                                                                                                                                 TaxTypeCode = new TaxTypeCodeType{  Value = "0015"},
                                                                                                                                 Name =new NameType1{ Value="KDV"} },
                                                                                                                                TaxExemptionReason = new TaxExemptionReasonType { Value="11/1-a Mal ihracatı" },
                                                                                                                                 TaxExemptionReasonCode= new TaxExemptionReasonCodeType { Value= "301" }

                                                                                                                         },

                                                                                                                          TaxAmount = new TaxAmountType{ Value =Math.Round(Convert.ToDecimal(txtFaturaKdvOran1.Text),2), currencyID= "TRY" },


                                                                                                                        },



                                                                                                            },

                                                                             TaxAmount = new TaxAmountType{ Value =Math.Round(Convert.ToDecimal(txtFaturaKdvOran1.Text),2), currencyID= "TRY" },

                                                                    }

                },

                #endregion

                #region Tevkifatlar

                // WithholdingTaxTotal = new TaxTotalType[] { new TaxTotalType { TaxSubtotal,taxamo     } }

                #endregion

                #region Yasal Alt Toplamlar - Legal Monetary Total

                LegalMonetaryTotal = new MonetaryTotalType
                {
                    LineExtensionAmount = new LineExtensionAmountType { Value = TotalLineExtentionAmount, currencyID = "TRY" },
                    TaxExclusiveAmount = new TaxExclusiveAmountType { Value = TotalTaxExculisiveAmount, currencyID = "TRY" },
                    TaxInclusiveAmount = new TaxInclusiveAmountType { Value = TotalTaxInclusiveAmount, currencyID = "TRY" },
                    AllowanceTotalAmount = new AllowanceTotalAmountType { Value = TotalAllowanceCharge, currencyID = "TRY" },
                    //-+    ChargeTotalAmount = new ChargeTotalAmountType { Value = Convert.ToDecimal(txtIskontoTutar1.Text) + Convert.ToDecimal(txtIskontoTutar2.Text), currencyID = "TRY" },
                    PayableAmount = new PayableAmountType { Value = TotalTaxInclusiveAmount, currencyID = "TRY" },
                    // PayableRoundingAmount = new PayableRoundingAmountType { Value = Convert.ToDecimal(txtToplamTutar1.Text) + Convert.ToDecimal(txtToplamTutar2.Text), currencyID = "TRY" }

                }
                #endregion

            };

            #region e-Arşiv Fatura Bilgileri
            //Bu alanda eğer fatura bir e-arşiv faturası ise doldurulması gerkene alanlar doldurulmalıdır.
            EArchiveInvoiceInformation earchiveinfo = new EArchiveInvoiceInformation
            {
                DeliveryType = rbtnEArchiveElectronic.Checked ? InvoiceDeliveryType.Electronic : InvoiceDeliveryType.Paper, //kağıt ortamda olduğunda Paper değeri set edilmelidir.

                //Eğer ilgili fatura bir internet satışına ait ise InternetSalesInfo nesnesinde gerekli değerler dolu olmalıdır. 
                InternetSalesInfo = new InternetSalesInformation
                {
                    PaymentDate = DateTime.Now, //Ödeme Tarihi
                    PaymentMidierName = txtEArchivePaymentMidierName.Text == "" ? null : txtEArchivePaymentMidierName.Text, //Ödeme Şekli
                    PaymentType = cmbEArchivePaymentType.Text == "" ? null : cmbEArchivePaymentType.Text == "DIGER - " ? cmbEArchivePaymentType.Text + txtEArchivePaymentDesc.Text : cmbEArchivePaymentType.Text, //Ödeme Şekli 

                    //Gönderi Bilgileri
                    ShipmentInfo = new ShipmentInformation
                    {
                        //Taşıyıcı Firma Bilgileri
                        Carier = new ShipmentCarier
                        {
                            SenderName = txtEArchiveSenderTitle.Text == "" ? null : txtEArchiveSenderTitle.Text, //Taşıyıcı(Kargo) Şirketi Adı
                            SenderTcknVkn = txtEArchiveSenderVKN.Text == "" ? null : txtEArchiveSenderVKN.Text, //Taşıyıcı(Kargo) Şirketi VKN
                        },
                        SendDate = DateTime.Now,//dtpEArchiveSendDate.Value == new DateTime(2500, 1, 1) ? DateTime.MinValue : dtpEArchiveSendDate.Value, //Gönderim-Kargo Tarihi
                    },

                    WebAddress = txtEArchiveWebAddress.Text == "" ? null : txtEArchiveWebAddress.Text, //Satışın yapıldığı internet sitesi adresi 

                },

            };


            #endregion



            return new InvoiceInfo
            {
                EArchiveInvoiceInfo = earchiveinfo,
                LocalDocumentId = txtLocalDocumentId.Text,
                Invoice = invoice,
                TargetCustomer = new CustomerInfo { Alias = txtAliciAlias.Text == "" ? "" : txtAliciAlias.Text },
                Scenario = InvoiceScenarioChoosen.Automated,
                ExtraInformation = txtExtraInformation.Text == "" ? null : txtExtraInformation.Text,

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

        public CustomerPartyType GetAccountingCustomerParty()
        {
            CustomerPartyType customer;

            PersonType person = new PersonType { FamilyName = new FamilyNameType { Value = txtAliciSoyad.Text }, FirstName = new FirstNameType { Value = txtAliciUnvan.Text } };

            if (cmbFaturaTuru.Text == "IHRACAT" || cmbFaturaTuru.Text == "YOLCUBERABERFATURA")
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

                        PartyName = new PartyNameType { Name = new NameType1 { Value = txtAliciUnvan.Text } },
                        PartyIdentification = new PartyIdentificationType[1] { new PartyIdentificationType() { ID = new IDType { Value = txtAliciVkn.Text, schemeID = txtAliciVkn.Text.Length == 10 ? "VKN" : "TCKN" } } },
                        PostalAddress = new AddressType
                        {
                            CityName = new CityNameType { Value = txtAliciIl.Text },
                            StreetName = new StreetNameType { Value = txtAliciCaddeSokak.Text },
                            Country = new CountryType { Name = new NameType1 { Value = txtAliciUlke.Text } },
                            Room = new RoomType { Value = txtAliciKapiNo.Text },
                            BuildingNumber = new BuildingNumberType { Value = txtAliciKapiNo.Text },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = txtGoncericiIlce.Text }

                        },
                        Contact = new ContactType { Telefax = new TelefaxType { Value = "22111222" }, ElectronicMail = new ElectronicMailType { Value = "test@test.com" }, Telephone = new TelephoneType { Value = "0212200022" } },
                        WebsiteURI = new WebsiteURIType { Value = "Web Sitesi" },
                        PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = txtAliciVergiDairesi.Text } } },
                        Person = txtAliciVkn.Text.Length == 11 ? person : null
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
            if (cmbFaturaTuru.Text == "IHRACAT")
            {



                customer = new CustomerPartyType
                {

                    Party = new PartyType
                    {

                        PartyName = new PartyNameType { Name = new NameType1 { Value = txtAliciUnvan.Text } },
                        PartyIdentification = new PartyIdentificationType[1] { new PartyIdentificationType() { ID = new IDType { Value = "EXPORT", schemeID = "PARTYTYPE" } } },
                        PostalAddress = new AddressType
                        {
                            CityName = new CityNameType { Value = txtAliciIl.Text },
                            StreetName = new StreetNameType { Value = txtAliciCaddeSokak.Text },
                            Country = new CountryType { Name = new NameType1 { Value = txtAliciUlke.Text } },
                            Room = new RoomType { Value = txtAliciKapiNo.Text },
                            BuildingNumber = new BuildingNumberType { Value = txtAliciKapiNo.Text },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = txtGoncericiIlce.Text }

                        },


                        PartyLegalEntity = new PartyLegalEntityType[] { new PartyLegalEntityType { RegistrationName = new RegistrationNameType { Value = txtAliciUnvan.Text }, CompanyID = new CompanyIDType { Value = txtAliciVkn.Text } } },
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
            if (cmbFaturaTuru.Text == "YOLCUBERABERFATURA")
            {



                customer = new CustomerPartyType
                {

                    Party = new PartyType
                    {

                        Person = new PersonType
                        {
                            FirstName = new FirstNameType { Value = "JOHN" },
                            FamilyName = new FamilyNameType { Value = "DOE" },
                            NationalityID = new NationalityIDType { Value = "TR" },
                            IdentityDocumentReference = new DocumentReferenceType { ID = new IDType { Value = "PSPTNO1234567" }, IssueDate = new IssueDateType { Value = new DateTime(2005, 1, 2) } }

                        },
                        PartyIdentification = new PartyIdentificationType[1] { new PartyIdentificationType() { ID = new IDType { Value = "TAXFREE", schemeID = "PARTYTYPE" } } },
                        PostalAddress = new AddressType
                        {
                            CityName = new CityNameType { Value = txtAliciIl.Text },
                            StreetName = new StreetNameType { Value = txtAliciCaddeSokak.Text },
                            Country = new CountryType { Name = new NameType1 { Value = txtAliciUlke.Text } },
                            Room = new RoomType { Value = txtAliciKapiNo.Text },
                            BuildingNumber = new BuildingNumberType { Value = txtAliciKapiNo.Text },
                            CitySubdivisionName = new CitySubdivisionNameType { Value = txtGoncericiIlce.Text }

                        },

                        PartyLegalEntity = new PartyLegalEntityType[] { new PartyLegalEntityType { RegistrationName = new RegistrationNameType { Value = txtAliciUnvan.Text }, CompanyID = new CompanyIDType { Value = txtAliciVkn.Text } } },
                        //Contact = new ContactType { Telefax = new TelefaxType { Value = "22111222" }, ElectronicMail = new ElectronicMailType { Value = "test@crssoft.com" }, Telephone = new TelephoneType { Value = "0212200022" } },
                        //WebsiteURI = new WebsiteURIType { Value = "Web Sitesi" },

                        //PartyTaxScheme = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = txtAliciVergiDairesi.Text } } },
                        //Person = new PersonType { FirstName = new FirstNameType { Value = "Ahmet" }, FamilyName = new FamilyNameType { Value = "Altınordu" } },
                    }
                };
                return customer;
            }
            #endregion

            else
            {
                return null;
            }

        }

        public PartyType GetTaxRepresantiveParty()
        {
            PartyType customer;

            if (cmbFaturaTuru.Text == "YOLCUBERABERFATURA")
            {


                #region Tax Free Aracı kurum Bilgileri - TaxRepresantiveParty
                customer = new PartyType
                {


                    PartyName = new PartyNameType { Name = new NameType1 { Value = "Tax Free Aracı kurum A.Ş." } },
                    PartyIdentification = new PartyIdentificationType[2] { new PartyIdentificationType() { ID = new IDType { Value = "1234567891", schemeID = "ARACIKURUMVKN" } }, new PartyIdentificationType() { ID = new IDType { Value = "urn:mail:yolcuberaberpk@aracikurum.com", schemeID = "ARACIKURUMETIKET" } } },
                    PostalAddress = new AddressType
                    {
                        CityName = new CityNameType { Value = "İstanbul" },
                        StreetName = new StreetNameType { Value = "Levent Mah. No:1 " },
                        Country = new CountryType { Name = new NameType1 { Value = "Türkiye" } },
                        CitySubdivisionName = new CitySubdivisionNameType { Value = "Şişli" }

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

        private void button3_Click(object sender, EventArgs e)
        {
            txtKdvTutar1.Text = (Math.Round(Convert.ToDecimal(txtMiktar1.Text), 2) * Math.Round(Convert.ToDecimal(txtFiyat1.Text), 2) * ((100 - Math.Round(Convert.ToDecimal(txtIskontoOrani1.Text), 2)) / 100) * (Math.Round(Convert.ToDecimal(txtKdvOrani1.Text), 2) / 100)).ToString("0.00");
            txtIskontoTutar1.Text = (Math.Round(Convert.ToDecimal(txtMiktar1.Text), 2) * Math.Round(Convert.ToDecimal(txtFiyat1.Text), 2) * (Math.Round(Convert.ToDecimal(txtIskontoOrani1.Text), 2) / 100)).ToString("0.00");
            //txtToplamTutar2.Text = (Math.Round(Convert.ToDecimal(txtMiktar2.Text), 2) * Math.Round(Convert.ToDecimal(txtFiyat2.Text), 2) * ((100 - Math.Round(Convert.ToDecimal(txtIskontoOrani2.Text), 2)) / 100) * ((Math.Round(Convert.ToDecimal(txtKdvOrani2.Text), 2) + 100) / 100)).ToString("0.00");
            //txtKdvTutar2.Text = (Math.Round(Convert.ToDecimal(txtMiktar2.Text), 2) * Math.Round(Convert.ToDecimal(txtFiyat2.Text), 2) * ((100 - Math.Round(Convert.ToDecimal(txtIskontoOrani2.Text), 2)) / 100) * (Math.Round(Convert.ToDecimal(txtKdvOrani2.Text), 2) / 100)).ToString("0.00");


            s1ToplamTutar = Convert.ToDecimal(txtFiyat1.Text) * Convert.ToDecimal(txtMiktar1.Text);
            //s2ToplamTutar = Convert.ToDecimal(txtFiyat2.Text) * Convert.ToDecimal(txtMiktar2.Text);

            s1KDVTutar = Math.Round(Convert.ToDecimal(txtKdvTutar1.Text), 2);
            //s2KDVTutar = Math.Round(Convert.ToDecimal(txtKdvTutar2.Text), 2);

            s1IskontoTutar = Math.Round(Convert.ToDecimal(txtIskontoTutar1.Text), 2);
            //s2IskontoTutar = Math.Round(Convert.ToDecimal(txtIskontoTutar2.Text), 2);

            TotalLineExtentionAmount = s1ToplamTutar + s2ToplamTutar;

            TotalAllowanceCharge = s1IskontoTutar + s2IskontoTutar;

            TotalTaxExculisiveAmount = TotalLineExtentionAmount - TotalAllowanceCharge;
            TotalTaxInclusiveAmount = TotalLineExtentionAmount - TotalAllowanceCharge + s1KDVTutar + s2KDVTutar;


            //2.hesaplar

            txtToplamTutar1.Text = (Math.Round(Convert.ToDecimal(txtMiktar1.Text), 2) * Math.Round(Convert.ToDecimal(txtFiyat1.Text), 2) * ((100 - Math.Round(Convert.ToDecimal(txtIskontoOrani1.Text), 2)) / 100) * ((Math.Round(Convert.ToDecimal(txtKdvOrani1.Text), 2) + 100) / 100)).ToString("0.00");
            txtKdvTutar1.Text = (Math.Round(Convert.ToDecimal(txtMiktar1.Text), 2) * Math.Round(Convert.ToDecimal(txtFiyat1.Text), 2) * ((100 - Math.Round(Convert.ToDecimal(txtIskontoOrani1.Text), 2)) / 100) * (Math.Round(Convert.ToDecimal(txtKdvOrani1.Text), 2) / 100)).ToString("0.00");
            txtIskontoTutar1.Text = (Math.Round(Convert.ToDecimal(txtMiktar1.Text), 2) * Math.Round(Convert.ToDecimal(txtFiyat1.Text), 2) * (Math.Round(Convert.ToDecimal(txtIskontoOrani1.Text), 2) / 100)).ToString("0.00");
          
            


            txtFaturaKdvOran1.Text = txtKdvOrani1.Text;
            txtFaturaKdvTutar1.Text = txtKdvTutar1.Text;
            

            s1ToplamTutar = Math.Round(Convert.ToDecimal(txtFiyat1.Text), 2) * Math.Round(Convert.ToDecimal(txtMiktar1.Text), 2);

            s1KDVTutar = Math.Round(Convert.ToDecimal(txtKdvTutar1.Text), 2);

            s1IskontoTutar = Math.Round(Convert.ToDecimal(txtIskontoTutar1.Text), 2);
            

            TotalLineExtentionAmount = s1ToplamTutar + s2ToplamTutar;

            TotalAllowanceCharge = s1IskontoTutar + s2IskontoTutar;

            TotalTaxExculisiveAmount = TotalLineExtentionAmount - TotalAllowanceCharge;
            TotalTaxInclusiveAmount = TotalLineExtentionAmount - TotalAllowanceCharge + s1KDVTutar + s2KDVTutar;

        }
    }
}
