using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Proxy
{
    public class UploadCertificateCommand : IRequest<bool>
    {
        public Frontend Frontend { get; }
        public SslOrder SslOrder { get; }

        public UploadCertificateCommand(Frontend frontend, SslOrder sslOrder)
        {
            this.Frontend = frontend;
            this.SslOrder = sslOrder;
        }
    }
}