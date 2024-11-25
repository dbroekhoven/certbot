namespace Shared
{
    public static class Settings
    {
        public const string LetsEncryptEmailAddress = "colin@cre8ion.com";

        public const int LetsEncryptMonitorDays = -39;
        public const int LetsEncryptRenewalDays = -29;
        public const int LetsEncryptExpireDays = -7;
        public const int LetsEncryptChallengeRetries = 10;

        public const string FortiWebUrl = "https://fortiweb.intern.cre8ion.nl";
        public const string FortiWebToken = "eyJ1c2VybmFtZSI6ImFkbWluIiwicGFzc3dvcmQiOiJUYypsI1JlbW90ZSEwMSIsInZkb20iOiJyb290In0=";

        public const string FortiWebServerPoolPrefix = "sp";
        public const string FortiWebContentRoutingPrefix = "cr";
        public const string FortiWebRewriteRulePrefix = "redirect";
        public const string FortiWebServerPolicyPrefix = "pol-http";
        public const string FortiWebVirtualServerPrefix = "vs-http";
        public const string FortiWebServerNameIndicationPrefix = "sni-http";
        public const string FortiWebVirtualAddressPrefix = "vip";

        public const string SolimasIPv4Prefix = "91.210.127.";
        public const string SolimasIPv6Prefix = "2a02:ac40:0:1:";

        public const string FortiGateBlockAddressPrefix = "block";
        public const string FortiGateFtpAddressPrefix = "ftp";
    }
}
