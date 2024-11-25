using MediatR;

namespace Application.Features.Proxy
{
    public class RemoveUnusedCertificatesCommand : IRequest<bool>
    {
        public RemoveUnusedCertificatesCommand()
        {
        }
    }
}