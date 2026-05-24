/*
=============================================================
  PRÁCTICA — RPC (XML-RPC)
  CLIENTE — C# (.NET)
  Máquina: Windows 10
=============================================================

COMPILAR Y EJECUTAR (en Windows 10):
  1. Abre "Símbolo del sistema" o PowerShell
  2. Instala el paquete NuGet XML-RPC.NET:
       dotnet add package xmlrpcnet
     O compila directamente con csc si tienes VS instalado.

  OPCIÓN MÁS FÁCIL — usar .NET con HttpClient (sin dependencias):
  Este archivo usa HttpClient puro para hacer la llamada XML-RPC
  manualmente, sin necesidad de paquetes externos.

  csc ClienteRpc.cs    (si tienes .NET Framework)
  dotnet run           (si usas .NET Core/5+)

IMPORTANTE: Cambia IP_DEL_SERVIDOR por la IP de tu máquina Debian.
=============================================================
*/

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

class ClienteRpc
{
    // ── CAMBIA ESTA IP por la de tu Debian ────────────────
    static readonly string IP_SERVIDOR = "192.168.100.138";
    static readonly int    PUERTO      = 8080;
    // ─────────────────────────────────────────────────────

    static async Task Main(string[] args)
    {
        Console.WriteLine(new string('=', 55));
        Console.WriteLine("  CLIENTE RPC — C# / XML-RPC");
        Console.WriteLine(new string('=', 55));
        Console.WriteLine($"  Servidor : {IP_SERVIDOR}:{PUERTO}");
        Console.WriteLine($"  Protocolo: XML-RPC sobre HTTP");
        Console.WriteLine(new string('=', 55));

        // Casos de prueba
        var casos = new (int a, int b)[] { (3, 4), (10, 25), (-5, 15), (100, 200) };

        using var http = new HttpClient();
        http.Timeout = TimeSpan.FromSeconds(5);

        foreach (var (a, b) in casos)
        {
            string resultado = await LlamarSumaRemota(http, a, b);
            Console.WriteLine($"  sumaRemota({a,4}, {b,4})  -->  \"{resultado}\"");
        }

        Console.WriteLine(new string('=', 55));
        Console.WriteLine("  [OK] Comunicación RPC exitosa");
        Console.WriteLine(new string('-', 55));
        Console.Write("\nPresiona ENTER para salir...");
        Console.ReadLine();
    }

    static async Task<string> LlamarSumaRemota(HttpClient http, int a, int b)
    {
        // Construir el mensaje XML-RPC manualmente
        string xmlRequest =
            "<?xml version=\"1.0\"?>" +
            "<methodCall>" +
            "  <methodName>sumaRemota</methodName>" +
            "  <params>" +
            $"    <param><value><int>{a}</int></value></param>" +
            $"    <param><value><int>{b}</int></value></param>" +
            "  </params>" +
            "</methodCall>";

        var content = new StringContent(xmlRequest, Encoding.UTF8, "text/xml");
        string url  = $"http://{IP_SERVIDOR}:{PUERTO}/";

        HttpResponseMessage response = await http.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        string xmlResponse = await response.Content.ReadAsStringAsync();

        // Parsear la respuesta XML-RPC
        var doc = new XmlDocument();
        doc.LoadXml(xmlResponse);
        XmlNode? valueNode = doc.SelectSingleNode("//methodResponse/params/param/value/string");
        return valueNode?.InnerText ?? "ERROR";
    }
}
