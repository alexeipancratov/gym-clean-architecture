namespace GymManagement.Core.ErrorHandling;

/// <summary>
/// Represents an error that occurred during the execution of an operation.
/// </summary>
public class OperationError
{
    public string Message { get; }

    public string Code { get; }

    public OperationErrorStatus Status { get; }

    public IReadOnlyList<ValidationError> ValidationErrors { get; } = [];
    
    private OperationError(string message, string code, OperationErrorStatus status)
    {
        Message = message;
        Code = code;
        Status = status;
    }
    
    private OperationError(string message, string code, OperationErrorStatus status,
        IReadOnlyList<ValidationError> validationErrors)
    {
        Message = message;
        Code = code;
        Status = status;
        ValidationErrors = validationErrors;
    }
    
    public static OperationError NotFound(string message, string code = "not_found")
        => new(message, code, OperationErrorStatus.NotFound);
    
    public static OperationError Forbidden(string message, string code = "forbidden")
        => new(message, code, OperationErrorStatus.Forbidden);
    
    public static OperationError Invalid(string message, string code = "invalid")
        => new(message, code, OperationErrorStatus.Invalid);
    
    public static OperationError Invalid(
        string message, IReadOnlyList<ValidationError> validationErrors, string code = "invalid")
        => new(message, code, OperationErrorStatus.Invalid, validationErrors);
    
    public static OperationError Unauthorized(string message, string code = "unauthorized")
        => new(message, code, OperationErrorStatus.Unauthorized);
    
    public static OperationError Internal(string message, string code = "internal")
        => new(message, code, OperationErrorStatus.Internal);
    
    public static OperationError Conflict(string message, string code = "conflict")
        => new(message, code, OperationErrorStatus.Conflict);
}