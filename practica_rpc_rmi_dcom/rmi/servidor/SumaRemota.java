// =============================================================
//   PRÁCTICA — RMI (Remote Method Invocation)
//   INTERFAZ REMOTA — Java
//   Este archivo va TANTO en el servidor como en el cliente
// =============================================================
import java.rmi.Remote;
import java.rmi.RemoteException;

public interface SumaRemota extends Remote {
    /**
     * Suma dos enteros y devuelve el resultado como String.
     * @param a primer entero
     * @param b segundo entero
     * @return resultado como cadena, p.ej. "7"
     */
    String sumaRemota(int a, int b) throws RemoteException;
}
