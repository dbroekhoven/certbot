using System;
using System.Collections.Generic;
using Cre8ion.Database;

namespace Shared.DatabaseEntities
{
    public class SslOrder : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }

        public Frontend Frontend { get; set; }
        public int FrontendId { get; set; }

        public string Location { get; set; }
        public string Error { get; set; }

        public DateTime? NotBefore { get; set; }
        public DateTime? NotAfter { get; set; }

        public DateTime? Finished { get; set; }

        public bool IsTested { get; set; }
        public bool IsValidated { get; set; }
        public bool IsFinished { get; set; }

        public bool HasLocalCertificate { get; set; }
        public bool HasServerNameIndication { get; set; }

        public string CaRootGroup { get; set; }
        public string ChaineNames { get; set; }

        public ISet<SslAuthorization> Authorizations { get; set; }
    }
}