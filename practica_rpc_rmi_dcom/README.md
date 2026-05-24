# Práctica: RPC, RMI y DCOM — Suma Remota

## Tabla Resumen

| Protocolo | Lenguaje Servidor | Lenguaje Cliente | VM Servidor     | VM Cliente      |
|-----------|-------------------|------------------|-----------------|-----------------|
| RPC       | Python 3          | C# (.NET)        | Debian          | Windows 10      |
| RMI       | Java              | Java             | Windows Server  | Windows 10      |
| DCOM      | C# (.NET)         | Python           | Windows Server  | Windows 10      |

---

## Configuración de Red

Antes de empezar, verifica que tus VMs se pueden comunicar:
- En cada VM ejecuta `ipconfig` (Windows) o `ip a` (Debian) y anota la IP
- Haz ping entre ellas para confirmar conectividad

---

## PRÁCTICA 1 — RPC (XML-RPC)

**Servidor: Debian | Cliente: Windows 10**

### En Debian (servidor):
```bash
# No se necesita instalar nada, xmlrpc viene con Python 3
python3 servidor_rpc.py
```
Anota la IP que aparece en pantalla.

### En Windows 10 (cliente):
```powershell
# Instala .NET SDK si no lo tienes
# https://dotnet.microsoft.com/download

# Edita ClienteRpc.cs y cambia IP_SERVIDOR por la IP de Debian

cd rpc/cliente
dotnet run
```

### Firewall en Debian:
```bash
# Si ufw está activo:
sudo ufw allow 8080/tcp
```

---

## PRÁCTICA 2 — RMI (Java)

**Servidor: Windows Server 2025 | Cliente: Windows 10**

### En Windows Server 2025 (servidor):

1. Instala JDK si no lo tienes:
   ```
   winget install Microsoft.OpenJDK.21
   ```

2. Compila y ejecuta:
   ```cmd
   cd rmi\servidor
   javac SumaRemota.java SumaRemotaImpl.java ServidorRMI.java
   
   REM Inicia el registro RMI en una ventana separada:
   start rmiregistry 1099
   
   REM Inicia el servidor:
   java ServidorRMI
   ```

3. Abre el firewall:
   ```cmd
   netsh advfirewall firewall add rule name="RMI_1099" dir=in action=allow protocol=TCP localport=1099
   ```

### En Windows 10 (cliente):

1. Instala JDK igual que arriba.

2. Copia SumaRemota.java al cliente y compila:
   ```cmd
   cd rmi\cliente
   javac SumaRemota.java ClienteRMI.java
   java ClienteRMI 192.168.1.101
   ```
   (Cambia la IP por la de tu Windows Server)

---

## PRÁCTICA 3 — DCOM (.NET Remoting)

**Servidor: Windows Server 2025 | Cliente: Windows 10**

### En Windows Server 2025 (servidor):

1. Instala .NET Framework 4.8 (ya suele venir instalado):
   ```powershell
   # Verificar:
   Get-WindowsFeature NET-Framework-45-Core
   ```

2. Compila y ejecuta:
   ```powershell
   cd dcom\servidor
   # Si usas Visual Studio: abre el .csproj y corre
   # Si usas dotnet CLI:
   dotnet run
   ```

3. Abre el firewall:
   ```cmd
   netsh advfirewall firewall add rule name="DCOM_9090" dir=in action=allow protocol=TCP localport=9090
   ```

### En Windows 10 (cliente):

```cmd
pip install pywin32

cd dcom\cliente
REM Edita cliente_dcom.py y cambia IP_SERVIDOR
python cliente_dcom.py
```

---

## Solución de Problemas Comunes

| Error | Causa | Solución |
|---|---|---|
| Connection refused | Firewall bloqueando | Agrega regla en firewall |
| rmiregistry no encontrado | Java no en PATH | `set PATH=%PATH%;C:\Program Files\Java\jdk\bin` |
| ModuleNotFoundError win32com | pywin32 no instalado | `pip install pywin32` |
| No route to host | VMs en redes distintas | Pon las VMs en modo "Red interna" o "Puente" |

---

## Cómo tomar evidencia (capturas de pantalla)

Para cada protocolo, captura:
1. **Servidor iniciado** — pantalla del servidor mostrando que está escuchando
2. **Cliente ejecutado** — pantalla del cliente mostrando los resultados
3. **Comunicación exitosa** — idealmente ambas ventanas simultáneas

En Windows: `Win + Shift + S` para captura de área.
En Debian: `scrot captura.png` (instalar con `apt install scrot`)
