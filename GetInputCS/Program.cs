using System.Net;

if (!File.Exists("cookies"))
{
    Console.WriteLine("Hace falta un archivo \"cookies\"");
    Environment.Exit(1);
} 

HttpClient client = new();

HttpRequestHeader reqHed = HttpRequestHeader.Cookie;

HttpRequestMessage req = new();


client.DefaultRequestHeaders.
