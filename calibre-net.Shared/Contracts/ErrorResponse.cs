using System.ComponentModel;

namespace calibre_net.Shared.Contracts;
public class ErrorResponse  
{
    private static readonly string[] _item = new string[1] { "application/problem+json" };

    //
    // Summary:
    //     the http status code sent to the client. default is 400.
    [DefaultValue(400)]
    public int StatusCode { get; set; }

    //
    // Summary:
    //     the message for the error response
    [DefaultValue("One or more errors occurred!")]
    public string Message { get; set; } = "One or more errors occurred!";

    //
    // Summary:
    //     the collection of errors for the current context
    public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
 
}