using Cre8ion.Common.Extensions;
using Shared.DatabaseEntities;

namespace Shared.Extensions
{
    public static class SslWildcardExtensions
    {
        public static bool IsValidDomain(this SslWildcard sslWildcard)
        {
            return sslWildcard.Name.IsValidDomain();
        }

        public static string GetCertificateName(this SslWildcard sslWildcard)
        {
            var slug = sslWildcard.Name
                .Replace(".", " ")
                .Replace("*", "wildcard")
                .Slugify();

            return $"{slug}-valid-{sslWildcard.NotBefore.Value:dd-MM-yyyy}-till-{sslWildcard.NotAfter.Value:dd-MM-yyyy}";
        }
    }
}