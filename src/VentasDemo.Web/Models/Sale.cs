namespace VentasDemo.Web.Models;

public sealed record Sale(
    int Id,
    string CustomerName,
    DateTime CreatedAt,
    decimal Total,
    int ConceptCount);
