using Package.ResultPattern;
using Package.ResultPattern.AspNetCore.MinimalApi;
using Messaging.Domain.Data.Policy.Commands.Create;
using Messaging.Domain.Data.Policy.Commands.Delete;
using Messaging.Domain.Data.Policy.Commands.Provision;
using Messaging.Domain.Data.Policy.Commands.Update;
using Messaging.Domain.Data.Policy.Enums;
using Messaging.Domain.Data.Policy.Queries.DTO;
using Messaging.Domain.Data.Policy.Queries.Get;
using Messaging.Domain.Data.Policy.Queries.GetById;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Messaging.Presentation.Endpoints.Policy;

public partial class QueuePolicyEndpoints 
{
    static async Task<IResult> CreatePolicy([FromBody] CreatePolicyCommand request, IMessageBus bus)
    {
        var result = await bus.InvokeAsync<Result>(request);
        return result.ToMinimalResult();
    }

    static async Task<IResult> PatchPolicy([FromBody] UpdatePolicyCommand request, IMessageBus bus)
    {
        var result = await bus.InvokeAsync<Result>(request);
        return result.ToMinimalResult();
    }

    static async Task<IResult> ProvisionPolicy([FromBody] ProvisionPolicyCommand request, IMessageBus bus)
    {
        var result = await bus.InvokeAsync<Result>(request);
        return result.ToMinimalResult();
    }

    static async Task<IResult> GetAllPolicies(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] FilterQueuePolicy filter,
        IMessageBus bus)
    {
        var request = new GetPoliciesQuery(page, pageSize, filter); 

        var result = await bus
            .InvokeAsync<Result<List<PolicyResponse>>>(request);

        return result.ToMinimalResult();
    }

    static async Task<IResult> GetAllDeletedPolicies([FromQuery] int Page, [FromQuery] int PageSize, IMessageBus bus)
    {
        var request = new GetPoliciesQuery(Page, PageSize, FilterQueuePolicy.Onlydeleted); 

        var result = await bus
            .InvokeAsync<Result<List<PolicyResponse>>>(request);

        return result.ToMinimalResult();
    }

    static async Task<IResult> GetPolicyById([FromRoute] string Id, IMessageBus bus)
    {
        var request = new GetPolicyByIdQuery(Id);

        var result = await bus
            .InvokeAsync<Result<PolicyResponse>>(request);

        return result.ToMinimalResult();
    }

    static async Task<IResult> DeletePolicyById([FromRoute] string Id, IMessageBus bus)
    {
        var request = new DeletePolicyCommand(Id);

        var result = await bus
            .InvokeAsync<Result>(request);

        return result.ToMinimalResult();
    }

    static async Task<IResult> RestorePolicyById([FromRoute] string Id, IMessageBus bus)
    {
        var request = new DeletePolicyCommand(Id);

        var result = await bus
            .InvokeAsync<Result>(request);

        return result.ToMinimalResult();
    }
}
