using System;
using System.Collections.Generic;
using Cre8ion.Database;
using Website.Features.Company;

namespace Shared.DatabaseEntities
{
    public class Backend : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string InternalAddress { get; set; }
        public string ServerPool { get; set; }
        public bool ChangedServerPool { get; set; }
        public bool HasServerPool { get; set; }
        public bool HasServerPoolRule { get; set; }
        public bool ChangedServerPoolRule { get; set; }
        public bool AllInOneCertificate { get; set; }

        public string ExternalAddress { get; set; }
        public string ProtectionProfile { get; set; }
        public bool HasContentRouting { get; set; }
        public bool HasServerPolicyRule { get; set; }
        public bool ChangedServerPolicyRule { get; set; }
        public bool UseIPv6 { get; set; }
        public bool HasIPv6 { get; set; }

        public bool IsCdn { get; set; }

        public bool IsRedirect { get; set; }
        public string RedirectUrl { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public ISet<Frontend> Frontends { get; set; }
    }
}