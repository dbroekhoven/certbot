using System;
using Cre8ion.Database;

namespace Shared.DatabaseEntities
{
    public class WhitelistAddress : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }
        public bool IsDeleted { get; set; }
        public bool HasAddress { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string Value { get; set; }
    }
}