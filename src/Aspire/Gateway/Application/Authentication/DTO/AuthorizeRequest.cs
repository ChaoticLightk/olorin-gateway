using System;

namespace Gateway.Application.Authentication.DTO;

public record AuthorizeRequest(string Application, string Password);