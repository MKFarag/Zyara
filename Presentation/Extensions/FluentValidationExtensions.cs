using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Presentation.Extensions;

public static class FluentValidationExtensions
{
    public static IActionResult ToProblem(this ControllerBase controller, ValidationResult result)
        => controller.ValidationProblem(result.ToModelState());

    private static ModelStateDictionary ToModelState(this ValidationResult result)
    {
        var modelState = new ModelStateDictionary();

        foreach (var error in result.Errors)
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);

        return modelState;
    }
}
