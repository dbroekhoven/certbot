using System;
using Cre8ion.Database;

namespace Shared.DatabaseEntities
{
    public class Log : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }

        public int BackendId { get; set; }
        public int FrontendId { get; set; }
        public int SubDomainId { get; set; }
        public int SslOrderId { get; set; }
        public int SslAuthorizationId { get; set; }
        public int SslWildcardId { get; set; }
        public int AddressId { get; set; }

        public string Message { get; set; }
        public string Subject { get; set; }
    }
}