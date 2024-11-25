using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Cdn
{
    public class CdnBindingsCommand : IRequest<bool>
    {
        public Frontend Frontend { get; }
        public SslOrder SslOrder { get; }

        public CdnBindingsCommand(Frontend frontend, SslOrder sslOrder)
        {
            this.Frontend = frontend;
            this.SslOrder = sslOrder;
        }
    }
}