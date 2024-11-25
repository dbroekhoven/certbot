using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class FortiWebLogMap : IEntityTypeConfiguration<FortiWebLog>
    {
        public void Configure(EntityTypeBuilder<FortiWebLog> entity)
        {
            entity.ToTable("fortiweb_logs").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("log_id");
            entity.Property(x => x.Date).HasColumnName("log_date");
            entity.Property(x => x.DateTime).HasColumnName("log_datetime");
            entity.Property(x => x.Status).HasColumnName("log_status");

            entity.Property(x => x.Policy).HasColumnName("log_policy");
            entity.Property(x => x.ServerPool).HasColumnName("log_server_pool");
            entity.Property(x => x.HttpPolicy).HasColumnName("log_http_policy");
            entity.Property(x => x.SignaturePolicy).HasColumnName("log_signature_policy");
            entity.Property(x => x.Country).HasColumnName("log_country");
            entity.Property(x => x.Destination).HasColumnName("log_destination");
            entity.Property(x => x.Source).HasColumnName("log_source");
            entity.Property(x => x.Host).HasColumnName("log_host");
            entity.Property(x => x.MainType).HasColumnName("log_attack_main_type");
            entity.Property(x => x.SubType).HasColumnName("log_attack_sub_type");
            entity.Property(x => x.Service).HasColumnName("log_service");
            entity.Property(x => x.Url).HasColumnName("log_url");
            entity.Property(x => x.UserAgent).HasColumnName("log_user_agent");
            entity.Property(x => x.Severity).HasColumnName("log_severity");
            entity.Property(x => x.Action).HasColumnName("log_action");
        }
    }
}