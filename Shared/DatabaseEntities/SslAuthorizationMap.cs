using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class SslAuthorizationMap : IEntityTypeConfiguration<SslAuthorization>
    {
        public void Configure(EntityTypeBuilder<SslAuthorization> entity)
        {
            entity.ToTable("proxy_ssl_authorizations").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("auth_id");
            entity.Property(x => x.DateTime).HasColumnName("auth_datetime");
            entity.Property(x => x.Status).HasColumnName("auth_status");

            entity.Property(x => x.SslOrderId).HasColumnName("auth_ssl_order_id");
            entity.Property(x => x.Domain).HasColumnName("auth_domain");
            entity.Property(x => x.Location).HasColumnName("auth_location");

            entity.Property(x => x.Token).HasColumnName("auth_token");
            entity.Property(x => x.Key).HasColumnName("auth_key");

            entity.Property(x => x.Challenge).HasColumnName("auth_challenge");
            entity.Property(x => x.Error).HasColumnName("auth_error");

            entity.Property(x => x.Validated).HasColumnName("auth_validated");
            entity.Property(x => x.Finished).HasColumnName("auth_finished");

            entity.HasQueryFilter(x => (x.Status == Cre8ion.Database.EntityStatus.Published || x.Status == Cre8ion.Database.EntityStatus.Approval));
        }
    }
}