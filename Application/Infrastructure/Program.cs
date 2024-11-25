using System.Threading.Tasks;
using Application.Infrastructure;

namespace Application
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await Cre8ion.Features.ConsoleApplication.Startup
                .ServiceBuilder<Application, CommandLineOptions>(args)
                .RunApplicationAsync();
        }
    }
}