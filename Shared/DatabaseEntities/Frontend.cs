using System;
using System.Collections.Generic;
using Cre8ion.Database;

namespace Shared.DatabaseEntities
{
    public class Frontend : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public bool UseLetsEncrypt { get; set; }
        public bool UseWildcard { get; set; }
        public bool HasWildcard { get; set; }
        public bool ChangedWildcard { get; set; }
        public bool Reissue { get; set; }
        public int ReissueCount { get; set; }

        public Backend Backend { get; set; }
        public int BackendId { get; set; }

        public SslWildcard SslWildcard { get; set; }
        public int SslWildcardId { get; set; }

        public ISet<SubDomain> SubDomains { get; set; }
        public ISet<SslOrder> SslOrders { get; set; }
    }
}