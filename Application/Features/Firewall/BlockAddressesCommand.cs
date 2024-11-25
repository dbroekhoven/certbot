using MediatR;

namespace Application.Features.Firewall
{
    public class BlockAddressesCommand : IRequest<bool>
    {
        public BlockAddressesCommand()
        {
        }
    }
}