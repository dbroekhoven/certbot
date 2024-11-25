using MediatR;

namespace Application.Features.Proxy
{
    public class RemoveOldRecordsCommand : IRequest<bool>
    {
        public RemoveOldRecordsCommand()
        {
        }
    }
}