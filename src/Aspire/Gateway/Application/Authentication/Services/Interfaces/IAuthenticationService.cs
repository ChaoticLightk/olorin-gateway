namespace Gateway.Application.Authentication.Services.Interfaces;

public interface IAuthenticationService
{
    Task<bool> BasicAuthorize(string application, string password);
    Task<string?> AuthorizeApplication(string application, string password);
}
