using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class FrontendMap : IEntityTypeConfiguration<Frontend>
    {
        public void Configure(EntityTypeBuilder<Frontend> entity)
        {
            entity.ToTable("proxy_frontends").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("frontend_id");
            entity.Property(x => x.DateTime).HasColumnName("frontend_datetime");
            entity.Property(x => x.Status).HasColumnName("frontend_status");
            entity.Property(x => x.IsDeleted).HasColumnName("frontend_deleted");

            entity.Property(x => x.BackendId).HasColumnName("frontend_backend_id");
            entity.Property(x => x.SslWildcardId).HasColumnName("frontend_wildcard_id");
            entity.Property(x => x.Name).HasColumnName("frontend_name");

            entity.Property(x => x.UseLetsEncrypt).HasColumnName("frontend_use_letsencrypt");
            entity.Property(x => x.UseWildcard).HasColumnName("frontend_use_wildcard");
            entity.Property(x => x.HasWildcard).HasColumnName("frontend_has_wildcard");
            entity.Property(x => x.ChangedWildcard).HasColumnName("frontend_changed_wildcard");
            entity.Property(x => x.Reissue).HasColumnName("frontend_reissue");
            entity.Property(x => x.ReissueCount).HasColumnName("frontend_reissue_count");

            entity
                .HasOne(x => x.Backend)
                .WithMany(x => x.Frontends)
                .HasForeignKey(x => x.BackendId)
                .IsRequired(false);

            entity
                .HasMany(x => x.SubDomains)
                .WithOne(x => x.Frontend)
                .HasForeignKey(x => x.FrontendId)
                .IsRequired(false);

            entity
                .HasMany(x => x.SslOrders)
                .WithOne(x => x.Frontend)
                .HasForeignKey(x => x.FrontendId)
                .IsRequired(false);

            entity.HasOne(x => x.SslWildcard)
               .WithMany(x => x.Frontends)
               .HasForeignKey(x => x.SslWildcardId)
               .IsRequired(false);

            entity.HasQueryFilter(x => (x.Status == Cre8ion.Database.EntityStatus.Published || x.Status == Cre8ion.Database.EntityStatus.Concept));
        }
    }
}