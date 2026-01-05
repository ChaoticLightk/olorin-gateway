using System;

namespace Messaging.Domain.Data.Policy.Commands.Provision;

public record ProvisionPolicyCommand(string QueueName, int Version);
