using VentasDemo.Web.Models;

namespace VentasDemo.Web.Services;

public interface ISalesService
{
    Task<NewSaleResult> CreateAsync(NewSaleRequest sale, NewSaleConcept concept, CancellationToken cancellationToken = default);
}

public sealed record NewSaleResult(int SaleId, int? ConceptId, string? ErrorMessage)
{
    public bool IsComplete => ConceptId.HasValue;
}
