from xmlrpc.server import SimpleXMLRPCServer
import socket

def sumaRemota(a, b):
    resultado = a + b
    print(f"  [RPC] sumaRemota({a}, {b}) → \"{resultado}\"")
    return str(resultado)

HOST = "0.0.0.0"
PORT = 8080

server = SimpleXMLRPCServer((HOST, PORT), allow_none=True, logRequests=False)

server.socket.setsockopt(socket.IPPROTO_TCP, socket.TCP_NODELAY, 1)

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