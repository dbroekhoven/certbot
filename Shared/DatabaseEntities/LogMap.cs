using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class LogMap : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> entity)
        {
            entity.ToTable("proxy_logs").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("log_id");
            entity.Property(x => x.DateTime).HasColumnName("log_datetime");
            entity.Property(x => x.Status).HasColumnName("log_status");

            entity.Property(x => x.BackendId).HasColumnName("log_backend_id");
            entity.Property(x => x.FrontendId).HasColumnName("log_frontend_id");
            entity.Property(x => x.SubDomainId).HasColumnName("log_subdomain_id");
            entity.Property(x => x.SslOrderId).HasColumnName("log_ssl_order_id");
            entity.Property(x => x.SslAuthorizationId).HasColumnName("log_ssl_auth_id");
            entity.Property(x => x.SslWildcardId).HasColumnName("log_ssl_wildcard_id");
            entity.Property(x => x.AddressId).HasColumnName("log_address_id");

            entity.Property(x => x.Message).HasColumnName("log_message");
            entity.Property(x => x.Subject).HasColumnName("log_subject");
        }
    }
}