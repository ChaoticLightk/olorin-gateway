namespace Gateway.Configuration.Jwt;

public class JWTSettings
{
    public static readonly string SECTION_NAME = "JWT";

    public string Issuer { get; set; } = default!; 
    public string Key { get; set; } = default!; 
}
