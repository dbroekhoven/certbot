using System;
using System.Collections.Generic;
using Cre8ion.Database;
using Website.Features.Company;

namespace Shared.DatabaseEntities
{
    public class SslWildcard : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }

        public string Name { get; set; }
        public byte[] PfxBundle { get; set; }

        public DateTime? NotBefore { get; set; }
        public DateTime? NotAfter { get; set; }

        public bool HasLocalCertificate { get; set; }
        public bool ChangedLocalCertificate { get; set; }
        public string CaRootGroup { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public ISet<Frontend> Frontends { get; set; }
    }
}