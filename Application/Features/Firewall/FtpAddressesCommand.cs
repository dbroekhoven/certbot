using MediatR;

namespace Application.Features.Firewall
{
    public class FtpAddressesCommand : IRequest<bool>
    {
        public FtpAddressesCommand()
        {
        }
    }
}