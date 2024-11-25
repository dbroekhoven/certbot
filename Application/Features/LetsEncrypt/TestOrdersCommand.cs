using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.LetsEncrypt
{
    public class TestOrdersCommand : IRequest<bool>
    {
        public SslOrder SslOrder { get; }

        public TestOrdersCommand(SslOrder sslOrder)
        {
            this.SslOrder = sslOrder;
        }
    }
}