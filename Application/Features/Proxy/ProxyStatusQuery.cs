using MediatR;

namespace Application.Features.Proxy
{
    public class ProxyStatusQuery : IRequest<bool>
    {
        public ProxyStatusQuery()
        {
        }
    }
}