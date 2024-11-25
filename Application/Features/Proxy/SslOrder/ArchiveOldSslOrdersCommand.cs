using MediatR;

namespace Application.Features.Proxy
{
    public class ArchiveOldSslOrdersCommand : IRequest<bool>
    {
        public ArchiveOldSslOrdersCommand()
        {
        }
    }
}