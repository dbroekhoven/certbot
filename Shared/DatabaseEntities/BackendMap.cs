using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class BackendMap : IEntityTypeConfiguration<Backend>
    {
        public void Configure(EntityTypeBuilder<Backend> entity)
        {
            entity.ToTable("proxy_backends").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("backend_id");
            entity.Property(x => x.DateTime).HasColumnName("backend_datetime");
            entity.Property(x => x.Status).HasColumnName("backend_status");
            entity.Property(x => x.IsDeleted).HasColumnName("backend_deleted");

            entity.Property(x => x.Name).HasColumnName("backend_name");
            entity.Property(x => x.InternalAddress).HasColumnName("backend_internal_address");
            entity.Property(x => x.ServerPool).HasColumnName("backend_serverpool");
            entity.Property(x => x.ChangedServerPool).HasColumnName("backend_changed_serverpool");
            entity.Property(x => x.HasServerPool).HasColumnName("backend_has_serverpool");
            entity.Property(x => x.HasServerPoolRule).HasColumnName("backend_has_serverpoolrule");
            entity.Property(x => x.ChangedServerPoolRule).HasColumnName("backend_changed_serverpoolrule");
            entity.Property(x => x.AllInOneCertificate).HasColumnName("backend_allinone_certificate");

            entity.Property(x => x.ExternalAddress).HasColumnName("backend_external_address");
            entity.Property(x => x.ProtectionProfile).HasColumnName("backend_protection_profile");
            entity.Property(x => x.HasContentRouting).HasColumnName("backend_has_contentrouting");
            entity.Property(x => x.HasServerPolicyRule).HasColumnName("backend_has_serverpolicyrule");
            entity.Property(x => x.ChangedServerPolicyRule).HasColumnName("backend_changed_serverpolicyrule");
            entity.Property(x => x.UseIPv6).HasColumnName("backend_use_ipv6");
            entity.Property(x => x.HasIPv6).HasColumnName("backend_has_ipv6");

            entity.Property(x => x.IsCdn).HasColumnName("backend_is_cdn");

            entity.Property(x => x.IsRedirect).HasColumnName("backend_is_redirect");
            entity.Property(x => x.RedirectUrl).HasColumnName("backend_redirect_url");

            entity.Property(x => x.CompanyId).HasColumnName("backend_comapny_id");
            entity.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);

            entity.HasQueryFilter(x => (x.Status == Cre8ion.Database.EntityStatus.Published || x.Status == Cre8ion.Database.EntityStatus.Concept));
        }
    }
}