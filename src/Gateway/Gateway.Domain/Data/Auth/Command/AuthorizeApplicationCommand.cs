using MiddleR.Abstractions;

namespace Gateway.Domain.Data.Auth.Command;

public record AuthorizeApplicationCommand(string Application, string Password) : IRequest<string?>;