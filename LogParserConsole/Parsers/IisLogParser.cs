using LogParser.Models;

namespace LogParser.Parsers;

public class IisLogParser : ILogParser
{
    public IEnumerable<LogEntry> Parse(string filePath)
    {
        string[]? headers = null;

        foreach (var line in File.ReadLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line.StartsWith("#Fields:"))
            {
                headers = line.Substring("#Fields:".Length).Trim().Split(' ');
                continue;
            }

            if (line.StartsWith("#")) continue;

            if (headers == null) continue;

            var values = line.Split(' ');
            if (values.Length != headers.Length) continue;

            var data = headers.Zip(values, (key, val) => new { key, val })
                              .ToDictionary(x => x.key, x => x.val);

            yield return new LogEntry
            {
                Timestamp = ParseTimestamp(data),
                Message = $"{data["cs-method"]} {data["cs-uri-stem"]} {data["sc-status"]} from {data["c-ip"]}",
                Ip = data.TryGetValue("c-ip", out var ip) ? ip : null
            };
        }
    }

    private DateTime ParseTimestamp(Dictionary<string, string> data)
    {
        if (data.TryGetValue("date", out var dateStr) && data.TryGetValue("time", out var timeStr))
        {
            var combined = $"{dateStr} {timeStr}";
            if (DateTime.TryParse(combined, out var dt))
                return dt;
        }

        return DateTime.MinValue;
    }

    public void CountIps(IEnumerable<LogEntry> entries)
    {
        var ipCount = new Dictionary<string, int>();
        var prefixCount = new Dictionary<string, int>();
        var uniqueIps = new HashSet<string>();

        foreach (var entry in entries)
        {
            if (string.IsNullOrEmpty(entry.Ip)) continue;

            uniqueIps.Add(entry.Ip!);

            // pełny IP
            if (!ipCount.ContainsKey(entry.Ip!))
                ipCount[entry.Ip!] = 0;
            ipCount[entry.Ip!]++;

            // pierwsze dwie oktety
            var prefix = string.Join('.', entry.Ip!.Split('.').Take(2));
            if (!prefixCount.ContainsKey(prefix))
                prefixCount[prefix] = 0;
            prefixCount[prefix]++;
        }

        Console.WriteLine($"\n📌 Liczba unikalnych IP: {uniqueIps.Count}");

        Console.WriteLine("\n🔢 TOP 10 pełnych IP:");
        foreach (var pair in ipCount.OrderByDescending(kv => kv.Value).Take(10))
        {
            Console.WriteLine($"{pair.Key} => {pair.Value}");
        }

        Console.WriteLine("\n🧩 TOP 10 prefiksów (pierwsze 2 oktety):");
        foreach (var pair in prefixCount.OrderByDescending(kv => kv.Value).Take(10))
        {
            Console.WriteLine($"{pair.Key}.x.x => {pair.Value}");
        }
    }
}