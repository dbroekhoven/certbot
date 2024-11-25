using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class SubDomainMap : IEntityTypeConfiguration<SubDomain>
    {
        public void Configure(EntityTypeBuilder<SubDomain> entity)
        {
            entity.ToTable("proxy_subdomains").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("sub_id");
            entity.Property(x => x.DateTime).HasColumnName("sub_datetime");
            entity.Property(x => x.Status).HasColumnName("sub_status");

            entity.Property(x => x.FrontendId).HasColumnName("sub_frontend_id");
            entity.Property(x => x.Name).HasColumnName("sub_name");
            entity.Property(x => x.IsDeleted).HasColumnName("sub_deleted");
            entity.Property(x => x.HasContentRoutingRule).HasColumnName("sub_has_contentroutingrule");
            entity.Property(x => x.HasRewriteRule).HasColumnName("sub_has_rewriterule");
            entity.Property(x => x.ChangedRewriteRule).HasColumnName("sub_changed_rewriterule");

            entity.HasQueryFilter(x => (x.Status == Cre8ion.Database.EntityStatus.Published || x.Status == Cre8ion.Database.EntityStatus.Concept));
        }
    }
}