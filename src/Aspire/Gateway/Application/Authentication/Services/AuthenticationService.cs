using System.Security.Cryptography;
using System.Text;
using Domain.Repositories.Interfaces;
using Gateway.Application.Authentication.Services.Interfaces;

namespace Gateway.Application.Authentication.Services;

public class AuthenticationService(
    IApplicationRepository repository) : IAuthenticationService
{
    public async Task<bool> AuthorizeApplication(string application, string password)
    {
        var app = await repository.GetApplicationDocumentAsync(application);

        return app is not null 
            && ComputeSha256Hash(password, app.Salt)
                .Equals(app.Password);
    }

    private static string ComputeSha256Hash(string password, string salt)
    {
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hashBytes = SHA256.HashData(combined);
        return Convert.ToHexString(hashBytes);
    }
}
