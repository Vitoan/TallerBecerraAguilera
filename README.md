# 🔧 Proyecto Taller Mecánico – Gestión Operativa y Administrativa

![ASP.NET](https://img.shields.io/badge/Backend-ASP.NET%20Core-blue?logo=dotnet)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-blue?logo=microsoftsqlserver)
![Bootstrap](https://img.shields.io/badge/Frontend-Bootstrap%205-purple?logo=bootstrap)
![Vue.js](https://img.shields.io/badge/Frontend-Vue.js-green?logo=vue.js)
![JWT](https://img.shields.io/badge/Seguridad-JWT-orange?logo=jsonwebtokens)
![License](https://img.shields.io/badge/License-MIT-green)
![Estado](https://img.shields.io/badge/Estado-En%20Desarrollo-yellow)

---

## 📘 Descripción

El **Proyecto Taller Mecánico** es una aplicación web desarrollada con **ASP.NET Core MVC** que informatiza la **gestión operativa y administrativa** de un taller mecánico automotor.

Permite administrar **empleados, vehículos, clientes, órdenes de trabajo (OT), herramientas, repuestos, proveedores y pedidos**, con control de inventario, auditoría de movimientos, generación de informes y funcionalidades avanzadas como préstamos de herramientas y descuentos automáticos de stock.

El sistema soporta **roles diferenciados** (Administrador y Empleado) para restringir accesos, y utiliza **JWT para una API REST** que expone endpoints para consultas y operaciones CRUD externas (probable integración con apps móviles o Postman para pruebas (probable)).

---

## 🚀 Tecnologías utilizadas

| Tipo              | Tecnología                          |
|-------------------|-------------------------------------|
| Lenguaje          | C# (.NET 8 LTS)                     |
| Framework Backend | ASP.NET Core MVC                    |
| Base de datos     | SQL Server                          |
| Frontend          | HTML5, CSS3, Bootstrap 5, Razor Views |
| Frontend Avanzado | Vue.js (para ABM específico)        |
| Seguridad         | Autenticación JWT + Roles (Authorize) |
| Archivos          | Upload de avatares y documentos (e.g., facturas de pedidos) |
| Control de versiones | Git + GitHub                    |

---

## 🧩 Funcionalidades principales

- 👥 **ABM de Empleados, Clientes, Vehículos, Proveedores y Repuestos** (CRUD completo con validaciones).
- 🛠️ **Gestión de Órdenes de Trabajo (OT)**: Creación, asignación de mecánicos, registro de tareas, repuestos y herramientas; actualización de estados (pendiente, en reparación, finalizado, entregado).
- 🔧 **Préstamo y Control de Herramientas**: Solicitud, retiro/devolución con timestamps, y marcado de estados (disponible, en uso, en mantenimiento).
- 📦 **Inventario de Repuestos**: Catálogo con filtros, descuento automático al asociar a OT, alertas de stock bajo y generación de pedidos de reposición.
- 🧾 **Pedidos a Proveedores**: Creación, aprobación por admin, recepción y actualización de stock.
- 📊 **Informes y Consultas**: Listados por estado, historial por vehículo, productividad por empleado, stock bajo y auditoría de movimientos.
- 🔐 **Autenticación y Autorización**: Login por email/contraseña con avatares; funcionalidades restringidas por rol (e.g., admins gestionan proveedores, empleados solo OTs).
- 🕵️ **Auditoría**: Registro de CUD (Create, Update, Delete) por usuario en todas las entidades.
- 🌐 **API con JWT**: Endpoints protegidos para OT y repuestos (e.g., GET /api/ots, POST /api/repuestos con token).

---

## ⚙️ Roles del sistema

### 👑 **Administrador**
- Gestión completa de empleados, proveedores, herramientas y repuestos.
- Aprobación/recepción de pedidos y actualización de stock.
- Acceso a auditoría, informes y configuración de roles.
- Upload de archivos (e.g., manuales de herramientas).

### 🧑‍🔧 **Empleado**
- Registro de clientes/vehículos y creación/asignación de OTs.
- Solicitud/devolución de herramientas y consulta de repuestos.
- Generación de pedidos (sujeta a aprobación).
- Acceso restringido a informes básicos.

---

## 🔄 Flujo principal de una Orden de Trabajo

1. **Recepción**: Registrar vehículo/cliente y crear OT (descripción de falla, fecha estimada).
2. **Asignación**: Seleccionar mecánico (búsqueda AJAX) y registrar tareas/repuestos/herramientas.
3. **Ejecución**: Préstamo de herramientas (AJAX), descuento de stock al usar repuestos.
4. **Cierre**: Actualizar estado, finalizar y entregar (auditoría automática).
5. **Post-entrega**: Generar informe de historial o productividad.

---

## 📋 Informes y consultas disponibles

| Informe                  | Descripción                                      |
|--------------------------|--------------------------------------------------|
| Órdenes por estado       | Filtrado por pendiente/en reparación/finalizada/entregada (paginado server-side). |
| Herramientas por estado  | Disponible/en uso/en mantenimiento; quién la tiene. |
| Stock bajo               | Repuestos < mínimo; alertas y pedidos pendientes. |
| Pedidos por fecha/proveedor | Estados: pendiente/recibido/cancelado.          |
| Historial de vehículo    | Todas las OTs y reparaciones (búsqueda por patente). |
| Productividad empleado   | Cantidad de OTs finalizadas en período (AJAX).   |

---

## 📊 Diagrama de Entidad-Relación (ERD)

El modelo de datos incluye **8 entidades principales** con relaciones 1:N y N:N:

- **Cliente (1) → Vehículo (N)**: Un cliente tiene múltiples vehículos.
- **Vehículo (1) → OT (N)**: Múltiples OTs por vehículo.
- **Empleado (1) → OT (N)**: Mecánico asignado a OTs.
- **OT (N) → Repuesto (N)** (junction table): Repuestos usados en OT (descuento stock).
- **OT (N) → Herramienta (N)** (junction): Herramientas prestadas.
- **Repuesto (N) → Pedido (N)**: Repuestos solicitados en pedidos.
- **Proveedor (1) → Pedido (N)**: Pedidos por proveedor.
- **Usuario (1) → Empleado (1)**: Autenticación ligada a empleados (roles).

![ERD Diagram](https://github.com/Vitoan/TallerBecerraAguilera/blob/main/docs/ERD_TallerMecanico.png)  
*(Diagrama generado con xampp diseñador).*

Relaciones clave: Al menos 4 tablas con 1:N (e.g., Cliente-Vehículo, Vehículo-OT).


## 🧠 Cómo ejecutar el proyecto (Futuro)

### 🪜 1. Clonar el repositorio
```bash
git clone https://github.com/Vitoan/TallerBecerraAguilera.git
cd TallerBecerraAguilera
```

### 🧩 2. Crear la base de datos en SQL 
```sql
CREATE DATABASE taller_mecanico_db;

USE taller_mecanico_db;
```
- Importar `/Database/taller_mecanico_db.sql` .

### ⚙️ 3. Configurar conexión en `appsettings.json` (Futuro)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TallerMecanicoDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyHereAtLeast32Chars",
    "Issuer": "TallerApp",
    "Audience": "TallerUsers"
  }
}
```


### ▶️ 4. Ejecutar el proyecto (Futuro)
Desde terminal:
```bash
dotnet run
```
O desde Visual Studio: Presionar **F5** → “Iniciar depuración”.

### 🌐 5. Acceder desde el navegador
```
https://localhost:7001/  # O puerto asignado
```
- Login inicial: Ver usuarios abajo.

---

## 👤 Usuarios de Prueba (para Roles)

| Rol          | Email                  | Contraseña | Notas |
|--------------|------------------------|------------|-------|
| **Admin**    | admin@taller.com      | Admin123! | Acceso total, avatar por default. |
| **Empleado** | mecanico@taller.com   | Mec123!   | Solo OTs y herramientas; restricciones en admins. |

*(Registrados en seed data; avatares en /wwwroot/uploads/avatars/)*

---

## 🔑 API con JWT - Pruebas en Postman (Futuro)

- **Colección**: `/docs/TallerAPI.postman_collection.json` (importar en Postman).
- **Autenticación**: POST `/api/auth/login` → Obtener token JWT.
- **Ejemplos**:
  - GET `/api/ots` (con Bearer Token): Lista OTs paginadas.
  - POST `/api/repuestos` (con Token, rol Empleado+): Crear repuesto.
- **Variables Postman**: `baseUrl: https://localhost:7001`, `token: {{jwt_token}}`.

---

## 👥 Autores

**Victor Angel Aguilera y Martin Becerra**  
📚 Proyecto académico – *Programación Web 2 (2025)*  
🔗 [GitHub Repo](https://github.com/Vitoan/TallerBecerraAguilera)  
✉️ Contacto: vitoan@proton.me | martinbecerrasl7@gmail.com 

---

> ✨ *“Un auto con buen mantenimiento es un auto seguro y seguro lo llevan al Taller BA ”*
