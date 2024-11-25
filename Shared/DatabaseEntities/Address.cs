using System;
using Cre8ion.Database;

namespace Shared.DatabaseEntities
{
    public class Address : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsFtp { get; set; }
        public bool HasAddress { get; set; }
    }
}