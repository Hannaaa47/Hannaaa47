"""
=============================================================
  PRÁCTICA — DCOM
  CLIENTE — Python
  Máquina: Windows 10
=============================================================

INSTALACIÓN (en Windows 10, PowerShell o CMD):
  pip install pywin32

EJECUCIÓN:
  python cliente_dcom.py

IMPORTANTE: Cambia IP_SERVIDOR por la IP de Windows Server 2025.

¿Por qué pywin32?
  pywin32 es la librería estándar para acceder a COM/DCOM
  desde Python en Windows. Permite instanciar objetos COM
  remotos exactamente como lo haría un cliente DCOM nativo.
=============================================================
"""

import sys

# ── Importar win32com (pywin32) ───────────────────────────
try:
    import win32com.client
except ImportError:
    print("[ERROR] Falta pywin32.")
    print("        Instálalo con:  pip install pywin32")
    sys.exit(1)

# ── Importar pythoncom para conexión remota ───────────────
try:
    import pythoncom
except ImportError:
    print("[ERROR] Falta pythoncom (parte de pywin32).")
    sys.exit(1)

# ── CAMBIA ESTA IP por la de tu Windows Server 2025 ───────
IP_SERVIDOR = "192.168.1.101"
PUERTO      = 9090
# ──────────────────────────────────────────────────────────

print("=" * 55)
print("  CLIENTE DCOM — Python / win32com")
print("=" * 55)
print(f"  Servidor  : {IP_SERVIDOR}:{PUERTO}")
print(f"  Protocolo : DCOM / .NET Remoting TCP")
print("=" * 55)

try:
    # Conectar al objeto remoto via DCOM
    # La URI del objeto .NET Remoting
    uri = f"tcp://{IP_SERVIDOR}:{PUERTO}/ServicioDCOM"

    # Usando win32com.client.Dispatch para conectar vía COM
    # Si el servidor .NET Remoting está registrado como COM:
    servicio = win32com.client.Dispatch(
        "ServicioDCOM.ServicioDCOM",
        clsctx=pythoncom.CLSCTX_REMOTE_SERVER
    )

    # Casos de prueba
    casos = [(3, 4), (10, 25), (-5, 15), (100, 200)]

    for a, b in casos:
        resultado = servicio.SumaRemota(a, b)
        print(f"  SumaRemota({a:4}, {b:4})  -->  \"{resultado}\"")

    print("=" * 55)
    print("  [OK] Comunicación DCOM exitosa")

except Exception as e:
    print(f"\n[ERROR] {e}")
    print("\nSi aparece un error de COM, asegúrate de que:")
    print("  1. El servidor .NET está ejecutándose en Windows Server 2025")
    print("  2. La IP es correcta")
    print("  3. El puerto 9090 está abierto en el firewall")
    print("  4. DCOM está habilitado en ambas máquinas (dcomcnfg)")

input("\nPresiona ENTER para salir...")
