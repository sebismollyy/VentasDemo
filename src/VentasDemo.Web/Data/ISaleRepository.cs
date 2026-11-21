using VentasDemo.Web.Models;

namespace VentasDemo.Web.Data;

public interface ISaleRepository
{
    Task<IReadOnlyList<Sale>> GetRecentAsync(CancellationToken cancellationToken = default);
    Task<SaleDetails?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateSaleAsync(NewSaleRequest sale, CancellationToken cancellationToken = default);
    Task<int> AddConceptAsync(int saleId, NewSaleConcept concept, CancellationToken cancellationToken = default);
}
