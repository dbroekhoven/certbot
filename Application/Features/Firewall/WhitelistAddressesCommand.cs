using MediatR;

namespace Application.Features.Firewall
{
    public class WhitelistAddressesCommand : IRequest<bool>
    {
        public WhitelistAddressesCommand()
        {
        }
    }
}