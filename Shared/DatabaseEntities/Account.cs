using System;
using Cre8ion.Database;

namespace Shared.DatabaseEntities
{
    public class Account : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }

        public string Email { get; set; }
        public string Key { get; set; }
    }
}