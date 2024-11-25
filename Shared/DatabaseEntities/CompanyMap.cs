using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Website.Features.Company
{
    public class CompanyMap : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> entity)
        {
            entity.ToTable("company").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("company_id");
            entity.Property(x => x.DateTime).HasColumnName("company_datum");
            entity.Property(x => x.Status).HasColumnName("company_status");

            entity.Property(x => x.Name).HasColumnName("company_name");

            entity.Property(x => x.PhoneNumber).HasColumnName("company_telnr");
            entity.Property(x => x.Email).HasColumnName("company_email");
            entity.Property(x => x.Website).HasColumnName("company_website");

            entity.Property(x => x.Supplier).HasColumnName("company_leverancier_bit");
            entity.Property(x => x.PersonalData).HasColumnName("company_verwerken_persoongegevens_bit");

            entity.Property(x => x.InvoiceDigital).HasColumnName("company_invoice_digital_bit");
            entity.Property(x => x.InvoiceName).HasColumnName("company_invoice_mail_name");
            entity.Property(x => x.InvoiceEmail).HasColumnName("company_invoice_mail_to");
            entity.Property(x => x.ReminderName).HasColumnName("company_reminder_mail_name");
            entity.Property(x => x.ReminderEmail).HasColumnName("company_reminder_mail_to");
            entity.Property(x => x.InvoiceCc).HasColumnName("company_invoice_mail_cc");
            entity.Property(x => x.InvoiceBcc).HasColumnName("company_invoice_mail_bcc");

            entity.Property(x => x.Address).HasColumnName("company_address");
            entity.Property(x => x.ZipCode).HasColumnName("company_postalcode");
            entity.Property(x => x.City).HasColumnName("company_city");
            entity.Property(x => x.Country).HasColumnName("company_country");

            entity.Property(x => x.PoBox).HasColumnName("company_postbus");
            entity.Property(x => x.PoZipCode).HasColumnName("company_pbpostcode");
            entity.Property(x => x.PoCity).HasColumnName("company_pbplaats");
            entity.Property(x => x.PoCountry).HasColumnName("company_pbland");

            entity.Property(x => x.VatNumber).HasColumnName("company_vatnr");
            entity.Property(x => x.VatPercentage).HasColumnName("company_btwtarief");
            entity.Property(x => x.VatPayment).HasColumnName("company_btwafdracht");
            entity.Property(x => x.KvkNumber).HasColumnName("company_kvknr");
            entity.Property(x => x.HourlyRate).HasColumnName("company_uurtarief");
            entity.Property(x => x.Iban).HasColumnName("company_iban");
            entity.Property(x => x.Language).HasColumnName("company_taal");
            entity.Property(x => x.PaymentTerms).HasColumnName("company_betaaltermijn");
            entity.Property(x => x.Exported).HasColumnName("company_isexported");

            entity.Property(x => x.DateProcessingAgreementSend).HasColumnName("company_verwerkersovereenkomst_send");
            entity.Property(x => x.DateProcessingAgreementSigned).HasColumnName("company_verwerkersovereenkomst_ondertekend");
            entity.Property(x => x.GenerateReports).HasColumnName("company_generate_reports");
            entity.Property(x => x.Segment).HasColumnName("company_segment");

            entity.HasMany(x => x.Contacts).WithOne().HasForeignKey(x => x.CompanyId);
        }
    }
}