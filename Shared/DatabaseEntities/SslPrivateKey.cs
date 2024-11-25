using System;
using Cre8ion.Database;

namespace Shared.DatabaseEntities
{
    public class SslPrivateKey : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }

        public string Value { get; set; }
    }
}