using System.Text.RegularExpressions;

namespace Shared.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidDomain(this string name)
        {
            var domainName = name.Replace("*.", string.Empty);

            return Regex.IsMatch(domainName, @"
                # Rev:2013-03-26
                # Match DNS host domain having one or more subdomains.
                # Top level domain subset taken from IANA.ORG. See:
                # http://data.iana.org/TLD/tlds-alpha-by-domain.txt
                ^                  # Anchor to start of string.
                (?!.{256})         # Whole domain must be 255 or less.
                (?:                # Group for one or more sub-domains.
                  [a-z0-9]         # Either subdomain length from 2-63.
                  [a-z0-9-]{0,61}  # Middle part may have dashes.
                  [a-z0-9]         # Starts and ends with alphanum.
                  \.               # Dot separates subdomains.
                | [a-z0-9]         # or subdomain length == 1 char.
                  \.               # Dot separates subdomains.
                )+                 # One or more sub-domains.
                (?:                # Top level domain alternatives.
                  [a-z]{2}         # Either any 2 char country code,
                | AERO|ARPA|ASIA|BIZ|CAT|COM|COOP|EDU|  # or TLD
                  GOV|INFO|INT|JOBS|MIL|MOBI|MUSEUM|    # from list.
                  NAME|NET|ORG|POST|PRO|TEL|TRAVEL|XXX| # IANA.ORG
                  DEV|SHOP|SHOES
                )                  # End group of TLD alternatives.
                $                  # Anchor to end of string.",
                RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }
    }
}