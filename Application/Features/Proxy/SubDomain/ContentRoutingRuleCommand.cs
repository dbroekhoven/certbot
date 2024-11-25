using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Proxy
{
    public class ContentRoutingRuleCommand : IRequest<bool>
    {
        public SubDomain SubDomain { get; }

        public ContentRoutingRuleCommand(SubDomain subDomain)
        {
            this.SubDomain = subDomain;
        }
    }
}