using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Proxy
{
    public class ServerNameIndicationCommand : IRequest<bool>
    {
        public Frontend Frontend { get; }
        public SslOrder SslOrder { get; }
        public SubDomain SubDomain { get; }

        public ServerNameIndicationCommand(Frontend frontend, SslOrder sslOrder, SubDomain subDomain)
        {
            this.Frontend = frontend;
            this.SslOrder = sslOrder;
            this.SubDomain = subDomain;
        }

        public ServerNameIndicationCommand(Frontend frontend, SslOrder sslOrder)
        {
            this.Frontend = frontend;
            this.SslOrder = sslOrder;
        }
    }
}