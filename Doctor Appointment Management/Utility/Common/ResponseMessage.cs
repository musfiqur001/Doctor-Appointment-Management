using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Doctor_Appointment_Management.Utility.Common;

public static class ResponseMessage
{
    public const string Fail = "An error occurred";
    public const string Save = "Data saved successfully";
    public const string Update = "Data updated successfully";
    public const string Delete = "Data deleted successfully";
    public const string Get = "Data retrieved successfully";
    public const string GetLoadFailed = "Failed to retrieve data";

    // Additional Messages
    public const string NotFound = "Requested data not found";
    public const string Conflict = "Data conflict occurred";
    public const string ValidationError = "Invalid input data";
    public const string Unauthorized = "Unauthorized access";
    public const string Forbidden = "You do not have permission to perform this action";

    // Server & Processing Errors
    public const string ServerError = "An internal server error occurred";
    public const string Processing = "Request is being processed";

    //// **Pagination Messages**  
    //public const string PageFetched = "Page {0} retrieved successfully";
    ////Console.WriteLine(string.Format(ApiResponseMessages.PageFetched, pageNumber));
    //public const string NoMorePages = "No more pages available";
    //public const string InvalidPage = "Invalid page number";
    //public const string PageSizeExceeded = "Page size exceeds the maximum allowed limit";
    //public const string EmptyPage = "No data available for the requested page";

    //// Rate Limiting
    //public const string TooManyRequests = "Too many requests, please try again later";

}
