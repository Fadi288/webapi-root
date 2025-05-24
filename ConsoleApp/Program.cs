Console.WriteLine("--------------Console app started------------");


var handler = new HttpClientHandler();
// TEMP: Accept untrusted certs inside container, remove in production
handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

var client = new HttpClient(handler)
{
    BaseAddress = new Uri("https://webapi:443") // Docker internal URL
};

Console.WriteLine("..Calling WebApi...");

try
{
    var result = await client.GetStringAsync("weatherforecast");
    Console.WriteLine("Response from WebApi:");
    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}


Console.ReadLine();