using Gateway.Domain.Data.Auth.Services.Interfaces;
using MiddleR.Abstractions;

namespace Gateway.Domain.Data.Auth.Command;

public class AuthorizeApplicationCommandHandler(IAuthService auth) 
    : IRequestHandler<AuthorizeApplicationCommand, string?>
{
    public Task<string?> Handle(AuthorizeApplicationCommand request, CancellationToken cancellationToken = default)
    {
        return auth.AuthorizeApplication(request.Application, request.Password);
    }
}
