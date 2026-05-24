// =============================================================
//   PRÁCTICA — RMI
//   IMPLEMENTACIÓN DEL OBJETO REMOTO — Java
// =============================================================
import java.rmi.RemoteException;

public class SumaRemotaImpl implements SumaRemota {

    @Override
    public String sumaRemota(int a, int b) throws RemoteException {
        int resultado = a + b;
        System.out.println("  [RMI] sumaRemota(" + a + ", " + b + ") → \"" + resultado + "\"");
        return String.valueOf(resultado);
    }
}
