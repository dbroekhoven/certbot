using Cre8ion.Common.Extensions;
using Shared.DatabaseEntities;

namespace Shared.Extensions
{
    public static class FrontendExtensions
    {
        public static string GetUrlRewriteRuleName(this Frontend frontend)
        {
            var slug = frontend.Name
               .Replace(".", " ")
               .Slugify();

            return $"{Settings.FortiWebRewriteRulePrefix}-{slug}";
        }

        public static bool IsValidDomain(this Frontend frontend)
        {
            return frontend.Name.IsValidDomain();
        }
    }
}