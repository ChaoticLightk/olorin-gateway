using Colombo.ResultPattern;
using Colombo.ResultPattern.AspNetCore.MinimalApi;
using Messaging.Domain.Data.Messaging.Commands.Send;
using Messaging.Domain.Data.Policy.Commands.Create;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Messaging.Presentation.Endpoints.Queue;

public partial class MessagingEndpoints
{
    static async Task<IResult> PublishMessage([FromBody] SendMessageCommand request, IMessageBus bus)
    {
        var result = await bus.InvokeAsync<Result>(request);
        return result.ToMinimalResult();
    }

    static async Task<IResult> GetMessages(IMessageBus bus) => Results.Ok();
    static async Task<IResult> GetMessageById([FromRoute] string id, IMessageBus bus) => Results.Ok();
    static async Task<IResult> GetMessagesByQueue([FromRoute] string queue, IMessageBus bus) => Results.Ok();
    static async Task<IResult> PatchMessage([FromRoute] string id, [FromBody] object request, IMessageBus bus) => Results.Ok();
    static async Task<IResult> RetryMessages([FromQuery] string fodase, IMessageBus bus) => Results.Ok();
    static async Task<IResult> DeleteMessages([FromBody] CreatePolicyCommand request, IMessageBus bus) => Results.Ok();
}
