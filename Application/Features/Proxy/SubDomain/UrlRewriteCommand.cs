using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Proxy
{
    public class UrlRewriteCommand : IRequest<bool>
    {
        public SubDomain SubDomain { get; }

        public UrlRewriteCommand(SubDomain subDomain)
        {
            this.SubDomain = subDomain;
        }
    }
}