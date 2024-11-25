using MediatR;
using Shared.DatabaseEntities;

namespace Application.Features.LetsEncrypt
{
    public class NewOrdersCommand : IRequest<bool>
    {
        public Frontend Frontend { get; }

        public NewOrdersCommand(Frontend frontend)
        {
            this.Frontend = frontend;
        }
    }
}