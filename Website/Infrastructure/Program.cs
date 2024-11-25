using Cre8ion.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Website.Infrastructure
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .Use<Startup>()
                .Build()
                .Run();
        }
    }
}
