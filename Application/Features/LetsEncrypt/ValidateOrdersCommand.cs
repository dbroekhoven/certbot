using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.LetsEncrypt
{
    public class ValidateOrdersCommand : IRequest<bool>
    {
        public SslOrder SslOrder { get; }

        public ValidateOrdersCommand(SslOrder sslOrder)
        {
            this.SslOrder = sslOrder;
        }
    }
}