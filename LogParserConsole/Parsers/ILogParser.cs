using LogParser.Models;

namespace LogParser.Parsers;

public interface ILogParser
{
    IEnumerable<LogEntry> Parse(string filePath);
}