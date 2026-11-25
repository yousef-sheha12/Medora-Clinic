using FluentValidation;
using FluentValidation.Results;
using MedoraClinic.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MedoraClinic.API.Filters;

public sealed class FluentValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context,ActionExecutionDelegate next)
    {
        var failures = new List<ValidationFailure>();
        var services = context.HttpContext.RequestServices;

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            if (services.GetService(validatorType) is not IValidator validator)
                continue;

            var validationContext = new ValidationContext<object>(argument);
            var result = await validator.ValidateAsync(validationContext,context.HttpContext.RequestAborted);

            failures.AddRange(result.Errors);
        }

        if (failures.Count > 0)
        {
            var errors = failures
                .Select(failure => $"{failure.PropertyName}: {failure.ErrorMessage}")
                .Distinct()
                .ToArray();

            var response = ApiResponse<object>.Failure("البيانات المرسلة غير صحيحة.",errors,StatusCodes.Status400BadRequest);

            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };

            return;
        }

        await next();
    }
}
