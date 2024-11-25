using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Website.Features.Company
{
    public class ContactMap : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> entity)
        {
            entity.ToTable("users").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("users_id");
            entity.Property(x => x.DateTime).HasColumnName("users_datum");
            entity.Property(x => x.Status).HasColumnName("users_status");

            entity.Property(x => x.FirstName).HasColumnName("users_voornaam");
            entity.Property(x => x.Preposition).HasColumnName("users_tussenvoegsel");
            entity.Property(x => x.LastName).HasColumnName("users_achternaam");

            entity.Property(x => x.Salution).HasColumnName("users_titel");
            entity.Property(x => x.Function).HasColumnName("users_functie");
            entity.Property(x => x.PhoneNumber).HasColumnName("users_telefoon");
            entity.Property(x => x.MobileNumber).HasColumnName("users_mobiel");
            entity.Property(x => x.Email).HasColumnName("users_email");

            entity.Property(x => x.CompanyId).HasColumnName("users_company_id");
        }
    }
}