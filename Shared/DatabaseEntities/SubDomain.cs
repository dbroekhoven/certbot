using System;
using Cre8ion.Database;

namespace Shared.DatabaseEntities
{
    public class SubDomain : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }

        public Frontend Frontend { get; set; }
        public int FrontendId { get; set; }

        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public bool HasContentRoutingRule { get; set; }
        public bool HasRewriteRule { get; set; }
        public bool ChangedRewriteRule { get; set; }
    }
}