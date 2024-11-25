using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.Proxy
{
    public class WildcardServerNameIndicationCommand : IRequest<bool>
    {
        public Frontend Frontend { get; }

        public WildcardServerNameIndicationCommand(Frontend frontend)
        {
            this.Frontend = frontend;
        }
    }
}