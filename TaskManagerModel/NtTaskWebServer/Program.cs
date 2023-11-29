using NtTaskWebServer.Framework;

var app = new WebServer();
var cts = new CancellationTokenSource();
try
{
    _=app.ListenAsync("http://127.0.0.1:5051/", cts.Token);
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




