using System;
using Cre8ion.Database;

namespace Shared.DatabaseEntities
{
    public class SslOrderData : IDatabaseEntity
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public EntityStatus Status { get; set; }

        public int SslOrderId { get; set; }

        public string PrivateKey { get; set; }
        public string Certificate { get; set; }
        public byte[] PfxBundle { get; set; }
    }
}