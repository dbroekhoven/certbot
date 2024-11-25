using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Proxy
{
    public class VirtualAddressGroupCommand : IRequest<bool>
    {
        public Backend Backend { get; }

        public VirtualAddressGroupCommand(Backend backend)
        {
            this.Backend = backend;
        }
    }
}