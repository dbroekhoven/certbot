using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseEntities
{
    public class AccountMap : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> entity)
        {
            entity.ToTable("proxy_ssl_accounts").HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("account_id");
            entity.Property(x => x.DateTime).HasColumnName("account_datetime");
            entity.Property(x => x.Status).HasColumnName("account_status");

            entity.Property(x => x.Email).HasColumnName("account_email");
            entity.Property(x => x.Key).HasColumnName("account_key");

            entity.HasQueryFilter(x => x.Status == Cre8ion.Database.EntityStatus.Published);
        }
    }
}