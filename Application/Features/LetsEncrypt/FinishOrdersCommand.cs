using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.LetsEncrypt
{
    public class FinishOrdersCommand : IRequest<bool>
    {
        public SslOrder SslOrder { get; }

        public FinishOrdersCommand(SslOrder sslOrder)
        {
            this.SslOrder = sslOrder;
        }
    }
}