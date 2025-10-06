using LogParser.Parsers;
using LogParser.Helpers;

Console.WriteLine("=== Log Parser ===");
Console.Write("Podaj ścieżkę do pliku logu: ");
var path = Console.ReadLine();

Console.WriteLine("Wybierz typ logów:");
Console.WriteLine("1. Syslog");
Console.WriteLine("2. JSON log");
Console.WriteLine("3. IIS log");
var option = Console.ReadLine();

ILogParser parser = option switch
{
    "1" => new SyslogParser(),
    "2" => new JsonLogParser(),
    "3" => new IisLogParser(),
    _ => throw new InvalidOperationException("Nieobsługiwany typ logu")
};

var entries = parser.Parse(path!).ToList();

//FileReader.PrintEntries(entries);

// Agregacja IP tylko dla parsera IIS
if (parser is IisLogParser iis)
{
    iis.CountIps(entries);
}


Console.ReadLine();