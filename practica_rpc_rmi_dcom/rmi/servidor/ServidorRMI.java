// =============================================================
//   PRÁCTICA — RMI (Remote Method Invocation)
//   SERVIDOR — Java
//   Máquina: Windows Server 2025
// =============================================================
//
// REQUISITOS:
//   Java JDK instalado (java y javac disponibles en PATH)
//
// COMPILAR (en Windows Server 2025, desde esta carpeta):
//   javac SumaRemota.java SumaRemotaImpl.java ServidorRMI.java
//
// EJECUTAR:
//   1. Iniciar el registro RMI (en una ventana separada):
//        start rmiregistry 1099
//   2. Iniciar el servidor:
//        java ServidorRMI
//
// NOTA: Abre el puerto 1099 en el firewall de Windows Server:
//   netsh advfirewall firewall add rule name="RMI" dir=in
//         action=allow protocol=TCP localport=1099
// =============================================================

import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;
import java.rmi.server.UnicastRemoteObject;
import java.net.InetAddress;

public class ServidorRMI {
    public static void main(String[] args) {
        try {
            // Crear implementación
            SumaRemotaImpl impl = new SumaRemotaImpl();

            // Exportar el objeto remoto
            SumaRemota stub = (SumaRemota) UnicastRemoteObject.exportObject(impl, 0);

            // Registrar en el Registry local (puerto 1099)
            Registry registry = LocateRegistry.getRegistry("localhost", 1099);
            registry.rebind("SumaRemotaService", stub);

            String ipLocal = InetAddress.getLocalHost().getHostAddress();
            System.out.println("=".repeat(55));
            System.out.println("  SERVIDOR RMI — Java");
            System.out.println("=".repeat(55));
            System.out.println("  IP local  : " + ipLocal);
            System.out.println("  Puerto    : 1099 (rmiregistry)");
            System.out.println("  Servicio  : SumaRemotaService");
            System.out.println("  Esperando clientes...");
            System.out.println("=".repeat(55));

        } catch (Exception e) {
            System.err.println("[ERROR] " + e.getMessage());
            e.printStackTrace();
        }
    }
}
