using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class SslOrderDataMap : IEntityTypeConfiguration<SslOrderData>
    {
        public void Configure(EntityTypeBuilder<SslOrderData> entity)
        {
            entity.ToTable("proxy_ssl_orders_data").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("data_id");
            entity.Property(x => x.DateTime).HasColumnName("data_datetime");
            entity.Property(x => x.Status).HasColumnName("data_status");

            entity.Property(x => x.SslOrderId).HasColumnName("data_order_id");

            entity.Property(x => x.Certificate).HasColumnName("data_crt_pem");
            entity.Property(x => x.PrivateKey).HasColumnName("data_key_pem");
            entity.Property(x => x.PfxBundle).HasColumnName("data_pfx");

            entity.HasQueryFilter(x => (x.Status == Cre8ion.Database.EntityStatus.Published || x.Status == Cre8ion.Database.EntityStatus.Approval));
        }
    }
}