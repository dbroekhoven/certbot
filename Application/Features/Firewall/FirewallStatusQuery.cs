using MediatR;

namespace Application.Features.Firewall
{
    public class FirewallStatusQuery : IRequest<bool>
    {
        public FirewallStatusQuery()
        {
        }
    }
}