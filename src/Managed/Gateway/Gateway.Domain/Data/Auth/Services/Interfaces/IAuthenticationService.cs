namespace Gateway.Domain.Data.Auth.Services.Interfaces;

public interface IAuthService
{
    Task<string?> AuthorizeApplication(string application, string password);
    Task<bool> BasicAuthorize(string application, string password);
}
