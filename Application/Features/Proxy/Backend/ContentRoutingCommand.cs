using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Proxy
{
    public class ContentRoutingCommand : IRequest<bool>
    {
        public Backend Backend { get; }

        public ContentRoutingCommand(Backend backend)
        {
            this.Backend = backend;
        }
    }
}