namespace Gateway.Application.Authentication.Services.Interfaces;

public interface IAuthenticationService
{
    Task<bool> AuthorizeApplication(string application, string password); 
}
