using FluentValidation;
using MediatR;

namespace BouvetOne.MediatR.Core.Infrastructure.Behaviors;

public class CommandValidationPipelineBehavior<TRequest,TResponse> : IPipelineBehavior<TRequest,TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public CommandValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var errorsDictionary = _validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct()
                })
            .ToDictionary(x => x.Key, x => x.Values);

        if (!errorsDictionary.Any()) return await next();

        var errorLists = errorsDictionary.Values.ToList().Select(item => item.FirstOrDefault()).ToList();
        throw new ArgumentException(string.Join(", ", errorLists));
    }
}