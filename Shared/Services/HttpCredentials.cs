namespace Shared.Services
{
    public class HttpCredentials
    {
        public string Authorization { get; set; }
        public string AccessToken { get; set; }

        public static HttpCredentials WithAuthorization(string value)
        {
            return new HttpCredentials
            {
                Authorization = value
            };
        }

        public static HttpCredentials WithAccessToken(string value)
        {
            return new HttpCredentials
            {
                AccessToken = value
            };
        }
    }
}