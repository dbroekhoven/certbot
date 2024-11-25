using System;
using Cre8ion.Database;

namespace Shared.DatabaseEntities
{
    public class SslAuthorization : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }

        public SslOrder SslOrder { get; set; }
        public int SslOrderId { get; set; }

        public string Domain { get; set; }
        public string Location { get; set; }

        public string Token { get; set; }
        public string Key { get; set; }

        public string Challenge { get; set; }
        public string Error { get; set; }

        public DateTime? Validated { get; set; }
        public DateTime? Finished { get; set; }
    }
}