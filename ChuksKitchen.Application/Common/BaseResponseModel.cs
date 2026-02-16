namespace ChuksKitchen.Application.Common;

public class BaseResponseModel<T>
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public List<string> Errors { get; set; } = new();

    // Success Response
    public static BaseResponseModel<T> SuccessResponse( T? data = default, string message = "")
    {
        return new BaseResponseModel<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    // Failure Response (Single Error)
    public static BaseResponseModel<T> FailureResponse(string message)
    {
        return new BaseResponseModel<T>
        {
            Success = false,
            Message = message
        };
    }

    // Failure Response (With Errors)
    public static BaseResponseModel<T> FailureResponse(
        string message,
        List<string> errors)
    {
        return new BaseResponseModel<T>
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}
