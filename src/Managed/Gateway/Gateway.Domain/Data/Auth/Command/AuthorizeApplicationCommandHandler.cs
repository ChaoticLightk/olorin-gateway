using Gateway.Domain.Data.Auth.Services.Interfaces;

namespace Gateway.Domain.Data.Auth.Command;

public class AuthorizeApplicationCommandHandler(IAuthService auth) 
{
    public Task<string?> Handle(AuthorizeApplicationCommand request, CancellationToken cancellationToken = default)
    {
        return auth.AuthorizeApplication(request.Application, request.Password);
    }
}
