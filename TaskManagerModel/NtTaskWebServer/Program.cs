using NtTaskWebServer.Framework;

var app = new WebServer();
var cts = new CancellationTokenSource();
_=app.ListenAsync("http://127.0.0.1:5051/", cts.Token);

try
{
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
}
catch (Exception exception)
{
    Console.WriteLine(exception.Message);
}



