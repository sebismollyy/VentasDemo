using Microsoft.Data.SqlClient;
using VentasDemo.Web.Data;
using VentasDemo.Web.Models;

namespace VentasDemo.Web.Services;

public sealed class SalesService(ISaleRepository repository, ILogger<SalesService> logger) : ISalesService
{
    public async Task<NewSaleResult> CreateAsync(
        NewSaleRequest sale, NewSaleConcept concept, CancellationToken cancellationToken = default)
    {
        // Deliberadamente se guardan por separado para estudiar la falta de atomicidad.
        var saleId = await repository.CreateSaleAsync(sale, cancellationToken);

        try
        {
            var conceptId = await repository.AddConceptAsync(saleId, concept, cancellationToken);
            return new NewSaleResult(saleId, conceptId, null);
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex, "No se pudo guardar el concepto de la venta {SaleId}.", saleId);
            return new NewSaleResult(saleId, null,
                "SQL Server rechazó el concepto. La cabecera de la venta quedó guardada sin conceptos.");
        }
    }
}
