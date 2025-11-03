✅ **YARP** como gateway
✅ **JWT + Basic Auth** funcionando lado a lado
✅ **MongoDB** como fonte dinâmica de rotas **e** authorization policies
✅ **Cache em memória** para performance

---

## Estrutura do projeto

```
YarpGateway/
├── Program.cs
├── Models/
│   ├── RouteDefinition.cs
│   └── AuthorizationPolicyModel.cs
├── Providers/
│   ├── MongoPolicyProvider.cs
│   └── MongoYarpConfigProvider.cs
├── Auth/
│   └── BasicAuthenticationHandler.cs
└── appsettings.json
```

---

## ⚙️ 1. appsettings.json (configuração mínima)

```json
{
  "Jwt": {
    "SecretKey": "super-secret-key-12345",
    "Issuer": "yarp-gateway",
    "Audience": "clients"
  },
  "Mongo": {
    "ConnectionString": "mongodb://localhost:27017",
    "Database": "YarpGatewayDb"
  }
}
```

---

## 📄 2. Models/AuthorizationPolicyModel.cs

```csharp
namespace YarpGateway.Models;

public class AuthorizationPolicyModel
{
    public string Id { get; set; } = default!;
    public string AuthenticationScheme { get; set; } = default!;
    public bool RequireAuthenticatedUser { get; set; } = true;
}
```

---

## 📄 3. Models/RouteDefinition.cs

```csharp
namespace YarpGateway.Models;

public class RouteDefinition
{
    public string RouteId { get; set; } = default!;
    public string Path { get; set; } = default!;
    public string ClusterAddress { get; set; } = default!;
    public string AuthorizationPolicy { get; set; } = default!;
}
```

---

## 🔐 4. Auth/BasicAuthenticationHandler.cs

```csharp
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace YarpGateway.Auth;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (authHeader.Scheme != "Basic")
                return Task.FromResult(AuthenticateResult.Fail("Invalid Scheme"));

            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter!)).Split(':', 2);
            var username = credentials[0];
            var password = credentials[1];

            // ⚠️ Aqui seria a validação real (ex: Mongo, Identity, etc.)
            if (username != "admin" || password != "1234")
                return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));

            var claims = new[] { new Claim(ClaimTypes.Name, username) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }
}
```

---

## 🧩 5. Providers/MongoPolicyProvider.cs

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using YarpGateway.Models;

namespace YarpGateway.Providers;

public class MongoPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly IAuthorizationPolicyProvider _fallback;
    private readonly IMongoCollection<AuthorizationPolicyModel> _collection;
    private readonly IMemoryCache _cache;

    public MongoPolicyProvider(IMongoClient client, IOptions<AuthorizationOptions> options, IMemoryCache cache)
    {
        _fallback = new DefaultAuthorizationPolicyProvider(options);
        _collection = client.GetDatabase("YarpGatewayDb").GetCollection<AuthorizationPolicyModel>("Policies");
        _cache = cache;
    }

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (_cache.TryGetValue(policyName, out AuthorizationPolicy? cached))
            return cached;

        var policyDoc = await _collection.Find(p => p.Id == policyName).FirstOrDefaultAsync();
        if (policyDoc == null)
            return await _fallback.GetPolicyAsync(policyName);

        var builder = new AuthorizationPolicyBuilder();
        builder.AddAuthenticationSchemes(policyDoc.AuthenticationScheme);

        if (policyDoc.RequireAuthenticatedUser)
            builder.RequireAuthenticatedUser();

        var policy = builder.Build();
        _cache.Set(policyName, policy, TimeSpan.FromMinutes(5));

        return policy;
    }

    public Task<AuthorizationPolicy?> GetDefaultPolicyAsync() => _fallback.GetDefaultPolicyAsync();
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallback.GetFallbackPolicyAsync();
}
```

---

## 🔁 6. Providers/MongoYarpConfigProvider.cs

Esse provider carrega as **rotas** dinamicamente do MongoDB e as converte em `IProxyConfig`.

```csharp
using Microsoft.Extensions.Primitives;
using MongoDB.Driver;
using Yarp.ReverseProxy.Configuration;
using YarpGateway.Models;

namespace YarpGateway.Providers;

public class MongoYarpConfigProvider : IProxyConfigProvider
{
    private readonly IMongoCollection<RouteDefinition> _routes;
    private readonly CancellationTokenSource _cts = new();
    private volatile MongoProxyConfig _config;

    public MongoYarpConfigProvider(IMongoClient client)
    {
        _routes = client.GetDatabase("YarpGatewayDb").GetCollection<RouteDefinition>("Routes");
        _config = LoadConfig();
    }

    public IProxyConfig GetConfig() => _config;

    private MongoProxyConfig LoadConfig()
    {
        var routes = _routes.Find(_ => true).ToList();

        var routeConfigs = routes.Select(r => new RouteConfig
        {
            RouteId = r.RouteId,
            Match = new RouteMatch { Path = r.Path },
            AuthorizationPolicy = r.AuthorizationPolicy,
            ClusterId = $"{r.RouteId}-cluster"
        }).ToList();

        var clusterConfigs = routes.Select(r => new ClusterConfig
        {
            ClusterId = $"{r.RouteId}-cluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "dest1", new DestinationConfig { Address = r.ClusterAddress } }
            }
        }).ToList();

        return new MongoProxyConfig(routeConfigs, clusterConfigs, _cts.Token);
    }

    private class MongoProxyConfig : IProxyConfig
    {
        public MongoProxyConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters, CancellationToken token)
        {
            Routes = routes;
            Clusters = clusters;
            ChangeToken = new CancellationChangeToken(token);
        }

        public IReadOnlyList<RouteConfig> Routes { get; }
        public IReadOnlyList<ClusterConfig> Clusters { get; }
        public IChangeToken ChangeToken { get; }
    }
}
```

---

## 🚀 7. Program.cs

```csharp
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Yarp.ReverseProxy;
using YarpGateway.Auth;
using YarpGateway.Providers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

// Mongo
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var conn = builder.Configuration["Mongo:ConnectionString"];
    return new MongoClient(conn);
});

// Auth
builder.Services
    .AddAuthentication()
    .AddJwtBearer("JwtScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    })
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicScheme", null);

// Policies dinâmicas do Mongo
builder.Services.AddSingleton<IAuthorizationPolicyProvider, MongoPolicyProvider>();

builder.Services.AddAuthorization();

// YARP dinâmico do Mongo
builder.Services.AddSingleton<IProxyConfigProvider, MongoYarpConfigProvider>();
builder.Services.AddReverseProxy();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();

app.Run();
```

---

## 🗄️ 8. Exemplo de documentos no MongoDB

**Coleção: `Policies`**

```json
{
  "_id": "JwtPolicy",
  "authenticationScheme": "JwtScheme",
  "requireAuthenticatedUser": true
}
```

```json
{
  "_id": "BasicPolicy",
  "authenticationScheme": "BasicScheme",
  "requireAuthenticatedUser": true
}
```

**Coleção: `Routes`**

```json
{
  "routeId": "jwtRoute",
  "path": "/api/jwt/{**catch-all}",
  "clusterAddress": "https://meu-backend-jwt.local/",
  "authorizationPolicy": "JwtPolicy"
}
```

```json
{
  "routeId": "basicRoute",
  "path": "/api/basic/{**catch-all}",
  "clusterAddress": "https://meu-backend-basic.local/",
  "authorizationPolicy": "BasicPolicy"
}
```

---

## ✅ Resultado

| Tipo de rota     | Cabeçalho                       | Policy aplicada | Resultado                        |
| ---------------- | ------------------------------- | --------------- | -------------------------------- |
| `/api/jwt/...`   | `Authorization: Bearer <jwt>`   | `JwtPolicy`     | Passa só com token JWT válido    |
| `/api/basic/...` | `Authorization: Basic <base64>` | `BasicPolicy`   | Passa só com credenciais válidas |

---

Quer que eu adicione **suporte a atualização automática** (MongoDB Change Streams → YARP recarrega as rotas em tempo real)?
Isso deixaria o gateway 100% dinâmico sem reiniciar a aplicação.
