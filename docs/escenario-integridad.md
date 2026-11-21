# Escenario de integridad: venta parcial

## Propósito

Observar que dos escrituras independientes no son atómicas. El código actual no implementa transacciones intencionalmente; la solución transaccional se realizará en una etapa posterior.

## Reproducir

1. Inicia SQL Server, ejecuta `database/02-ensure-schema.sql` y configura la conexión.
2. Abre **Nueva venta**.
3. Captura cliente y producto válidos.
4. Marca **Simular fallo del concepto** y envía el formulario. La aplicación insertará la cabecera con los datos válidos y usará cantidad `0` para el concepto.
5. El `CHECK` `CK_SaleConcepts_Quantity_Positive` rechaza la segunda escritura después de haber insertado la venta.
6. La pantalla informa que la inserción fue parcial. Abre el enlace de la venta y observa la cabecera sin conceptos.

También puedes reproducir el fallo manualmente ingresando cantidad `0` o precio negativo. No uses nombres vacíos: la validación HTML puede impedir el envío antes de llegar al servidor.

## Orden de operaciones

1. `SalesService` solicita `CreateSaleAsync`.
2. `SaleRepository` abre una conexión e inserta `Sales`. Al completarse, esa sentencia queda confirmada por SQL Server.
3. El servicio solicita `AddConceptAsync`.
4. El repositorio abre otra conexión e intenta insertar `SaleConcepts`.
5. La restricción rechaza el comando. El servicio devuelve el ID de cabecera y un mensaje que deja visible el estado parcial.

El resultado esperado es una fila en `Sales` y cero filas relacionadas en `SaleConcepts`. Verificación opcional:

```sql
SELECT s.SaleId, s.CustomerName, COUNT(c.SaleConceptId) AS Conceptos
FROM dbo.Sales AS s
LEFT JOIN dbo.SaleConcepts AS c ON c.SaleId = s.SaleId
GROUP BY s.SaleId, s.CustomerName
ORDER BY s.SaleId DESC;
```

## Qué se estudiará después

La corrección conceptual es hacer que el alta de cabecera y conceptos sea una unidad atómica, con `SqlTransaction`, `Commit` al completar y `Rollback` ante un error. Este proyecto aún no contiene ese código ni una opción de alternarlo.
