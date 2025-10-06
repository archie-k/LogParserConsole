namespace LogParser.Models;

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Ip { get; set; }
}