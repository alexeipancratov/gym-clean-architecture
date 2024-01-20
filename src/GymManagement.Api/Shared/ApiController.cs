using GymManagement.Core.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GymManagement.Api.Shared;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(OperationError operationError)
    {
        var statusCode = operationError.Status switch
        {
            OperationErrorStatus.NotFound => StatusCodes.Status404NotFound,
            OperationErrorStatus.Forbidden => StatusCodes.Status403Forbidden,
            OperationErrorStatus.Invalid => StatusCodes.Status400BadRequest,
            OperationErrorStatus.Unauthorized => StatusCodes.Status401Unauthorized,
            OperationErrorStatus.Internal => StatusCodes.Status500InternalServerError,
            OperationErrorStatus.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
        
        if (operationError.Status == OperationErrorStatus.Invalid && operationError.ValidationErrors.Any())
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var validationError in operationError.ValidationErrors)
            {
                modelStateDictionary.AddModelError(
                    validationError.Identifier,
                    validationError.Message);
            }
            return ValidationProblem(modelStateDictionary);
        }

        return Problem(statusCode: statusCode, detail: operationError.Message);
    }
}