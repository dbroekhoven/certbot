using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class AddressMap : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entity)
        {
            entity.ToTable("proxy_addresses").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("address_id");
            entity.Property(x => x.DateTime).HasColumnName("address_datetime");
            entity.Property(x => x.Status).HasColumnName("address_status");
            entity.Property(x => x.IsDeleted).HasColumnName("address_deleted");

            entity.Property(x => x.Name).HasColumnName("address_name");
            entity.Property(x => x.Value).HasColumnName("address_value");
            entity.Property(x => x.IsBlocked).HasColumnName("address_is_block");
            entity.Property(x => x.IsFtp).HasColumnName("address_is_ftp");
            entity.Property(x => x.HasAddress).HasColumnName("address_has_address");

            entity.HasQueryFilter(x => x.Status == Cre8ion.Database.EntityStatus.Published);
        }
    }
}