using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Certes;
using Certes.Acme;
using Cre8ion.Common;
using Cre8ion.Common.Extensions;
using Cre8ion.Database;
using Cre8ion.Database.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Shared.DatabaseEntities;
using Shared.Extensions;
using Shared.Models;

namespace Shared.Services
{
    public class LetsEncryptService : IService
    {
        private readonly DatabaseContext dbContext;
        private readonly DatabaseLogger logger;
        private readonly HttpService httpService;

        public LetsEncryptService(DatabaseContext dbContext, DatabaseLogger logger, HttpService httpService)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.httpService = httpService;
        }

        public async Task<SslOrder> NewOrderAsync(IAcmeContext acme, Frontend frontend)
        {
            var domainNames = frontend.SubDomains
                .Where(x => x.Status == EntityStatus.Published)
                .Select(x => x.Name)
                .ToList();

            if (!domainNames.Any())
            {
                this.logger.Warning(frontend, $"Missing domain names for {frontend.Name}.");

                return default;
            }

            var dbOrder = new SslOrder
            {
                FrontendId = frontend.Id,
                Frontend = frontend,
                Location = string.Empty,
                Status = Cre8ion.Database.EntityStatus.Backup,
                DateTime = DateTime.Now,
                Authorizations = new HashSet<SslAuthorization>()
            };

            this.dbContext
                .Add(dbOrder);

            await this.dbContext
                .SaveChangesAsync();

            this.logger.Information(frontend, $"Creating new order for domain {frontend.Name}.");

            IOrderContext order = null;

            try
            {
                order = await acme.NewOrder(domainNames);
            }
            catch (AcmeRequestException exception)
            {
                dbOrder.Error = exception.Message;
            }

            if (order == null)
            {
                this.dbContext
                    .Update(dbOrder);

                await this.dbContext
                    .SaveChangesAsync();

                this.logger.Error(dbOrder, $"Failed to create new order for domain {frontend.Name}.");
                this.logger.Debug(dbOrder, dbOrder.Error);

                return dbOrder;
            }

            this.logger.Information(dbOrder, $"Save new order for domain {frontend.Name}.");

            var authorizations = await order.Authorizations();

            foreach (var authorization in authorizations)
            {
                var httpChallenge = await authorization.Http();
                var resource = await authorization.Resource();
                var name = resource.Identifier.Value;

                var dbAuthorization = new SslAuthorization
                {
                    Domain = name,
                    Location = authorization.Location.ToString(),
                    Token = httpChallenge.Token,
                    Key = httpChallenge.KeyAuthz,
                    Challenge = "New",
                    Error = string.Empty,
                    Status = Cre8ion.Database.EntityStatus.Approval,
                    DateTime = DateTime.Now
                };

                dbOrder.Authorizations.Add(dbAuthorization);

                this.logger.Information(dbOrder, $"Save authorization for domain {name}.");
            }

            dbOrder.Location = order.Location.ToString();
            dbOrder.Status = Cre8ion.Database.EntityStatus.Approval;

            this.dbContext
                .Update(dbOrder);

            await this.dbContext
                .SaveChangesAsync();

            this.logger.Information(dbOrder, $"Successful created order for domain {frontend.Name}.");

            return dbOrder;
        }

        public async Task<bool> TestAsync(SslOrder dbOrder)
        {
            var passed = true;

            this.logger.Information(dbOrder, $"Starting tests for domain {dbOrder.Frontend.Name}.");

            foreach (var dbAuthorization in dbOrder.Authorizations)
            {
                this.logger.Information(dbOrder, dbAuthorization, $"Test dns {dbAuthorization.Domain}");

                var dnsResult = await this.GetIpAddressAsync(dbAuthorization.Domain);
                if (!dnsResult.Passed)
                {
                    passed = false;

                    if (string.IsNullOrEmpty(dnsResult.IpAddress))
                    {
                        this.logger.Warning(dbOrder, dbAuthorization, $"Test for domain {dbAuthorization.Domain} failed: {dnsResult.Error}");
                    }
                    else
                    {
                        this.logger.Warning(dbOrder, dbAuthorization, $"Test for domain {dbAuthorization.Domain} failed: {dnsResult.IpAddress}{(string.IsNullOrEmpty(dnsResult.Error) ? "" : $", {dnsResult.Error}")}");
                    }

                    continue;
                }

                this.logger.Information(dbOrder, dbAuthorization, $"Test for domain {dbAuthorization.Domain} passed: {dnsResult.IpAddress}");

                var url = $"http://{dbAuthorization.Domain}/.well-known/acme-challenge/{dbAuthorization.Token}";

                this.logger.Information(dbOrder, dbAuthorization, $"Test url {url}");

                var response = await this.httpService.GetAsync(url);

                if (response == dbAuthorization.Key)
                {
                    dbAuthorization.Error = string.Empty;
                    dbAuthorization.Challenge = "Passed";

                    this.logger.Information(dbOrder, dbAuthorization, $"Test for domain {dbAuthorization.Domain}: {dbAuthorization.Challenge}");
                }
                else
                {
                    passed = false;

                    dbAuthorization.Error = response;
                    dbAuthorization.Challenge = "Failed";

                    this.logger.Warning(dbOrder, dbAuthorization, $"Test for domain {dbAuthorization.Domain}: {dbAuthorization.Challenge}");
                    this.logger.Debug(dbOrder, dbAuthorization, $"Test response: {response}");
                }
            }

            this.logger.Information(dbOrder, $"Finishing tests for domain {dbOrder.Frontend.Name}.");

            this.dbContext
                .Update(dbOrder);

            await this.dbContext
                .SaveChangesAsync();

            return passed;
        }

        public async Task ValidateAsync(IAcmeContext acme, SslOrder dbOrder)
        {
            this.logger.Information(dbOrder, $"Starting validation for domain {dbOrder.Frontend.Name}.");

            foreach (var dbAuthorization in dbOrder.Authorizations)
            {
                var authorizationContext = acme.Authorization(new Uri(dbAuthorization.Location));
                var authorization = await authorizationContext.Resource();
                var httpChallengeContext = await authorizationContext.Http();

                var challengeResult = authorization.Status != Certes.Acme.Resource.AuthorizationStatus.Pending
                    ? await httpChallengeContext.Resource()
                    : await httpChallengeContext.ValidateWithRetryAsync();

                var passed = challengeResult.Status?.ToString() == "Valid";

                dbAuthorization.Error = passed ? string.Empty : challengeResult.Error?.Detail;
                dbAuthorization.Challenge = authorization.Status == Certes.Acme.Resource.AuthorizationStatus.Deactivated
                    ? authorization.Status?.ToString()
                    : challengeResult.Status?.ToString();
                dbAuthorization.Validated = DateTime.Now;

                if (passed)
                {
                    this.logger.Information(dbOrder, dbAuthorization, $"Challenge response for {dbAuthorization.Domain}: {dbAuthorization.Challenge}{(string.IsNullOrEmpty(dbAuthorization.Error) ? string.Empty : $"- {dbAuthorization.Error}")}");
                }
                else
                {
                    this.logger.Warning(dbOrder, dbAuthorization, $"Challenge response for {dbAuthorization.Domain}: {dbAuthorization.Challenge} - {dbAuthorization.Error}");
                }
            }

            if (dbOrder.Authorizations.All(x => x.Challenge == "Valid"))
            {
                dbOrder.IsValidated = true;
            }

            this.logger.Information(dbOrder, $"Finishing validation for domain {dbOrder.Frontend.Name}.");

            this.dbContext
                .Update(dbOrder);

            await this.dbContext
                .SaveChangesAsync();
        }

        public async Task FinishAsync(IAcmeContext acme, SslOrder dbOrder, SslOrderData dbOrderData)
        {
            this.logger.Information(dbOrder, $"Finishing domain {dbOrder.Frontend.Name}.");

            var order = acme.Order(new Uri(dbOrder.Location));

            var orderResource = await order.Resource();
            var orderStatus = orderResource.Status.Value;

            this.logger.Information(dbOrder, $"Order status {dbOrder.Frontend.Name}: {orderStatus}");

            var invalidStatus = new[] { Certes.Acme.Resource.OrderStatus.Invalid, Certes.Acme.Resource.OrderStatus.Pending, Certes.Acme.Resource.OrderStatus.Processing };
            if (invalidStatus.Contains(orderStatus))
            {
                return;
            }

            IKey privateKey = null;

            if (dbOrderData == null)
            {
                dbOrderData = new SslOrderData
                {
                    SslOrderId = dbOrder.Id,
                    DateTime = DateTime.Now,
                    Status = Cre8ion.Database.EntityStatus.Published
                };

                this.dbContext.Add(dbOrderData);

                await this.dbContext
                    .SaveChangesAsync();
            }

            if (string.IsNullOrEmpty(dbOrderData.PrivateKey))
            {
                dbOrderData.PrivateKey = await this.GetPrivateKeyAsync();

                if (string.IsNullOrEmpty(dbOrderData.PrivateKey))
                {
                    this.logger.Information(dbOrder, $"Generate private key {dbOrder.Frontend.Name} with keysize 4096");

                    privateKey = KeyFactory.NewKey(KeyAlgorithm.RS256, 4096);

                    this.logger.Information(dbOrder, $"Generate pem from private key {dbOrder.Frontend.Name}");

                    dbOrderData.PrivateKey = privateKey.ToPem();
                }
                else
                {
                    this.logger.Information(dbOrder, $"Got private key {dbOrder.Frontend.Name} with keysize 4096");
                }
            }

            if (privateKey == null)
            {
                this.logger.Information(dbOrder, $"Import pem from private key {dbOrder.Frontend.Name}");

                privateKey = KeyFactory.FromPem(dbOrderData.PrivateKey);
            }

            this.logger.Information(dbOrder, $"Download order for {dbOrder.Frontend.Name}");

            var certificate = orderStatus == Certes.Acme.Resource.OrderStatus.Valid
                ? await order.Download()
                : await order.Generate(new CsrInfo
                {
                    CountryName = "NL",
                    State = "Noord-Brabant",
                    Locality = "'s-Hertogenbosch",
                    Organization = "The Cre8ion.Lab",
                    OrganizationUnit = "Web",
                    CommonName = dbOrder.Frontend.Name,
                }, privateKey);

            this.logger.Information(dbOrder, $"Generate certificate bundle for {dbOrder.Frontend.Name}");

            var pfxBuilder = certificate.ToPfx(privateKey);
            pfxBuilder.FullChain = true;

            var pfxBundle = pfxBuilder.Build(dbOrder.Frontend.Name, dbOrder.Frontend.Name);

            this.logger.Information(dbOrder, $"Extract details from certificate for {dbOrder.Frontend.Name}");

            using (var x509 = new X509Certificate2(pfxBundle, dbOrder.Frontend.Name))
            {
                dbOrder.NotBefore = x509.NotBefore;
                dbOrder.NotAfter = x509.NotAfter;
            }

            var privateKeyPem = privateKey.ToPem();
            var certificatePem = certificate.ToPem();

            dbOrder.Status = Cre8ion.Database.EntityStatus.Published;
            dbOrderData.PrivateKey = privateKeyPem;
            dbOrderData.Certificate = certificatePem;
            dbOrderData.PfxBundle = pfxBundle;
            dbOrder.Finished = DateTime.Now;
            dbOrder.IsFinished = true;

            this.logger.Information(dbOrder, $"Extract chain from certificate for {dbOrder.Frontend.Name}");

            dbOrder.CaRootGroup = this.GetChaineName(pfxBundle, dbOrder.Frontend.Name);
            dbOrder.ChaineNames = this.GetChaineNames(pfxBundle, dbOrder.Frontend.Name);

            foreach (var dbAuthorization in dbOrder.Authorizations)
            {
                this.logger.Information(dbOrder, $"Update authorization {dbAuthorization.Domain}");

                dbAuthorization.Error = string.Empty;
                dbAuthorization.Challenge = "Ready";
                dbAuthorization.Finished = DateTime.Now;
                dbAuthorization.Status = Cre8ion.Database.EntityStatus.Published;

                this.logger.Information(dbOrder, $"Finish response for {dbAuthorization.Domain}: {dbAuthorization.Challenge}{(string.IsNullOrEmpty(dbAuthorization.Error) ? string.Empty : $"- {dbAuthorization.Error}")}");
            }

            this.logger.Information(dbOrder, $"Certificate ready for {dbOrder.Frontend.Name}.");

            this.dbContext
                .Update(dbOrder);

            this.dbContext
                .Update(dbOrderData);

            await this.dbContext
                .SaveChangesAsync();

            return;
        }

        public async Task<string> GetKeyByTokenAsync(string token)
        {
            return await this.dbContext.Set<SslAuthorization>()
                .Where(x => x.Token == token)
                .Select(x => x.Key)
                .FirstOrDefaultAsync();
        }

        public string GetChaineName(byte[] pfxBundle, string password)
        {
            var certificateCollection = new X509Certificate2Collection();
            certificateCollection.Import(pfxBundle, password);

            if (certificateCollection.Count == 1)
            {
                return password.StartsWith("*.")
                    ? "ca-digicert-global-root-g2"
                    : "ca-isrg-root-x1";
            }

            var chainName = string.Empty;

            foreach (var certificateFile in certificateCollection)
            {
                if (!certificateFile.HasPrivateKey)
                {
                    chainName = certificateFile.GetNameInfo(X509NameType.SimpleName, false);
                }
            }

            return $"ca-{chainName.Slugify()}";
        }

        public string GetChaineNames(byte[] pfxBundle, string password)
        {
            var certificateCollection = new X509Certificate2Collection();
            certificateCollection.Import(pfxBundle, password);

            var chainNames = new List<string>();

            foreach (var certificateFile in certificateCollection)
            {
                var chainName = certificateFile.GetNameInfo(X509NameType.SimpleName, false);

                chainNames.Add(chainName);
            }

            return string.Join(", ", chainNames);
        }

        private async Task<DnsResult> GetIpAddressAsync(string hostName)
        {
            var passed = true;
            var address = string.Empty;
            var error = string.Empty;

            var allowedIPv4 = Enumerable.Range(0, 5)
                .Select(x => $"{Settings.SolimasIPv4Prefix}{(58 + x)}")
                .ToList();

            allowedIPv4.Add($"{Settings.SolimasIPv4Prefix}43");

            var allowedIPv6Prefix = $"{Settings.SolimasIPv6Prefix}0:fe08:";

            try
            {
                var host = await Dns.GetHostEntryAsync(hostName);

                foreach (var entry in host.AddressList)
                {
                    var result = entry.AddressFamily == AddressFamily.InterNetworkV6
                        ? entry.ToString().StartsWith(allowedIPv6Prefix)
                        : allowedIPv4.Contains(entry.ToString());

                    if (!result)
                    {
                        passed = false;
                    }
                }

                address = string.Join(", ", host.AddressList.Select(x => x.ToString()));
            }
            catch (SocketException exception)
            {
                passed = false;
                error = exception.Message;
            }

            return new DnsResult
            {
                IpAddress = address,
                Passed = passed,
                Error = error
            };
        }

        public async Task<string> GetPrivateKeyAsync()
        {
            var key = await this.dbContext.Set<SslPrivateKey>()
                .Where(x => x.Status == Cre8ion.Database.EntityStatus.Published)
                .FirstOrDefaultAsync();

            if (key == null)
            {
                return String.Empty;
            }

            var value = key.Value;

            key.Status = Cre8ion.Database.EntityStatus.Archive;

            return value;
        }
    }
}