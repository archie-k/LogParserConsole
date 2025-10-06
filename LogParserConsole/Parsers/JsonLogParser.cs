using LogParser.Models;
using System.Text.Json;

namespace LogParser.Parsers;

public class JsonLogParser : ILogParser
{
    public IEnumerable<LogEntry> Parse(string filePath)
    {
        foreach (var line in File.ReadLines(filePath))
        {
            var entry = JsonSerializer.Deserialize<LogEntry>(line);
            if (entry != null)
                yield return entry;
        }
    }
}