# Arquitectura

## Resumen

La solución contiene una aplicación Blazor Web App en modo **Static Server-Side Rendering (SSR)**. Los componentes producen HTML desde el servidor; los formularios usan POST de Blazor SSR y no necesitan WebAssembly, SignalR ni cuentas de usuario.

```text
Components (Razor SSR)
        ↓
ISalesService / SalesService
        ↓
ISaleRepository / SaleRepository (ADO.NET)
        ↓
ISqlConnectionFactory
        ↓
SQL Server
```

## Organización

- `Components/`: rutas, páginas y layout visual.
- `Data/`: contratos del repositorio, implementación ADO.NET y fábrica de conexiones.
- `Models/`: modelos de lectura y comandos simples.
- `Services/`: orquestación del caso de uso de creación de una venta.
- `wwwroot/`: estilos de la interfaz.
- `database/`: script repetible para crear el esquema.
- `docs/`: documentación del proyecto.

## Repository y ADO.NET

Las páginas dependen del contrato `ISaleRepository`, no de `SqlConnection` ni de SQL. `SaleRepository` concentra las consultas parametrizadas de SQL Server. `ISqlConnectionFactory` mantiene la configuración de conexión fuera del repositorio y permite sustituirla en otros contextos.

Cada operación abre y dispone su propia conexión con `await using`. No se comparte una conexión global ni se mantiene una conexión abierta entre solicitudes. Los parámetros tienen tipos y tamaños declarados para evitar concatenar entrada del usuario en SQL.

## Responsabilidades y principios

- **SRP:** UI, orquestación y acceso a datos tienen responsabilidades distintas.
- **OCP/DIP:** el servicio recibe `ISaleRepository` y el repositorio recibe `ISqlConnectionFactory` mediante inyección de dependencias.
- **ISP:** contratos pequeños, enfocados en el caso de ventas.
- **DRY:** creación de conexiones, lectura SQL y estilos se mantienen centralizados.
- **KISS/YAGNI:** un solo proyecto web, sin capa genérica de repositorio, ORM, autenticación, API ni abstracciones no necesarias para la demostración.

## SSR y formulario

La aplicación no habilita render modes interactivos. El formulario de venta declara `FormName`, usa antiforgery y recibe los datos mediante `[SupplyParameterFromForm]`, como corresponde a un formulario SSR procesado en servidor.

## Decisión pedagógica

`SalesService.CreateAsync` inserta la cabecera y luego el concepto mediante dos métodos separados del repositorio. **No crea ni coordina una transacción.** Si falla la segunda sentencia por un `CHECK`, SQL Server conserva la primera sentencia ya confirmada. Este comportamiento es intencional para la práctica posterior.
