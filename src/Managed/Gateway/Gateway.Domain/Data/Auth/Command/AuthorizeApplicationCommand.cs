namespace Gateway.Domain.Data.Auth.Command;

public record AuthorizeApplicationCommand(string Application, string Password);