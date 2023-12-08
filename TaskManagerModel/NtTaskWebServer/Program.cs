using NtTaskWebServer.Framework;
using System.Configuration;

var app = new WebServer();
var cts = new CancellationTokenSource();
try
{
    _=app.ListenAsync(ConfigurationManager.AppSettings["DefaultPrefix"], cts.Token);
}
catch (Exception exception)
{
    Console.WriteLine(exception.Message);
}


while (app.IsServerWork)
{
    switch (Console.ReadKey().KeyChar)
    {
        case 'q':
            cts.Cancel();
            break;
        default:
            break;
    }
}




