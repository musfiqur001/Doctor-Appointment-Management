namespace Doctor_Appointment_Management.Utility.Common;

public class ResultData
{
    public object? Data { get; set; }
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
}
