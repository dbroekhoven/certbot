using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Cdn
{
    public class UploadCdnCertificateCommand : IRequest<bool>
    {
        public Frontend Frontend { get; }
        public SslOrder SslOrder { get; }

        public UploadCdnCertificateCommand(Frontend frontend, SslOrder sslOrder)
        {
            this.Frontend = frontend;
            this.SslOrder = sslOrder;
        }
    }
}