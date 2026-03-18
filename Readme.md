# PAM API- Clean Architecture (.NET)

API desarrollada en .NET utilizando Clean Architecture.

---

## Tecnologías

- .NET 10
- ASP.NET Core Web API
- Clean Architecture
- MySQL
- JWT Authentication
- MediatR (CQRS)
- Background Jobs

---

## Estructura del Proyecto

PUI.Api            → Capa de presentación (Controllers, Middlewares)  
PUI.Application    → Casos de uso, interfaces, lógica de negocio  
PUI.Domain         → Entidades, enums, value objects  
PUI.Infrastructure → Servicios externos, jobs, integraciones  
PUI.Persistence    → Acceso a datos (repositorios, UnitOfWork)  
PUI.Identity       → Autenticación, JWT, servicios de identidad  

---

## Arquitectura

- Domain: Núcleo del negocio (sin dependencias externas)
- Application: Casos de uso y lógica de aplicación
- Infrastructure: Integraciones externas
- Persistence: Acceso a base de datos
- Api: Exposición HTTP

---

## Configuración

### 1. Clonar repositorio

git clone <repo-url>
cd pui-api

---

### 2. Ejemplo de configuración de appsettings.Development.json

{
  "ConnectionStrings": {
    "PuiConnectionString": "Server=HOST;Port=3306;User ID=USER;Password=PASSWORD;Database=DB;SSL Mode=Required;Certificate File=RUTA_CERT;Certificate Password=PASSWORD_CERT"
  },
  "Jwt": {
    "Secret": "CLAVE_SUPER_LARGA_Y_SEGURA",
    "Issuer": "PUI",
    "Audience": "PUI.Api",
    "ExpireMinutes": 80
  },
  "Jobs": {
    "BusquedaContinua": {
      "HoraEjecucion": 13,
      "MinutoEjecucion": 30,
      "DelayHorasEjecucion": 0,
      "DelayMinutosEjecucion": 2
    }
  },
  "ApiPui": {
    "Credenciales": {
      "InstitucionId": "1234",
      "Clave": "clave123"
    },
    "BaseUrl": "http://localhost:3000",
    "Endpoints": {
      "Login": "/api/2_3_0/login",
      "ListarReportes": "/api/2_3_0/reportes",
      "NotificarCoincidencia": "/api/2_3_0/notificar-coincidencia",
      "BusquedaFinalizada": "/api/2_3_0/busqueda-finalizada"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "origenesPermitidos": [ "http://localhost:5131", "http://127.0.0.1:5500" ]
}

---

## Variables importantes

### Base de datos

- Server: IP o dominio del servidor MySQL
- Database: nombre de la base de datos
- SSL Mode: requerido si usas certificados
- Certificate File: ruta del archivo .pfx

### JWT

- Secret: clave secreta (debe cambiarse en producción)
- ExpireMinutes: duración del token

### API externa

Configuración de endpoints externos consumidos por la aplicación.

---

## Ejecución del proyecto

### Desde Visual Studio

- Establecer PUI.Api como proyecto de inicio
- Ejecutar con F5

### Desde CLI

dotnet restore
dotnet build
dotnet run --project PUI.Api

---

## Jobs (Background Tasks)

Ubicación:

PUI.Infrastructure/Jobs

Configuración:

"Jobs": {
  "BusquedaContinua": {
    "HoraEjecucion": 13,
    "MinutoEjecucion": 30
  }
}

---

## Patrones implementados

- CQRS (Commands / Queries)
- Repository Pattern
- Unit of Work
- Dependency Injection
- Middleware personalizado
- Manejo centralizado de excepciones

---
