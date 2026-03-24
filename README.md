# BackInventario — API REST

Backend del sistema de gestión de inventario. Desarrollado con **.NET 10** y **SQL Server**, expone una API REST consumida por el frontend Angular.

> **Repositorio frontend:** [front_inventario](https://github.com/tu-usuario/front_inventario)

---

## Tecnologías

- .NET 10 / ASP.NET Core
- Dapper + SQL Server (Stored Procedures)
- Autenticación JWT
- SignalR (notificaciones en tiempo real)
- xUnit + Moq (pruebas unitarias)

---

## Requisitos previos

| Herramienta | Versión mínima |
|---|---|
| .NET SDK | 10 |
| SQL Server | 2019 o superior |

---

## Configuración

### 1. Base de datos

Crea la base de datos en SQL Server y ejecuta el script de configuración:

```sql
CREATE DATABASE db_tienda;
```

```bash
# Ejecutar en SQL Server Management Studio o sqlcmd
db_setup.sql
```

El script crea tablas, stored procedures y datos iniciales de prueba.

### 2. Cadena de conexión

Edita `BackInventario.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=db_tienda;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Si usas usuario y contraseña en lugar de autenticación de Windows:

```json
"DefaultConnection": "Server=localhost;Database=db_tienda;User Id=sa;Password=tu_password;TrustServerCertificate=True;"
```

---

## Ejecutar

```bash
dotnet run --project BackInventario.API
```

La API quedará disponible en `http://localhost:5000`.

---

## Pruebas unitarias

```bash
dotnet test BackInventario.Tests
```

63 pruebas cubren la capa de servicios y controladores.

---

## Estructura del proyecto

```
BackInventario/
├── BackInventario.API/           # Controladores, Program.cs, JWT, SignalR Hub
├── BackInventario.Application/   # Servicios, interfaces, DTOs, validaciones
├── BackInventario.Infrastructure/# Repositorios con Dapper
├── BackInventario.Domain/        # Entidades
├── BackInventario.Tests/         # Pruebas unitarias (xUnit + Moq)
└── db_setup.sql                  # Script completo de base de datos
```

---

## Endpoints principales

| Método | Ruta | Rol requerido |
|---|---|---|
| POST | `/api/auth/login` | Público |
| GET | `/api/productos` | Autenticado |
| POST | `/api/productos` | ADMINISTRADOR |
| PATCH | `/api/productos/{id}/estado` | ADMINISTRADOR |
| GET | `/api/categorias` | Autenticado |
| POST | `/api/categorias` | ADMINISTRADOR |
| GET | `/api/usuarios` | ADMINISTRADOR |
| POST | `/api/usuarios` | ADMINISTRADOR |
| GET | `/api/reportes/resumen` | ADMINISTRADOR |
| GET | `/api/reportes/bajo-stock` | Autenticado |
| POST | `/api/notificaciones/reportar` | EMPLEADO |

---

## Credenciales de prueba

| Rol | Correo | Contraseña |
|---|---|---|
| Administrador | admin@inventario.com | Admin123 |
| Empleado | empleado@inventario.com | Empleado123 |

---

## Seguridad implementada

- Autenticación con **JWT** (expiración configurable)
- Autorización por **roles** (`ADMINISTRADOR`, `EMPLEADO`)
- **Rate limiting** en login: máximo 5 intentos por minuto por IP
- Validación de entradas con **Data Annotations** en todos los DTOs
- Protección contra **inyección SQL** mediante Stored Procedures parametrizados
- **Headers de seguridad** en todas las respuestas (`X-Frame-Options`, `X-Content-Type-Options`, etc.)
- Contraseñas almacenadas con hash **SHA2-256**

---

## Relación con el frontend

Este backend es consumido por **[front_inventario](https://github.com/tu-usuario/front_inventario)**, una aplicación Angular 21 que debe estar corriendo en `http://localhost:4200`. El CORS está configurado para aceptar únicamente ese origen.

La comunicación en tiempo real entre ambos se realiza mediante **SignalR** en el endpoint `/hubs/notificaciones`.
