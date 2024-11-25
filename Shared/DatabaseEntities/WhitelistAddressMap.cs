using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class WhitelistAddressMap : IEntityTypeConfiguration<WhitelistAddress>
    {
        public void Configure(EntityTypeBuilder<WhitelistAddress> entity)
        {
            entity.ToTable("proxy_whitelist").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("address_id");
            entity.Property(x => x.DateTime).HasColumnName("address_datetime");
            entity.Property(x => x.Status).HasColumnName("address_status");
            entity.Property(x => x.IsDeleted).HasColumnName("address_deleted");
            entity.Property(x => x.HasAddress).HasColumnName("address_has_address");

            entity.Property(x => x.FirstName).HasColumnName("address_firstname");
            entity.Property(x => x.LastName).HasColumnName("address_lastname");
            entity.Property(x => x.CompanyName).HasColumnName("address_companyname");
            entity.Property(x => x.EmailAddress).HasColumnName("address_email");
            entity.Property(x => x.Value).HasColumnName("address_value");
        }
    }
}