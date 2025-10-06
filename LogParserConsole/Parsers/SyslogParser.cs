using LogParser.Models;
using System.Text.RegularExpressions;

namespace LogParser.Parsers;

public class SyslogParser : ILogParser
{
    public IEnumerable<LogEntry> Parse(string filePath)
    {
        var pattern = @"^(?<month>\w+)\s+(?<day>\d+)\s+(?<time>\d{2}:\d{2}:\d{2})\s+(?<host>\S+)\s+(?<service>\S+):\s+(?<message>.+)$";

        foreach (var line in File.ReadLines(filePath))
        {
            var match = Regex.Match(line, pattern);
            if (match.Success)
            {
                var timestamp = $"{match.Groups["month"]} {match.Groups["day"]} {DateTime.Now.Year} {match.Groups["time"]}";
                yield return new LogEntry
                {
                    Timestamp = DateTime.Parse(timestamp),
                    Message = match.Groups["message"].Value
                };
            }
        }
    }
}