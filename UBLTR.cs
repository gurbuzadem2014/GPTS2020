using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPTS
{
    public class UBLTR
    {
        private string prefixName = "cbc";
        private const string prefixArrayName = "cac";

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2")]
        public partial class Invoice
        {
            public Invoice()
            {
                if (this == null)
                    new Invoice();
            }

            public Invoice(int length)
            {
                AdditionalDocumentReference = new AdditionalDocumentReference[8];
                InvoiceLine = new InvoiceLine[length];
            }

            private String uBLVersionIDField;

            private string customizationIDField;

            private string profileIDField;

            private ID idField;

            private bool copyIndicatorField;

            private string uUIDField;

            private System.DateTime issueDateField;

            private System.DateTime issueTimeField;

            private string invoiceTypeCodeField;

            private string noteField;

            private DocumentCurrencyCode documentCurrencyCodeField;

            private byte lineCountNumericField;

            private DespatchDocumentReference despatchDocumentReferenceField;

            private AdditionalDocumentReference[] additionalDocumentReferenceField;

            private Signature signatureField;

            private AccountingSupplierParty accountingSupplierPartyField;

            private AccountingCustomerParty accountingCustomerPartyField;

            private TaxTotal taxTotalField;

            private LegalMonetaryTotal legalMonetaryTotalField;

            private InvoiceLine[] invoiceLineField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            //[System.Xml.Serialization.XmlElement("cbc:UBLVersionID")]
            public String UBLVersionID
            {
                get
                {
                    return this.uBLVersionIDField;
                }
                set
                {
                    this.uBLVersionIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string CustomizationID
            {
                get
                {
                    return this.customizationIDField;
                }
                set
                {
                    this.customizationIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string ProfileID
            {
                get
                {
                    return this.profileIDField;
                }
                set
                {
                    this.profileIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public ID ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public bool CopyIndicator
            {
                get
                {
                    return this.copyIndicatorField;
                }
                set
                {
                    this.copyIndicatorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string UUID
            {
                get
                {
                    return this.uUIDField;
                }
                set
                {
                    this.uUIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2", DataType = "date")]
            public System.DateTime IssueDate
            {
                get
                {
                    return this.issueDateField;
                }
                set
                {
                    this.issueDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2", DataType = "time")]
            public System.DateTime IssueTime
            {
                get
                {
                    return this.issueTimeField;
                }
                set
                {
                    this.issueTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string InvoiceTypeCode
            {
                get
                {
                    return this.invoiceTypeCodeField;
                }
                set
                {
                    this.invoiceTypeCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Note
            {
                get
                {
                    return this.noteField;
                }
                set
                {
                    this.noteField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public DocumentCurrencyCode DocumentCurrencyCode
            {
                get
                {
                    return this.documentCurrencyCodeField;
                }
                set
                {
                    this.documentCurrencyCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public byte LineCountNumeric
            {
                get
                {
                    return this.lineCountNumericField;
                }
                set
                {
                    this.lineCountNumericField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("DespatchDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
            public DespatchDocumentReference DespatchDocumentReference
            {
                get
                {
                    return this.despatchDocumentReferenceField;
                }
                set
                {
                    this.despatchDocumentReferenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("AdditionalDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
            public AdditionalDocumentReference[] AdditionalDocumentReference
            {
                get
                {
                    return this.additionalDocumentReferenceField;
                }
                set
                {
                    this.additionalDocumentReferenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
            public Signature Signature
            {
                get
                {
                    return this.signatureField;
                }
                set
                {
                    this.signatureField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
            public AccountingSupplierParty AccountingSupplierParty
            {
                get
                {
                    return this.accountingSupplierPartyField;
                }
                set
                {
                    this.accountingSupplierPartyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
            public AccountingCustomerParty AccountingCustomerParty
            {
                get
                {
                    return this.accountingCustomerPartyField;
                }
                set
                {
                    this.accountingCustomerPartyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
            public TaxTotal TaxTotal
            {
                get
                {
                    return this.taxTotalField;
                }
                set
                {
                    this.taxTotalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
            public LegalMonetaryTotal LegalMonetaryTotal
            {
                get
                {
                    return this.legalMonetaryTotalField;
                }
                set
                {
                    this.legalMonetaryTotalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("InvoiceLine", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
            public InvoiceLine[] InvoiceLine
            {
                get
                {
                    return this.invoiceLineField;
                }
                set
                {
                    this.invoiceLineField = value;
                }
            }


        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public partial class ID
        {

            private string schemeIDField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string schemeID
            {
                get
                {
                    return this.schemeIDField;
                }
                set
                {
                    this.schemeIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public partial class DocumentCurrencyCode
        {

            private string listAgencyNameField;

            private string listIDField;

            private string listNameField;

            private ushort listVersionIDField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string listAgencyName
            {
                get
                {
                    return this.listAgencyNameField;
                }
                set
                {
                    this.listAgencyNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string listID
            {
                get
                {
                    return this.listIDField;
                }
                set
                {
                    this.listIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string listName
            {
                get
                {
                    return this.listNameField;
                }
                set
                {
                    this.listNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort listVersionID
            {
                get
                {
                    return this.listVersionIDField;
                }
                set
                {
                    this.listVersionIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class DespatchDocumentReference
        {

            private ID idField;

            private System.DateTime issueDateField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public ID ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2", DataType = "date")]
            public System.DateTime IssueDate
            {
                get
                {
                    return this.issueDateField;
                }
                set
                {
                    this.issueDateField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AdditionalDocumentReference
        {
            //public AdditionalDocumentReference(int count)
            //{
            //    AdditionalDocumentReference [] adr = new AdditionalDocumentReference[count];
            //}

            private ID idField;

            private System.DateTime issueDateField;

            private AdditionalDocumentReferenceAttachment attachmentField;

            private string documentTypeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public ID ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2", DataType = "date")]
            public System.DateTime IssueDate
            {
                get
                {
                    return this.issueDateField;
                }
                set
                {
                    this.issueDateField = value;
                }
            }

            /// <remarks/>
            public AdditionalDocumentReferenceAttachment Attachment
            {
                get
                {
                    return this.attachmentField;
                }
                set
                {
                    this.attachmentField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string DocumentType
            {
                get
                {
                    return this.documentTypeField;
                }
                set
                {
                    this.documentTypeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AdditionalDocumentReferenceAttachment
        {
            public AdditionalDocumentReferenceAttachment()
            {

            }

            private EmbeddedDocumentBinaryObject embeddedDocumentBinaryObjectField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public EmbeddedDocumentBinaryObject EmbeddedDocumentBinaryObject
            {
                get
                {
                    return this.embeddedDocumentBinaryObjectField;
                }
                set
                {
                    this.embeddedDocumentBinaryObjectField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        //[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2", IsNullable = false)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public partial class EmbeddedDocumentBinaryObject : AdditionalDocumentReference
        {
            public EmbeddedDocumentBinaryObject()
            {
                if (this.Attachment == null)
                    this.Attachment = new AdditionalDocumentReferenceAttachment();
            }

            private string characterSetCodeField;

            private string encodingCodeField;

            private string filenameField;

            private string mimeCodeField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string characterSetCode
            {
                get
                {
                    return this.characterSetCodeField;
                }
                set
                {
                    this.characterSetCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string encodingCode
            {
                get
                {
                    return this.encodingCodeField;
                }
                set
                {
                    this.encodingCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string filename
            {
                get
                {
                    return this.filenameField;
                }
                set
                {
                    this.filenameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string mimeCode
            {
                get
                {
                    return this.mimeCodeField;
                }
                set
                {
                    this.mimeCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class Signature
        {

            private ID idField;

            private SignatureSignatoryParty signatoryPartyField;

            private SignatureDigitalSignatureAttachment digitalSignatureAttachmentField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public ID ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public SignatureSignatoryParty SignatoryParty
            {
                get
                {
                    return this.signatoryPartyField;
                }
                set
                {
                    this.signatoryPartyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public SignatureDigitalSignatureAttachment DigitalSignatureAttachment
            {
                get
                {
                    return this.digitalSignatureAttachmentField;
                }
                set
                {
                    this.digitalSignatureAttachmentField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class SignatureSignatoryParty
        {

            private SignatureSignatoryPartyPartyIdentification partyIdentificationField;

            private SignatureSignatoryPartyPartyName partyNameField;

            private SignatureSignatoryPartyPostalAddress postalAddressField;

            private SignatureSignatoryPartyPartyTaxScheme partyTaxSchemeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(ElementName = "PartyIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public SignatureSignatoryPartyPartyIdentification PartyIdentification
            {
                get
                {
                    return this.partyIdentificationField;
                }
                set
                {
                    this.partyIdentificationField = value;
                }
            }
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(ElementName = "PartyName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public SignatureSignatoryPartyPartyName PartyName
            {
                get
                {
                    return this.partyNameField;
                }
                set
                {
                    this.partyNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(ElementName = "PostalAddress", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public SignatureSignatoryPartyPostalAddress PostalAddress
            {
                get
                {
                    return this.postalAddressField;
                }
                set
                {
                    this.postalAddressField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public SignatureSignatoryPartyPartyTaxScheme PartyTaxScheme
            {
                get
                {
                    return this.partyTaxSchemeField;
                }
                set
                {
                    this.partyTaxSchemeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class SignatureSignatoryPartyPartyIdentification
        {

            private ID idField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public ID ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class SignatureSignatoryPartyPartyName
        {

            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class SignatureSignatoryPartyPostalAddress
        {

            private object streetNameField;

            private object buildingNumberField;

            private string citySubdivisionNameField;

            private string cityNameField;

            private object postalZoneField;

            private string regionField;

            private SignatureSignatoryPartyPostalAddressCountry countryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object StreetName
            {
                get
                {
                    return this.streetNameField;
                }
                set
                {
                    this.streetNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object BuildingNumber
            {
                get
                {
                    return this.buildingNumberField;
                }
                set
                {
                    this.buildingNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string CitySubdivisionName
            {
                get
                {
                    return this.citySubdivisionNameField;
                }
                set
                {
                    this.citySubdivisionNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string CityName
            {
                get
                {
                    return this.cityNameField;
                }
                set
                {
                    this.cityNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object PostalZone
            {
                get
                {
                    return this.postalZoneField;
                }
                set
                {
                    this.postalZoneField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Region
            {
                get
                {
                    return this.regionField;
                }
                set
                {
                    this.regionField = value;
                }
            }

            /// <remarks/>
            public SignatureSignatoryPartyPostalAddressCountry Country
            {
                get
                {
                    return this.countryField;
                }
                set
                {
                    this.countryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class SignatureSignatoryPartyPostalAddressCountry
        {

            private string identificationCodeField;

            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string IdentificationCode
            {
                get
                {
                    return this.identificationCodeField;
                }
                set
                {
                    this.identificationCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class SignatureSignatoryPartyPartyTaxScheme
        {
            public SignatureSignatoryPartyPartyTaxScheme()
            {

            }

            private SignatureSignatoryPartyPartyTaxSchemeTaxScheme taxSchemeField;

            /// <remarks/>
            public SignatureSignatoryPartyPartyTaxSchemeTaxScheme TaxScheme
            {
                get
                {
                    return this.taxSchemeField;
                }
                set
                {
                    this.taxSchemeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class SignatureSignatoryPartyPartyTaxSchemeTaxScheme : SignatureSignatoryParty
        {
            public SignatureSignatoryPartyPartyTaxSchemeTaxScheme()
            {
                if (this.PartyTaxScheme == null)
                    this.PartyTaxScheme = new SignatureSignatoryPartyPartyTaxScheme();
            }

            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class SignatureDigitalSignatureAttachment
        {
            private ExternalReference externalReferenceField;

            /// <remarks/>
            public ExternalReference ExternalReference
            {
                get
                {
                    return this.externalReferenceField;
                }
                set
                {
                    this.externalReferenceField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class ExternalReference
        {

            private object uRIField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(ElementName = "URI", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object URI
            {
                get
                {
                    return this.uRIField;
                }
                set
                {
                    this.uRIField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingSupplierParty
        {
            public AccountingSupplierParty()
            {

            }
            private AccountingSupplierPartyParty partyField;

            /// <remarks/>
            public AccountingSupplierPartyParty Party
            {
                get
                {
                    return this.partyField;
                }
                set
                {
                    this.partyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingSupplierPartyParty
        {
            public AccountingSupplierPartyParty()
            {
                PartyIdentification = new AccountingSupplierPartyPartyPartyIdentification[3];
            }

            private object websiteURIField;

            private AccountingSupplierPartyPartyPartyIdentification[] partyIdentificationField;

            private AccountingSupplierPartyPartyPartyName partyNameField;

            private AccountingSupplierPartyPartyPostalAddress postalAddressField;

            private AccountingSupplierPartyPartyPartyTaxScheme partyTaxSchemeField;

            private AccountingSupplierPartyPartyContact contactField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object WebsiteURI
            {
                get
                {
                    return this.websiteURIField;
                }
                set
                {
                    this.websiteURIField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("PartyIdentification")]
            public AccountingSupplierPartyPartyPartyIdentification[] PartyIdentification
            {
                get
                {
                    return this.partyIdentificationField;
                }
                set
                {
                    this.partyIdentificationField = value;
                }
            }

            /// <remarks/>
            public AccountingSupplierPartyPartyPartyName PartyName
            {
                get
                {
                    return this.partyNameField;
                }
                set
                {
                    this.partyNameField = value;
                }
            }

            /// <remarks/>
            public AccountingSupplierPartyPartyPostalAddress PostalAddress
            {
                get
                {
                    return this.postalAddressField;
                }
                set
                {
                    this.postalAddressField = value;
                }
            }

            /// <remarks/>
            public AccountingSupplierPartyPartyPartyTaxScheme PartyTaxScheme
            {
                get
                {
                    return this.partyTaxSchemeField;
                }
                set
                {
                    this.partyTaxSchemeField = value;
                }
            }

            /// <remarks/>
            public AccountingSupplierPartyPartyContact Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingSupplierPartyPartyPartyIdentification
        {
            private ID idField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public ID ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingSupplierPartyPartyPartyName
        {

            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingSupplierPartyPartyPostalAddress
        {

            private object streetNameField;

            private object buildingNumberField;

            private string citySubdivisionNameField;

            private string cityNameField;

            private object postalZoneField;

            private AccountingSupplierPartyPartyPostalAddressCountry countryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object StreetName
            {
                get
                {
                    return this.streetNameField;
                }
                set
                {
                    this.streetNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object BuildingNumber
            {
                get
                {
                    return this.buildingNumberField;
                }
                set
                {
                    this.buildingNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string CitySubdivisionName
            {
                get
                {
                    return this.citySubdivisionNameField;
                }
                set
                {
                    this.citySubdivisionNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string CityName
            {
                get
                {
                    return this.cityNameField;
                }
                set
                {
                    this.cityNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object PostalZone
            {
                get
                {
                    return this.postalZoneField;
                }
                set
                {
                    this.postalZoneField = value;
                }
            }

            /// <remarks/>
            public AccountingSupplierPartyPartyPostalAddressCountry Country
            {
                get
                {
                    return this.countryField;
                }
                set
                {
                    this.countryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingSupplierPartyPartyPostalAddressCountry
        {

            private string identificationCodeField;

            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string IdentificationCode
            {
                get
                {
                    return this.identificationCodeField;
                }
                set
                {
                    this.identificationCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingSupplierPartyPartyPartyTaxScheme
        {

            private AccountingSupplierPartyPartyPartyTaxSchemeTaxScheme taxSchemeField;

            /// <remarks/>
            public AccountingSupplierPartyPartyPartyTaxSchemeTaxScheme TaxScheme
            {
                get
                {
                    return this.taxSchemeField;
                }
                set
                {
                    this.taxSchemeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingSupplierPartyPartyPartyTaxSchemeTaxScheme
        {

            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingSupplierPartyPartyContact
        {

            private object telephoneField;

            private object telefaxField;

            private object electronicMailField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object Telephone
            {
                get
                {
                    return this.telephoneField;
                }
                set
                {
                    this.telephoneField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object Telefax
            {
                get
                {
                    return this.telefaxField;
                }
                set
                {
                    this.telefaxField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object ElectronicMail
            {
                get
                {
                    return this.electronicMailField;
                }
                set
                {
                    this.electronicMailField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingCustomerParty
        {

            private AccountingCustomerPartyParty partyField;

            /// <remarks/>
            public AccountingCustomerPartyParty Party
            {
                get
                {
                    return this.partyField;
                }
                set
                {
                    this.partyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingCustomerPartyParty
        {

            private object websiteURIField;

            private AccountingCustomerPartyPartyPartyIdentification partyIdentificationField;

            private AccountingCustomerPartyPartyPartyName partyNameField;

            private AccountingCustomerPartyPartyPostalAddress postalAddressField;

            private AccountingCustomerPartyPartyPartyTaxScheme partyTaxSchemeField;

            private AccountingCustomerPartyPartyContact contactField;

            private AccountingCustomerPartyPartyPerson personField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object WebsiteURI
            {
                get
                {
                    return this.websiteURIField;
                }
                set
                {
                    this.websiteURIField = value;
                }
            }

            /// <remarks/>
            public AccountingCustomerPartyPartyPartyIdentification PartyIdentification
            {
                get
                {
                    return this.partyIdentificationField;
                }
                set
                {
                    this.partyIdentificationField = value;
                }
            }

            /// <remarks/>
            public AccountingCustomerPartyPartyPartyName PartyName
            {
                get
                {
                    return this.partyNameField;
                }
                set
                {
                    this.partyNameField = value;
                }
            }

            /// <remarks/>
            public AccountingCustomerPartyPartyPostalAddress PostalAddress
            {
                get
                {
                    return this.postalAddressField;
                }
                set
                {
                    this.postalAddressField = value;
                }
            }

            /// <remarks/>
            public AccountingCustomerPartyPartyPartyTaxScheme PartyTaxScheme
            {
                get
                {
                    return this.partyTaxSchemeField;
                }
                set
                {
                    this.partyTaxSchemeField = value;
                }
            }

            /// <remarks/>
            public AccountingCustomerPartyPartyContact Contact
            {
                get
                {
                    return this.contactField;
                }
                set
                {
                    this.contactField = value;
                }
            }

            /// <remarks/>
            public AccountingCustomerPartyPartyPerson Person
            {
                get
                {
                    return this.personField;
                }
                set
                {
                    this.personField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingCustomerPartyPartyPartyIdentification
        {

            private ID idField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public ID ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingCustomerPartyPartyPartyName
        {

            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingCustomerPartyPartyPostalAddress
        {

            private object streetNameField;

            private object buildingNumberField;

            private string citySubdivisionNameField;

            private string cityNameField;

            private object postalZoneField;

            private AccountingCustomerPartyPartyPostalAddressCountry countryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object StreetName
            {
                get
                {
                    return this.streetNameField;
                }
                set
                {
                    this.streetNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object BuildingNumber
            {
                get
                {
                    return this.buildingNumberField;
                }
                set
                {
                    this.buildingNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string CitySubdivisionName
            {
                get
                {
                    return this.citySubdivisionNameField;
                }
                set
                {
                    this.citySubdivisionNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string CityName
            {
                get
                {
                    return this.cityNameField;
                }
                set
                {
                    this.cityNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object PostalZone
            {
                get
                {
                    return this.postalZoneField;
                }
                set
                {
                    this.postalZoneField = value;
                }
            }

            /// <remarks/>
            public AccountingCustomerPartyPartyPostalAddressCountry Country
            {
                get
                {
                    return this.countryField;
                }
                set
                {
                    this.countryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingCustomerPartyPartyPostalAddressCountry
        {

            private string identificationCodeField;

            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string IdentificationCode
            {
                get
                {
                    return this.identificationCodeField;
                }
                set
                {
                    this.identificationCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingCustomerPartyPartyPartyTaxScheme
        {

            private AccountingCustomerPartyPartyPartyTaxSchemeTaxScheme taxSchemeField;

            /// <remarks/>
            public AccountingCustomerPartyPartyPartyTaxSchemeTaxScheme TaxScheme
            {
                get
                {
                    return this.taxSchemeField;
                }
                set
                {
                    this.taxSchemeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingCustomerPartyPartyPartyTaxSchemeTaxScheme
        {

            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingCustomerPartyPartyContact
        {

            private object telephoneField;

            private object telefaxField;

            private object electronicMailField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object Telephone
            {
                get
                {
                    return this.telephoneField;
                }
                set
                {
                    this.telephoneField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object Telefax
            {
                get
                {
                    return this.telefaxField;
                }
                set
                {
                    this.telefaxField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public object ElectronicMail
            {
                get
                {
                    return this.electronicMailField;
                }
                set
                {
                    this.electronicMailField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class AccountingCustomerPartyPartyPerson
        {

            private string firstNameField;

            private string familyNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string FirstName
            {
                get
                {
                    return this.firstNameField;
                }
                set
                {
                    this.firstNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string FamilyName
            {
                get
                {
                    return this.familyNameField;
                }
                set
                {
                    this.familyNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class TaxTotal
        {

            private TaxAmount taxAmountField;

            private TaxTotalTaxSubtotal taxSubtotalField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public TaxAmount TaxAmount
            {
                get
                {
                    return this.taxAmountField;
                }
                set
                {
                    this.taxAmountField = value;
                }
            }

            /// <remarks/>
            public TaxTotalTaxSubtotal TaxSubtotal
            {
                get
                {
                    return this.taxSubtotalField;
                }
                set
                {
                    this.taxSubtotalField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public partial class TaxAmount
        {

            private string currencyIDField;

            private Double valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string currencyID
            {
                get
                {
                    return this.currencyIDField;
                }
                set
                {
                    this.currencyIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public Double Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class TaxTotalTaxSubtotal
        {

            private TaxableAmount taxableAmountField;

            private TaxAmount taxAmountField;

            private Double percentField;

            private TaxTotalTaxSubtotalTaxCategory taxCategoryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public TaxableAmount TaxableAmount
            {
                get
                {
                    return this.taxableAmountField;
                }
                set
                {
                    this.taxableAmountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public TaxAmount TaxAmount
            {
                get
                {
                    return this.taxAmountField;
                }
                set
                {
                    this.taxAmountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public Double Percent
            {
                get
                {
                    return this.percentField;
                }
                set
                {
                    this.percentField = value;
                }
            }

            /// <remarks/>
            public TaxTotalTaxSubtotalTaxCategory TaxCategory
            {
                get
                {
                    return this.taxCategoryField;
                }
                set
                {
                    this.taxCategoryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public partial class TaxableAmount
        {

            private string currencyIDField;

            private Double valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string currencyID
            {
                get
                {
                    return this.currencyIDField;
                }
                set
                {
                    this.currencyIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public Double Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class TaxTotalTaxSubtotalTaxCategory
        {

            private TaxTotalTaxSubtotalTaxCategoryTaxScheme taxSchemeField;

            /// <remarks/>
            public TaxTotalTaxSubtotalTaxCategoryTaxScheme TaxScheme
            {
                get
                {
                    return this.taxSchemeField;
                }
                set
                {
                    this.taxSchemeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class TaxTotalTaxSubtotalTaxCategoryTaxScheme
        {

            private string nameField;

            private string taxTypeCodeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string TaxTypeCode
            {
                get
                {
                    return this.taxTypeCodeField;
                }
                set
                {
                    this.taxTypeCodeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class LegalMonetaryTotal
        {

            private LineExtensionAmount lineExtensionAmountField;

            private TaxExclusiveAmount taxExclusiveAmountField;

            private TaxInclusiveAmount taxInclusiveAmountField;

            private AllowanceTotalAmount allowanceTotalAmountField;

            private PayableAmount payableAmountField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public LineExtensionAmount LineExtensionAmount
            {
                get
                {
                    return this.lineExtensionAmountField;
                }
                set
                {
                    this.lineExtensionAmountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public TaxExclusiveAmount TaxExclusiveAmount
            {
                get
                {
                    return this.taxExclusiveAmountField;
                }
                set
                {
                    this.taxExclusiveAmountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public TaxInclusiveAmount TaxInclusiveAmount
            {
                get
                {
                    return this.taxInclusiveAmountField;
                }
                set
                {
                    this.taxInclusiveAmountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public AllowanceTotalAmount AllowanceTotalAmount
            {
                get
                {
                    return this.allowanceTotalAmountField;
                }
                set
                {
                    this.allowanceTotalAmountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public PayableAmount PayableAmount
            {
                get
                {
                    return this.payableAmountField;
                }
                set
                {
                    this.payableAmountField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public partial class LineExtensionAmount
        {

            private string currencyIDField;

            private double valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string currencyID
            {
                get
                {
                    return this.currencyIDField;
                }
                set
                {
                    this.currencyIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public double Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public partial class TaxExclusiveAmount
        {

            private string currencyIDField;

            private double valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string currencyID
            {
                get
                {
                    return this.currencyIDField;
                }
                set
                {
                    this.currencyIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public double Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public partial class TaxInclusiveAmount
        {

            private string currencyIDField;

            private double valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string currencyID
            {
                get
                {
                    return this.currencyIDField;
                }
                set
                {
                    this.currencyIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public double Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public partial class AllowanceTotalAmount
        {

            private string currencyIDField;

            private byte valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string currencyID
            {
                get
                {
                    return this.currencyIDField;
                }
                set
                {
                    this.currencyIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public byte Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public partial class PayableAmount
        {

            private string currencyIDField;

            private double valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string currencyID
            {
                get
                {
                    return this.currencyIDField;
                }
                set
                {
                    this.currencyIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public double Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class InvoiceLine
        {
            private ID idField;

            private string noteField;

            private InvoicedQuantity invoicedQuantityField;

            private LineExtensionAmount lineExtensionAmountField;

            private InvoiceLineTaxTotal taxTotalField;

            private InvoiceLineItem itemField;

            private InvoiceLinePrice priceField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public ID ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Note
            {
                get
                {
                    return this.noteField;
                }
                set
                {
                    this.noteField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public InvoicedQuantity InvoicedQuantity
            {
                get
                {
                    return this.invoicedQuantityField;
                }
                set
                {
                    this.invoicedQuantityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public LineExtensionAmount LineExtensionAmount
            {
                get
                {
                    return this.lineExtensionAmountField;
                }
                set
                {
                    this.lineExtensionAmountField = value;
                }
            }

            /// <remarks/>
            public InvoiceLineTaxTotal TaxTotal
            {
                get
                {
                    return this.taxTotalField;
                }
                set
                {
                    this.taxTotalField = value;
                }
            }

            /// <remarks/>
            public InvoiceLineItem Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }

            /// <remarks/>
            public InvoiceLinePrice Price
            {
                get
                {
                    return this.priceField;
                }
                set
                {
                    this.priceField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public partial class InvoicedQuantity
        {

            private string unitCodeField;

            private int valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string unitCode
            {
                get
                {
                    return this.unitCodeField;
                }
                set
                {
                    this.unitCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public int Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class InvoiceLineTaxTotal
        {

            private TaxAmount taxAmountField;

            private InvoiceLineTaxTotalTaxSubtotal taxSubtotalField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public TaxAmount TaxAmount
            {
                get
                {
                    return this.taxAmountField;
                }
                set
                {
                    this.taxAmountField = value;
                }
            }

            /// <remarks/>
            public InvoiceLineTaxTotalTaxSubtotal TaxSubtotal
            {
                get
                {
                    return this.taxSubtotalField;
                }
                set
                {
                    this.taxSubtotalField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class InvoiceLineTaxTotalTaxSubtotal
        {

            private TaxableAmount taxableAmountField;

            private TaxAmount taxAmountField;

            private double percentField;

            private InvoiceLineTaxTotalTaxSubtotalTaxCategory taxCategoryField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public TaxableAmount TaxableAmount
            {
                get
                {
                    return this.taxableAmountField;
                }
                set
                {
                    this.taxableAmountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public TaxAmount TaxAmount
            {
                get
                {
                    return this.taxAmountField;
                }
                set
                {
                    this.taxAmountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public double Percent
            {
                get
                {
                    return this.percentField;
                }
                set
                {
                    this.percentField = value;
                }
            }

            /// <remarks/>
            public InvoiceLineTaxTotalTaxSubtotalTaxCategory TaxCategory
            {
                get
                {
                    return this.taxCategoryField;
                }
                set
                {
                    this.taxCategoryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class InvoiceLineTaxTotalTaxSubtotalTaxCategory
        {

            private InvoiceLineTaxTotalTaxSubtotalTaxCategoryTaxScheme taxSchemeField;

            /// <remarks/>
            public InvoiceLineTaxTotalTaxSubtotalTaxCategoryTaxScheme TaxScheme
            {
                get
                {
                    return this.taxSchemeField;
                }
                set
                {
                    this.taxSchemeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class InvoiceLineTaxTotalTaxSubtotalTaxCategoryTaxScheme
        {

            private string nameField;

            private string taxTypeCodeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string TaxTypeCode
            {
                get
                {
                    return this.taxTypeCodeField;
                }
                set
                {
                    this.taxTypeCodeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class InvoiceLineItem
        {

            private string descriptionField;

            private string nameField;

            private InvoiceLineItemSellersItemIdentification sellersItemIdentificationField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public InvoiceLineItemSellersItemIdentification SellersItemIdentification
            {
                get
                {
                    return this.sellersItemIdentificationField;
                }
                set
                {
                    this.sellersItemIdentificationField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class InvoiceLineItemSellersItemIdentification
        {

            private ID idField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public ID ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public partial class InvoiceLinePrice
        {

            private PriceAmount priceAmountField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
            public PriceAmount PriceAmount
            {
                get
                {
                    return this.priceAmountField;
                }
                set
                {
                    this.priceAmountField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public partial class PriceAmount
        {

            private string currencyIDField;

            private double valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string currencyID
            {
                get
                {
                    return this.currencyIDField;
                }
                set
                {
                    this.currencyIDField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public double Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

    }
}
