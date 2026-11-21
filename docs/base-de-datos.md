# Base de datos

## Esquema

El script `database/02-ensure-schema.sql` crea la base `VentasDemo` y las tablas que falten, sin eliminar tablas ni datos. Para reiniciar completamente el entorno de práctica existe `database/01-create-database.sql`, que sí elimina y recrea las tablas; úsalo solo si quieres borrar sus datos.

- `Sales`: cabecera de la venta, cliente y fecha UTC.
- `SaleConcepts`: renglones asociados a la venta, cantidad y precio.

La FK `FK_SaleConcepts_Sales` protege la referencia a la venta. Los `CHECK` exigen nombre no vacío, cantidad positiva y precio no negativo. Los dos últimos permiten probar directamente restricciones de integridad.

## Preparar SQL Server

1. Ejecutar `database/02-ensure-schema.sql` en SQL Server Management Studio o `sqlcmd`.
2. Configurar `ConnectionStrings:VentasDb` con User Secrets (consulta [ejecución](ejecucion.md)) o proveer `ConnectionStrings__VentasDb` como variable de entorno.
3. Usar un principal de base de datos con permisos de lectura e inserción sobre las tablas.

La cadena local de ejemplo usa autenticación integrada de Windows: `Server=localhost;Database=VentasDemo;Trusted_Connection=True;TrustServerCertificate=True;`. Guárdala en User Secrets durante el desarrollo y ajusta `Server` para que coincida con la instancia de SQL Server. `TrustServerCertificate=True` es una comodidad local para desarrollo, no una recomendación de configuración de producción.

## Persistencia y lecturas

Los totales no se almacenan duplicados: se calculan desde los conceptos persistidos. Por ello una cabecera sin conceptos muestra total cero. Las lecturas del inicio muestran las últimas 50 ventas.
