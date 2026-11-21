namespace VentasDemo.Web.Models;

public sealed record SaleDetails(Sale Sale, IReadOnlyList<SaleConcept> Concepts);
