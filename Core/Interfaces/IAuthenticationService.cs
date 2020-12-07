using Common.Dto;

namespace Core.Interfaces
{
    public interface IAuthenticationService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest req);
    }
}
