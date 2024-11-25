using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class SslWildcardMap : IEntityTypeConfiguration<SslWildcard>
    {
        public void Configure(EntityTypeBuilder<SslWildcard> entity)
        {
            entity.ToTable("proxy_ssl_wildcards").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("wildcard_id");
            entity.Property(x => x.DateTime).HasColumnName("wildcard_datetime");
            entity.Property(x => x.Status).HasColumnName("wildcard_status");

            entity.Property(x => x.Name).HasColumnName("wildcard_name");
            entity.Property(x => x.PfxBundle).HasColumnName("wildcard_pfx");

            entity.Property(x => x.NotBefore).HasColumnName("wildcard_not_before");
            entity.Property(x => x.NotAfter).HasColumnName("wildcard_not_after");

            entity.Property(x => x.HasLocalCertificate).HasColumnName("wildcard_has_localcertificate");
            entity.Property(x => x.ChangedLocalCertificate).HasColumnName("wildcard_changed_localcertificate");

            entity.Property(x => x.CaRootGroup).HasColumnName("wildcard_ca_root_group");

            entity.Property(x => x.CompanyId).HasColumnName("wildcard_company_id");
            entity.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);

            entity.HasQueryFilter(x => x.Status == Cre8ion.Database.EntityStatus.Published);
        }
    }
}