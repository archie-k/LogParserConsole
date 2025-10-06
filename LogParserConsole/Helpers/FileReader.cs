using System.Reflection;

namespace LogParser.Helpers;

public static class FileReader
{
    public static void PrintEntries(IEnumerable<Models.LogEntry> entries)
    {
        foreach (var entry in entries)
        {
            Console.WriteLine($"[{entry.Timestamp}] {entry.Message}");
        }
    }
}