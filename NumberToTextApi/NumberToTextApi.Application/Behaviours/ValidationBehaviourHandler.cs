using Ardalis.Result;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace NumberToTextApi.Application.Behaviours
{
    /// <summary>
    /// Prepares a pipeline for requests validations
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="logger"></param>
    /// <param name="validators"></param>
    public class ValidationBehaviourHandler<TRequest, TResponse>(ILogger<ValidationBehaviourHandler<TRequest, TResponse>> logger, IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
        where TResponse : notnull
    {

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestTypeName = typeof(TRequest).Name;

            if (!validators.Any())
            {
                logger.LogDebug("no validators for the request");
                return await next();
            }
            var context = new ValidationContext<TRequest>(request);
            var failures = validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

            if (!failures.Any())
            {
                return await next();
            }


            var errorList = failures.Select(f => new ValidationError()
            {
                Identifier = f.ErrorCode+f.PropertyName,
                ErrorMessage = f.ErrorMessage,
                ErrorCode = f.ErrorCode,
                Severity = (ValidationSeverity)(int)f.Severity
            }).ToArray();

            var resType = typeof(TResponse);
            var method = resType.GetMethod(nameof(Result.Invalid), BindingFlags.Static | BindingFlags.Public, null, [typeof(List<ValidationError>)], null);
            if (method is not null)
            {
                var respResult = method?.Invoke(null, [errorList]);
                if (respResult is not null)
                {
                    return (TResponse)respResult;
                }
            }
            return Result<TResponse>.Invalid(errorList);
        }
    }
}
