using System;
using Cre8ion.Database;

namespace Website.Features.Company
{
    public class Contact : IDatabaseEntity
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string FirstName { get; set; }
        public string Preposition { get; set; }
        public string LastName { get; set; }
        public string Salution { get; set; }
        public string Function { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }

        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }

        public string Name => this.FirstName + ' ' + (string.IsNullOrEmpty(this.Preposition) ? string.Empty : this.Preposition + " ") + this.LastName;
    }
}