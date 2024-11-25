using System;
using Cre8ion.Database;

namespace Shared.DatabaseEntities
{
    public class FortiWebLog : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }

        public string Policy { get; set; }
        public string ServerPool { get; set; }
        public string HttpPolicy { get; set; }
        public string SignaturePolicy { get; set; }
        public string Service { get; set; }
        public string Host { get; set; }
        public string Country { get; set; }
        public string Url { get; set; }
        public string UserAgent { get; set; }

        public string MainType { get; set; }
        public string SubType { get; set; }

        public string Source { get; set; }
        public string Destination { get; set; }

        public string Severity { get; set; }
        public string Action { get; set; }
    }
}