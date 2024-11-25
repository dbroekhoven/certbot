using Cre8ion.Common.Extensions;
using Shared.DatabaseEntities;

namespace Shared.Extensions
{
    public static class SubDomainExtensions
    {
        public static string GetUrlRewriteRuleName(this SubDomain subDomain)
        {
            var slug = subDomain.Name
               .Replace(".", " ")
               .Slugify();

            return $"{Settings.FortiWebRewriteRulePrefix}-{slug}";
        }
    }
}