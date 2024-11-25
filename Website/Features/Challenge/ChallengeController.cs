using System.Threading.Tasks;
using Cre8ion.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;

namespace Website.Features.Challenge
{
    public class ChallengeController : BaseController
    {
        private readonly LetsEncryptService letsEncryptService;

        public ChallengeController(LetsEncryptService letsEncryptService)
        {
            this.letsEncryptService = letsEncryptService;
        }

        [HttpGet(".well-known/acme-challenge/{token}")]
        public async Task<IActionResult> WellKnown(string token)
        {
            var key = await this.letsEncryptService.GetKeyByTokenAsync(token);

            if (string.IsNullOrEmpty(key))
            {
                return this.NotFound();
            }

            return this.Content(key, "text/plain");
        }

        [HttpGet(".well-known/acme-challenge/configcheck")]
        public IActionResult ConfigCheck()
        {
            return this.Content("Extensionless File Config Test - OK", "text/plain");
        }
    }
}