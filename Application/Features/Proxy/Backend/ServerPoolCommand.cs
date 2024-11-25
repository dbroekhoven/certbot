using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Proxy
{
    public class ServerPoolCommand : IRequest<bool>
    {
        public Backend Backend { get; }

        public ServerPoolCommand(Backend backend)
        {
            this.Backend = backend;
        }
    }
}