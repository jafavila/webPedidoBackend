# webPedido > webPedidoBackend

Sistema de pedidos minimalista basado.

Proyecto webPedidoFrontend https://github.com/jafavila/webPedidoFrontend

## Archivos del Proyecto
- ventaWeb.txt: Definición de requerimientos, estructura de vistas y reglas de acceso.
- ventaWebPostGresSql.txt: Script SQL con tablas, roles, permisos y procedimientos almacenados (SPs).

## Stack
- Frontend: React (Vite, JSX).
- Backend: .NET 10 (C#).
- Base de Datos: PostgreSQL 18.

## Configuración DB
- DB: webPedido
- Usuario: usrWebPedido
- Password: MiWebPedido
- Seguridad: Login con MD5 y bloqueo automático tras 3 intentos fallidos.

## Vistas y Accesos
- Login: Acceso público.
- Comprar / Carrito: Solo clientes autenticados.
- Mis Pedidos: Clientes (ver solo lo propio) y Admin (ver todo).
- Catálogos (Productos/Clientes): Solo usuarios Administradores.

## Lógica de Persistencia
Todo el mantenimiento se realiza mediante Procedimientos Almacenados:
- spMttoCatCliente
- spMttoCatProducto
- spMttoTblPedido
- spMttoTblPedidoDetalle (incluye validación de stock)

## Instalación
1. Ejecutar ventaWebPostGresSql.txt en la instancia de PostgreSQL.
2. Configurar el ConnectionString en el backend .NET.

3. En la carpeta frontend: npm install && npm run dev.
