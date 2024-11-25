using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Proxy
{
    public class ServerPolicyRuleCommand : IRequest<bool>
    {
        public Backend Backend { get; }

        public ServerPolicyRuleCommand(Backend backend)
        {
            this.Backend = backend;
        }
    }
}