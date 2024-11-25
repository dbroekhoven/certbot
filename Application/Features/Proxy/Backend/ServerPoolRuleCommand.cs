using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Proxy
{
    public class ServerPoolRuleCommand : IRequest<bool>
    {
        public Backend Backend { get; }

        public ServerPoolRuleCommand(Backend backend)
        {
            this.Backend = backend;
        }
    }
}