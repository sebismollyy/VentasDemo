# Documentación

Esta carpeta concentra la documentación funcional y técnica del proyecto.

- [Arquitectura](arquitectura.md): capas, responsabilidades y decisiones de diseño.
- [Base de datos](base-de-datos.md): esquema, restricciones y configuración de conexión.
- [Escenario de integridad](escenario-integridad.md): pasos para provocar y observar la inserción parcial.
- [Ejecución](ejecucion.md): requisitos y comandos para iniciar la aplicación.

El esquema se encuentra en `database/`. Usa [`02-ensure-schema.sql`](../database/02-ensure-schema.sql) para crear lo que falte sin borrar datos. `01-create-database.sql` recrea las tablas para reiniciar el entorno de práctica.
