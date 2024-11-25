using MediatR;

namespace Application.Features.Account
{
    public class AccountQuery : IRequest<bool>
    {
        public AccountQuery()
        {
        }
    }
}