namespace Contactor.Tests.Dto;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

internal static class SimpleModelValidator
{
    public static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}
