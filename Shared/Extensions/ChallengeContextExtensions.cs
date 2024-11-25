using System.Threading;
using System.Threading.Tasks;
using Certes.Acme;
using Certes.Acme.Resource;

namespace Shared.Extensions
{
    public static class ChallengeContextExtensions
    {
        public static async Task<Challenge> ValidateWithRetryAsync(this IChallengeContext httpChallenge)
        {
            var result = await httpChallenge.Validate();

            var attempts = Settings.LetsEncryptChallengeRetries;
            while (attempts > 0 && result.Status == ChallengeStatus.Pending || result.Status == ChallengeStatus.Processing)
            {
                Thread.Sleep(1000);

                attempts--;

                result = await httpChallenge.Resource();
            }

            return result;
        }
    }
}