using Cre8ion.Common.Extensions;
using Shared.DatabaseEntities;

namespace Shared.Extensions
{
    public static class SslOrderExtensions
    {
        public static string GetCertificateName(this SslOrder sslOrder, Frontend frontend)
        {
            var slug = frontend.Name
                .Replace(".", " ")
                .Replace("*", "wildcard")
                .Slugify();

            var reissueSuffix = string.Empty;

            if (frontend.ReissueCount > 0)
            {
                reissueSuffix = $"-r{frontend.ReissueCount}";
            }

            return $"{slug}-valid-{sslOrder.NotBefore.Value:dd-MM-yyyy}-till-{sslOrder.NotAfter.Value:dd-MM-yyyy}{reissueSuffix}";
        }
    }
}