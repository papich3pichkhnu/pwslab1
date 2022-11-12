using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace lab1_pws.Validators
{
    public static class ValidatorExtensions
    {
        public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
