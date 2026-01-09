using Messaging.Domain.Shared.Messaging.DTO;

namespace Messaging.Domain.Shared.Messaging.Services;

public interface IProvisionService
{
    Task ProvisionAsync(
        ProvisionQueue provision,
        CancellationToken ct
    );
}
