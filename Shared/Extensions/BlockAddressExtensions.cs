using Cre8ion.Common.Extensions;
using Shared.DatabaseEntities;

namespace Shared.Extensions
{
    public static class BlockAddressExtensions
    {
        public static string GetBlockName(this Address address)
        {
            var slug = address.Name
               .Replace(".", " ")
               .Slugify();

            return $"{Settings.FortiGateBlockAddressPrefix}-{slug}";
        }

        public static string GetFtpName(this Address address)
        {
            var slug = address.Name
               .Replace(".", " ")
               .Slugify();

            return $"{Settings.FortiGateFtpAddressPrefix}-{slug}";
        }
    }
}