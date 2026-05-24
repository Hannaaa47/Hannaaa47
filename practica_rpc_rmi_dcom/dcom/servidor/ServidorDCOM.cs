/*
=============================================================
  PRÁCTICA — DCOM
  SERVIDOR — C# (.NET COM Server)
  Máquina: Windows Server 2025
=============================================================

¿Qué es DCOM aquí?
  DCOM (Distributed COM) es COM sobre la red. En .NET moderno,
  la forma práctica de implementarlo es registrar una clase COM
  y exponerla vía .NET Remoting / COM Interop para que un cliente
  en otra máquina la instancie y llame remotamente.

  Para esta práctica usamos .NET Remoting TCP (que es la
  implementación equivalente a DCOM en .NET), que permite
  llamadas remotas a objetos .NET entre máquinas Windows.
  El cliente Python usará win32com para conectarse como
  cliente COM tradicional en Windows 10.

COMPILAR (PowerShell en Windows Server 2025):
  dotnet new console -n ServidorDCOM
  Reemplaza Program.cs con este archivo.
  dotnet run

ALTERNATIVA SIN .NET CLI:
  Copia todo en un archivo ServidorDCOM.csproj + este Program.cs
  y ejecuta: dotnet run

ABRE EL PUERTO EN FIREWALL:
  netsh advfirewall firewall add rule name="DCOM_RPC" ^
        dir=in action=allow protocol=TCP localport=9090
=============================================================
*/

using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Net;

// ── Objeto remoto (hereda MarshalByRefObject para ser remoto) ─
public class ServicioDCOM : MarshalByRefObject
{
    public string SumaRemota(int a, int b)
    {
        int resultado = a + b;
        Console.WriteLine($"  [DCOM] SumaRemota({a}, {b}) → \"{resultado}\"");
        return resultado.ToString();
    }

    // Necesario para que el objeto no expire
    public override object InitializeLifetimeService() => null!;
}

// ── Servidor principal ────────────────────────────────────────
class ServidorDCOM
{
    static void Main()
    {
        int    puerto   = 9090;
        string ipLocal  = Dns.GetHostEntry(Dns.GetHostName())
                            .AddressList
                            .FirstOrDefault(ip => ip.AddressFamily ==
                                System.Net.Sockets.AddressFamily.InterNetwork)
                            ?.ToString() ?? "desconocida";

        // Registrar canal TCP
        var channel = new TcpServerChannel(puerto);
        ChannelServices.RegisterChannel(channel, false);

        // Registrar el tipo remoto
        RemotingConfiguration.RegisterWellKnownServiceType(
            type       : typeof(ServicioDCOM),
            objectUri  : "ServicioDCOM",
            mode       : WellKnownObjectMode.Singleton
        );

        Console.WriteLine(new string('=', 55));
        Console.WriteLine("  SERVIDOR DCOM — C# / .NET Remoting TCP");
        Console.WriteLine(new string('=', 55));
        Console.WriteLine($"  IP local  : {ipLocal}");
        Console.WriteLine($"  Puerto    : {puerto}");
        Console.WriteLine($"  URI       : tcp://{ipLocal}:{puerto}/ServicioDCOM");
        Console.WriteLine("  Esperando clientes... (Ctrl+C para salir)");
        Console.WriteLine(new string('=', 55));

        Console.ReadLine(); // Mantener vivo
    }
}
