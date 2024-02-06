using NtTaskWebServer.Framework;
using NtTaskWebServer.Framework.Helpers;
using System.Configuration;

var app = new WebServer();
var cts = new CancellationTokenSource();
AppDomain.CurrentDomain.UnhandledException+=UnhandledExceptionHandler;

var connectionString = ConfigurationManager.ConnectionStrings["nttask"].ConnectionString;
var hashSalt = ConfigurationManager.AppSettings.Get("salt");
DatabaseHelper.Init(connectionString, hashSalt);

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




static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
{
    // Get the exception object
    Exception exception = e.ExceptionObject as Exception;

    // Log or handle the exception as needed
    Console.WriteLine($"Unhandled Exception: {exception}");

    // Terminate the application gracefully (optional)
    Environment.Exit(1);
}