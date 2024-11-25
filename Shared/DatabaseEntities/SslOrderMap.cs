using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class SslOrderMap : IEntityTypeConfiguration<SslOrder>
    {
        public void Configure(EntityTypeBuilder<SslOrder> entity)
        {
            entity.ToTable("proxy_ssl_orders").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("order_id");
            entity.Property(x => x.DateTime).HasColumnName("order_datetime");
            entity.Property(x => x.Status).HasColumnName("order_status");

            entity.Property(x => x.FrontendId).HasColumnName("order_frontend_id");
            entity.Property(x => x.Location).HasColumnName("order_location");
            entity.Property(x => x.Error).HasColumnName("order_error");

            //entity.Property(x => x.Certificate).HasColumnName("order_crt_pem");
            //entity.Property(x => x.PrivateKey).HasColumnName("order_key_pem");
            //entity.Property(x => x.PfxBundle).HasColumnName("order_pfx");

            entity.Property(x => x.NotBefore).HasColumnName("order_not_before");
            entity.Property(x => x.NotAfter).HasColumnName("order_not_after");

            entity.Property(x => x.Finished).HasColumnName("order_finished");

            entity.Property(x => x.IsTested).HasColumnName("order_is_tested");
            entity.Property(x => x.IsValidated).HasColumnName("order_is_validated");
            entity.Property(x => x.IsFinished).HasColumnName("order_is_finished");

            entity.Property(x => x.HasLocalCertificate).HasColumnName("order_has_localcertificate");
            entity.Property(x => x.HasServerNameIndication).HasColumnName("order_has_servernameindication");

            entity.Property(x => x.CaRootGroup).HasColumnName("order_ca_root_group");
            entity.Property(x => x.ChaineNames).HasColumnName("order_ca_chain");

            entity.HasMany(x => x.Authorizations)
               .WithOne(x => x.SslOrder)
               .HasForeignKey(x => x.SslOrderId)
               .IsRequired(false);

            entity.HasQueryFilter(x => (x.Status == Cre8ion.Database.EntityStatus.Published || x.Status == Cre8ion.Database.EntityStatus.Approval));
        }
    }
}