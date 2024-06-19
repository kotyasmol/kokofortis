// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Reporters;

Console.WriteLine("Hello, World!");

//var ports = new List<JsonPortDescription>
//{
//    new JsonPortDescription(isSfp: false, PoeType.Poe, 4),
//    new JsonPortDescription(isSfp: true, PoeType.No, 2)
//};

//var contacts = new List<JsonContactDescription>
//{
//    new JsonContactDescription(ContactType.Input, 2)
//};

//var descr = new JsonModelDescription(9, "PSW-2G4F-UPS", true, ports, contacts);

//JsonSerializerOptions opts = new JsonSerializerOptions
//{
//    WriteIndented = true,
//    Converters =
//    {
//        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
//    }
//};

//var list = new List<JsonModelDescription> { descr, descr };
//var json = JsonSerializer.Serialize(list, opts);

//File.WriteAllText("D:\\tmp\\d.json", json);



//var reporter = new DeviceReporter(new MyLogger());

//var device = NetworkSwitch.TryCreateAsync(5, reporter, CancellationToken.None).Result;
//var device = DevicesFactory.Instance.TryCreateAsync(1, "192.168.0.1", "255.255.255.0", "192.168.0.100", "", "", "123", "1h", reporter, CancellationToken.None).Result;
//var device2 = DevicesFactory.Instance.TryCreateAsync(2, "192.168.0.2", "255.255.255.0", "192.168.0.100", "", "", "123", "2h", reporter, CancellationToken.None).Result;
//var device3 = DevicesFactory.Instance.TryCreateAsync(3, "192.168.0.3", "255.255.255.0", "192.168.0.100", "", "", "1234", "3h", reporter, CancellationToken.None).Result;
//var device4 = DevicesFactory.Instance.TryCreateAsync(9, "192.168.0.4", "255.255.255.0", "192.168.0.100", "", "", "1235", "4h", reporter, CancellationToken.None).Result;
//var device5 = DevicesFactory.Instance.TryCreateAsync(9, "192.168.0.5", "255.255.255.0", "192.168.0.100", "", "", "1236", "5h", reporter, CancellationToken.None).Result;

Console.WriteLine();

class MyLogger : ILogger
{
    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        Debug.WriteLine(exception);
    }
}