using Application.Features;
using CommandLine;
using Cre8ion.Features.Synchronization;

namespace Application.Infrastructure
{
    public class CommandLineOptions : ICommandLineOptions
    {
        [Option(nameof(Scheduled))]
        public bool Scheduled { get; set; }

        [Option(nameof(Full))]
        [Synchronizer(1, typeof(FullSynchronizer))]
        public bool Full { get; set; }

        [Option(nameof(Manual))]
        [Synchronizer(2, typeof(ManualSynchronizer))]
        public bool Manual { get; set; }

        [Option(nameof(Firewall))]
        [Synchronizer(3, typeof(FirewallSynchronizer))]
        public bool Firewall { get; set; }

        [Option(nameof(Whitelist))]
        [Synchronizer(7, typeof(WhitelistSynchronizer))]
        public bool Whitelist { get; set; }

        [Option(nameof(Cleanup))]
        [Synchronizer(4, typeof(CleanupSynchronizer))]
        public bool Cleanup { get; set; }

        [Option(nameof(FortiWebLogs))]
        [Synchronizer(5, typeof(FortiWebLogSynchronizer))]
        public bool FortiWebLogs { get; set; }

        [Option(nameof(Cdn))]
        [Synchronizer(5, typeof(CdnSynchronizer))]
        public bool Cdn { get; set; }

        [Option(nameof(Orangebag))]
        public bool Orangebag { get; set; }

        [Option(nameof(PrivateKeys))]
        [Synchronizer(6, typeof(PrivateKeySynchronizer))]
        public bool PrivateKeys { get; set; }

        [Option(nameof(From))]
        public string From { get; set; }

        [Option(nameof(Till))]
        public string Till { get; set; }
    }
}