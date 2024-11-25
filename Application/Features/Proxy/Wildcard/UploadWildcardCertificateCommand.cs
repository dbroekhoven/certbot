using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Proxy
{
    public class UploadWildcardCertificateCommand : IRequest<bool>
    {
        public SslWildcard SslWildcard { get; }

        public UploadWildcardCertificateCommand(SslWildcard sslWildcard)
        {
            this.SslWildcard = sslWildcard;
        }
    }
}