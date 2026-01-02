using Colombo.ResultPattern;
using Colombo.ResultPattern.AspNetCore.MinimalApi;
using Messaging.Domain.Data.Policy.Commands.Create;
using Messaging.Domain.Data.Policy.Commands.Delete;
using Messaging.Domain.Data.Policy.Commands.Update;
using Messaging.Domain.Data.Policy.Queries.DTO;
using Messaging.Domain.Data.Policy.Queries.Get;
using Messaging.Domain.Data.Policy.Queries.GetById;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Messaging.Presentation.Endpoints.Policy;

public partial class QueuePolicyEndpoints 
{
    static async Task<IResult> CreatePolicy(IMessageBus bus, [FromBody] CreatePolicyCommand request)
    {
        var result = await bus.InvokeAsync<Result>(request);
        return result.ToMinimalResult();
    }

    static async Task<IResult> PatchPolicyById([FromBody] UpdatePolicyCommand request, IMessageBus bus)
    {
        var result = await bus.InvokeAsync<Result>(request);
        return result.ToMinimalResult();
    }

    static async Task<IResult> GetAllPolicies([FromQuery] int Page, [FromQuery] int PageSize, IMessageBus bus)
    {
        var request = new GetPoliciesQuery(Page, PageSize); 

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
}
