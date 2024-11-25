using Cre8ion.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Website.Features.Homepage
{
    public class HomepageController : BaseController
    {
        public IActionResult Index()
        {
            return this.Content("Hello world!", "text/plain");
        }
    }
}