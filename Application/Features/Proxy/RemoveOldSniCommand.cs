using MediatR;

namespace Application.Features.Proxy
{
    public class RemoveOldSniCommand : IRequest<bool>
    {
        public RemoveOldSniCommand()
        {
        }
    }
}