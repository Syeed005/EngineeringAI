

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngineeringAI.Api.Filters {
    public class ValidationFilter<T> : IAsyncActionFilter where T : class {
        private readonly IValidator<T> _validator;

        public ValidationFilter(IValidator<T> validator) {
            _validator = validator;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            var model = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

            if (model == null) {
                await next();
                return;
            }

            var validationResult = await _validator.ValidateAsync(model, context.HttpContext.RequestAborted);

            if (!validationResult.IsValid) {
                var errors = validationResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(e => e.ErrorMessage).ToArray());
                
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors) {
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest
                });

                return;
            }

            await next();
        }
    }
}
