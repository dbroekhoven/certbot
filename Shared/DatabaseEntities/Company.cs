using System;
using System.Collections.Generic;
using Cre8ion.Database;

namespace Website.Features.Company
{
    public class Company : IDatabaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public bool Supplier { get; set; }
        public bool InvoiceDigital { get; set; }
        public string InvoiceName { get; set; }
        public string InvoiceEmail { get; set; }
        public string ReminderName { get; set; }
        public string ReminderEmail { get; set; }
        public string InvoiceCc { get; set; }
        public string InvoiceBcc { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PoBox { get; set; }
        public string PoZipCode { get; set; }
        public string PoCity { get; set; }
        public string PoCountry { get; set; }
        public string VatNumber { get; set; }
        public int VatPercentage { get; set; }
        public int VatPayment { get; set; }
        public string KvkNumber { get; set; }
        public double HourlyRate { get; set; }
        public string Iban { get; set; }
        public string Language { get; set; }
        public string PaymentTerms { get; set; }
        public bool Exported { get; set; }
        public DateTime? DateProcessingAgreementSend { get; set; }
        public DateTime? DateProcessingAgreementSigned { get; set; }
        public bool GenerateReports { get; set; }
        public bool PersonalData { get; set; }
        public string Segment { get; set; }

        public ISet<Contact> Contacts { get; set; }

        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }
    }
}