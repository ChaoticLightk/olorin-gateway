namespace Gateway.Application.Authentication.Services.Interfaces;

public interface IAuthenticationService
{
    Task<string?> AuthorizeApplication(string application, string password);
}
