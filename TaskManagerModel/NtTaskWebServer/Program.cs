using NtTaskWebServer.Framework;

var app = new WebServer();
var cts = new CancellationTokenSource();
app.ListenAsync("http://127.0.0.1:5051/", cts.Token);


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


