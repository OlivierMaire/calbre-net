
public class ServiceException : Exception
{
    /// <summary>
    /// HTTP Status code to return when this exception is
    /// handled by UnhandledExceptionFilter
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// An optional collection of errors that can be set
    /// to provide more error detail
    /// </summary>
    public string[] Errors { get; set; }

    public string ErrorCode { get; set; }

    /// <summary>
    /// Create a new exception with a message, status code
    /// and optional error collection
    /// </summary>
    /// <param name="message"></param>
    /// <param name="statusCode">Optional - Http Status code to display</param>
    /// <param name="errors">Optional - Collection of validation errors</param>
    /// <param name="errorCode">Optional - An error code for this error</param>
    public ServiceException(string message,
        int statusCode = 500,
        string[] errors = null,
        string errorCode = null) :
        base(message)
    {
        StatusCode = statusCode;
        Errors = errors;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Create a new Api Exception from an existing exception
    /// with a status code
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="statusCode"></param>
    public ServiceException(Exception ex, int statusCode = 500, string errorCode = null) : base(ex.Message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

}