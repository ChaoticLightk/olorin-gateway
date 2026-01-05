using FluentValidation;

namespace Messaging.Domain.Chain.Middlewares;

public class ValidationMiddleware
{
    public static async Task BeforeAsync<T>(
        T message,
        IEnumerable<IValidator<T>> validators,
        CancellationToken ct)
    {
        if (!validators.Any())
        {
            return;
        }

        var context = new ValidationContext<T>(message);

        var results = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, ct)));

        var failures = results
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count == 0)
        {
            return;
        }

        throw new ValidationException(failures);
    }
}
