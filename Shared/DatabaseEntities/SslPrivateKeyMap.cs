using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class SslPrivateKeyMap : IEntityTypeConfiguration<SslPrivateKey>
    {
        public void Configure(EntityTypeBuilder<SslPrivateKey> entity)
        {
            entity.ToTable("proxy_ssl_private_keys").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("private_id");
            entity.Property(x => x.DateTime).HasColumnName("private_datetime");
            entity.Property(x => x.Status).HasColumnName("private_status");

            entity.Property(x => x.Value).HasColumnName("private_key_pem");

            entity.HasQueryFilter(x => x.Status == Cre8ion.Database.EntityStatus.Published);
        }
    }
}