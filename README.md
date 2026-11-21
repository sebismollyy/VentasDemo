# Punto · Demo de ventas

Aplicación de práctica construida con **Blazor Web App en .NET 10**, Static Server-Side Rendering (SSR), SQL Server y ADO.NET. El acceso a datos usa el patrón Repository.

> **Propósito didáctico:** el guardado de una venta y su concepto se realiza en operaciones independientes y no implementa transacciones. Una restricción puede rechazar el concepto y dejar una cabecera sin detalle. Esa inconsistencia es intencional para estudiarla y corregirla en una etapa posterior.

## Inicio rápido

1. Revisa los [requisitos y comandos](docs/ejecucion.md).
2. Prepara la base con [`database/02-ensure-schema.sql`](database/02-ensure-schema.sql). Este script no borra datos.
3. Configura la cadena `VentasDb` en `src/VentasDemo.Web/appsettings.json`.
4. Ejecuta `dotnet restore .\VentasDemo.sln` y `dotnet run --project .\src\VentasDemo.Web\VentasDemo.Web.csproj`.

## Experiencia

- Resumen con ventas recientes, ingresos y conceptos persistidos.
- Formulario SSR para registrar cliente y primer concepto.
- Detalle que identifica ventas parciales sin conceptos.
- Interfaz adaptable a pantalla pequeña, sin autenticación.

## Estructura

```text
VentasDemo.sln
database/                  Esquema de SQL Server
docs/                      Documentación técnica y funcional
src/VentasDemo.Web/
  Components/              Rutas, páginas SSR y layout
  Data/                    Repository ADO.NET y conexión
  Models/                  Modelos de dominio y transferencia
  Services/                Caso de uso de creación de ventas
  wwwroot/                  Estilos
```

Consulta el [índice de documentación](docs/README.md), la [arquitectura](docs/arquitectura.md) y el [escenario de integridad](docs/escenario-integridad.md).
