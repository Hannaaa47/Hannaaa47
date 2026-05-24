// =============================================================
//   PRÁCTICA — RMI (Remote Method Invocation)
//   CLIENTE — Java
//   Máquina: Windows 10
// =============================================================
//
// ARCHIVOS NECESARIOS en esta carpeta:
//   - SumaRemota.java   (la interfaz, copia del servidor)
//   - ClienteRMI.java   (este archivo)
//
// COMPILAR (en Windows 10, desde esta carpeta):
//   javac SumaRemota.java ClienteRMI.java
//
// EJECUTAR:
//   java ClienteRMI <IP_SERVIDOR>
//   Ejemplo:
//   java ClienteRMI 192.168.1.101
//
// IMPORTANTE: Cambia la IP por la de tu Windows Server 2025
// =============================================================

import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;

public class ClienteRMI {

    public static void main(String[] args) {
        // IP del servidor (Windows Server 2025) — pásala como argumento
        // o cámbiala aquí directamente
        String ipServidor = args.length > 0 ? args[0] : "192.168.1.101";
        int    puerto     = 1099;

        System.out.println("=".repeat(55));
        System.out.println("  CLIENTE RMI — Java");
        System.out.println("=".repeat(55));
        System.out.println("  Servidor  : " + ipServidor + ":" + puerto);
        System.out.println("  Protocolo : Java RMI / JRMP");
        System.out.println("=".repeat(55));

        try {
            // Obtener referencia al registro remoto
            Registry registry = LocateRegistry.getRegistry(ipServidor, puerto);

            // Buscar el objeto remoto por nombre
            SumaRemota servicio = (SumaRemota) registry.lookup("SumaRemotaService");

            // Casos de prueba
            int[][] casos = {{3, 4}, {10, 25}, {-5, 15}, {100, 200}};

            for (int[] c : casos) {
                String resultado = servicio.sumaRemota(c[0], c[1]);
                System.out.printf("  sumaRemota(%4d, %4d)  -->  \"%s\"%n",
                                  c[0], c[1], resultado);
            }

            System.out.println("=".repeat(55));
            System.out.println("  [OK] Comunicación RMI exitosa");

        } catch (Exception e) {
            System.err.println("[ERROR] " + e.getMessage());
            e.printStackTrace();
        }
    }
}
