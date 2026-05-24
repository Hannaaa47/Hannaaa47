using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ServidorDCOM
{
    static void Main()
    {
        int puerto = 9090;
        string ipLocal = Dns.GetHostEntry(Dns.GetHostName())
                           .AddressList
                           .Cast<IPAddress>()
                           .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                           ?.ToString() ?? "desconocida";

        TcpListener listener = new TcpListener(IPAddress.Any, puerto);
        listener.Start();

        Console.WriteLine(new string('=', 55));
        Console.WriteLine("  SERVIDOR DCOM — C# / TCP");
        Console.WriteLine(new string('=', 55));
        Console.WriteLine($"  IP local  : {ipLocal}");
        Console.WriteLine($"  Puerto    : {puerto}");
        Console.WriteLine("  Esperando clientes... (Ctrl+C para salir)");
        Console.WriteLine(new string('=', 55));

        while (true)
        {
            TcpClient cliente = listener.AcceptTcpClient();
            Thread t = new Thread(() => ManejarCliente(cliente));
            t.Start();
        }
    }

    static void ManejarCliente(TcpClient cliente)
    {
        using (cliente)
        {
            NetworkStream stream = cliente.GetStream();
            byte[] buffer = new byte[1024];
            int bytes = stream.Read(buffer, 0, buffer.Length);
            string mensaje = Encoding.UTF8.GetString(buffer, 0, bytes).Trim();

            string[] partes = mensaje.Split(',');
            int a = int.Parse(partes[0]);
            int b = int.Parse(partes[1]);
            int resultado = a + b;

            Console.WriteLine($"  [DCOM] SumaRemota({a}, {b}) → \"{resultado}\"");

            byte[] respuesta = Encoding.UTF8.GetBytes(resultado.ToString() + "\n");
            stream.Write(respuesta, 0, respuesta.Length);
        }
    }
}