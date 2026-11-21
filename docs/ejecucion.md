# Ejecución

## Requisitos

- .NET SDK 10.
- SQL Server local o accesible desde el equipo.
- Ejecutar `database/02-ensure-schema.sql` y configurar `VentasDb`.

## Comandos

Desde la raíz de la solución:

```powershell
dotnet restore .\VentasDemo.sln
dotnet run --project .\src\VentasDemo.Web\VentasDemo.Web.csproj
```

Usa la URL HTTPS que muestre ASP.NET Core. El perfil de ejecución del proyecto establece el entorno `Development`, donde ASP.NET Core carga User Secrets. Guarda la conexión local con autenticación integrada de Windows así:

```powershell
dotnet user-secrets set "ConnectionStrings:VentasDb" "Server=localhost;Database=VentasDemo;Trusted_Connection=True;TrustServerCertificate=True;" --project .\src\VentasDemo.Web\VentasDemo.Web.csproj
```

User Secrets mantiene este valor fuera del repositorio, en el perfil del usuario. No cifra los datos y es solo para desarrollo local, no para producción. Si SSMS usa otra instancia, cambia `Server=localhost` por el nombre exacto de esa instancia. Como alternativa temporal para una sesión de PowerShell, define `ConnectionStrings__VentasDb` como variable de entorno.

La primera solicitud de páginas que consultan SQL Server requiere que la base y las tablas ya existan. Si la base no está disponible, el panel mostrará un mensaje de conexión.
