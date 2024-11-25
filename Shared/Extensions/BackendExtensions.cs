using Cre8ion.Common.Extensions;
using Shared.DatabaseEntities;

namespace Shared.Extensions
{
    public static class BackendExtensions
    {
        public static string GetObsoleteServerPoolName(this Backend backend)
        {
            if (backend.IsCdn)
            {
                return string.Empty;
            }

            if (backend.IsRedirect)
            {
                var defaultName = "default-cre8ion-nl";

                return $"{Settings.FortiWebServerPoolPrefix}-{defaultName}";
            }

            var nameSlug = backend.Name
                .Replace(".", " ")
                .Slugify();

            return $"{Settings.FortiWebServerPoolPrefix}-{nameSlug}";
        }

        public static string GetServerPoolName(this Backend backend)
        {
            if (backend.IsCdn)
            {
                return string.Empty;
            }

            if (backend.IsRedirect)
            {
                var defaultName = "default-cre8ion-nl";

                return $"{Settings.FortiWebServerPoolPrefix}-{defaultName}";
            }

            if (string.IsNullOrEmpty(backend.ServerPool))
            {
                var nameSlug = backend.Name
                   .Replace(".", " ")
                   .Slugify();

                return $"{Settings.FortiWebServerPoolPrefix}-{nameSlug}";
            }

            if (backend.ServerPool.Contains(", "))
            {
                var nameSlug = backend.Name
                   .Replace(".", " ")
                   .Slugify();

                return $"{Settings.FortiWebServerPoolPrefix}-{nameSlug}";
            }

            var poolSlug = backend.ServerPool
               .Replace(".", " ")
               .Slugify();

            return $"{Settings.FortiWebServerPoolPrefix}-{poolSlug}";
        }

        public static string GetContentRoutingName(this Backend backend)
        {
            var slug = backend.Name
               .Replace(".", " ")
               .Slugify();

            return $"{Settings.FortiWebContentRoutingPrefix}-{slug}";
        }

        public static bool IsValidDomain(this Backend backend)
        {
            return backend.Name.IsValidDomain();
        }

        public static bool IsLoadBalanced(this Backend backend)
        {
            return backend.ServerPool.Contains(", ");
        }

        public static string GetServerPolicyName(this Backend backend)
        {
            return $"{Settings.FortiWebServerPolicyPrefix}-{backend.ExternalAddress[^2..]}";
        }

        public static string GetVirtualServerName(this Backend backend)
        {
            return $"{Settings.FortiWebVirtualServerPrefix}-{backend.ExternalAddress[^2..]}";
        }

        public static string GetServerNameIndication(this Backend backend)
        {
            return $"{Settings.FortiWebServerNameIndicationPrefix}-{backend.ExternalAddress[^2..]}";
        }

        public static string GetIPv6Address(this Backend backend)
        {
            var x = backend.GetNumbers();

            var address = $"{Settings.SolimasIPv6Prefix}0:fe08:c{x.networkNumber.PadLeft(3, '0')}:b{x.backendNumber.PadLeft(3, '0')}";

            return address;
        }

        public static string GetIPv6VipName(this Backend backend)
        {
            var x = backend.GetNumbers();

            var name = $"{Settings.FortiWebVirtualAddressPrefix}-fe08-c{x.networkNumber.PadLeft(3, '0')}-b{x.backendNumber.PadLeft(3, '0')}";

            return name;
        }

        private static (string networkNumber, string backendNumber) GetNumbers(this Backend backend)
        {

            string networkNumber;
            string backendNumber;

            if (string.IsNullOrEmpty(backend.InternalAddress) || backend.IsLoadBalanced())
            {
                if (string.IsNullOrEmpty(backend.ServerPool))
                {
                    networkNumber = "101";
                    backendNumber = backend.Id.ToString();
                }
                else
                {
                    networkNumber = "204";
                    backendNumber = backend.Id.ToString();
                }
            }
            else
            {
                networkNumber = backend.InternalAddress.Split('.')[2];
                backendNumber = backend.InternalAddress.Split('.')[3];
            }

            return (networkNumber, backendNumber);
        }
    }
}