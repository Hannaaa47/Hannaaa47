"""
=============================================================
  PRÁCTICA — RPC (XML-RPC)
  SERVIDOR — Python 3
  Máquina: Debian (sin interfaz gráfica)
  Puerto:  8080
=============================================================

INSTALACIÓN (en Debian):
  No requiere librerías externas. xmlrpc viene con Python 3.

EJECUCIÓN:
  python3 servidor_rpc.py

NOTA: El servidor escucha en 0.0.0.0:8080 para aceptar
      conexiones desde otras máquinas de la red.
=============================================================
"""

from xmlrpc.server import SimpleXMLRPCServer
import socket

# ── Función remota ────────────────────────────────────────
def sumaRemota(a, b):
    """Suma dos enteros y devuelve el resultado como string."""
    resultado = a + b
    print(f"  [RPC] sumaRemota({a}, {b}) → \"{resultado}\"")
    return str(resultado)

# ── Iniciar servidor ──────────────────────────────────────
HOST = "0.0.0.0"
PORT = 8080

server = SimpleXMLRPCServer((HOST, PORT), allow_none=True, logRequests=False)
server.register_function(sumaRemota, "sumaRemota")

ip_local = socket.gethostbyname(socket.gethostname())
print("=" * 55)
print("  SERVIDOR RPC — Python / XML-RPC")
print("=" * 55)
print(f"  IP local  : {ip_local}")
print(f"  Puerto    : {PORT}")
print(f"  Protocolo : XML-RPC sobre HTTP")
print(f"  Esperando clientes... (Ctrl+C para salir)")
print("=" * 55)

try:
    server.serve_forever()
except KeyboardInterrupt:
    print("\n[Servidor] Detenido.")
