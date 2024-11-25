using MediatR;

namespace Application.Features.Proxy
{
    public class GenerateNewPrivateKeysCommand : IRequest<bool>
    {
        public GenerateNewPrivateKeysCommand()
        {
        }
    }
}