import socket
import sys

IP_SERVIDOR = "192.168.100.136"  # Cambia por la IP de tu Windows Server
PUERTO      = 9090

print("=" * 55)
print("  CLIENTE DCOM — Python / TCP")
print("=" * 55)
print(f"  Servidor  : {IP_SERVIDOR}:{PUERTO}")
print(f"  Protocolo : TCP directo")
print("=" * 55)

casos = [(3, 4), (10, 25), (-5, 15), (100, 200)]

try:
    for a, b in casos:
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect((IP_SERVIDOR, PUERTO))
            mensaje = f"{a},{b}\n"
            s.sendall(mensaje.encode())
            respuesta = s.recv(1024).decode().strip()
            print(f"  SumaRemota({a:4}, {b:4})  -->  \"{respuesta}\"")

    print("=" * 55)
    print("  [OK] Comunicación exitosa")

except Exception as e:
    print(f"\n[ERROR] {e}")

input("\nPresiona ENTER para salir...")