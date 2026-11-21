namespace VentasDemo.Web.Models;

public sealed record SaleConcept(
    int Id,
    int SaleId,
    string ProductName,
    int Quantity,
    decimal UnitPrice)
{
    public decimal Subtotal => Quantity * UnitPrice;
}
