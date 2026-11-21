using Microsoft.Data.SqlClient;
using System.Data;
using VentasDemo.Web.Models;

namespace VentasDemo.Web.Data;

public sealed class SaleRepository(ISqlConnectionFactory connectionFactory) : ISaleRepository
{
    public async Task<IReadOnlyList<Sale>> GetRecentAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT TOP (50) s.SaleId, s.CustomerName, s.CreatedAt,
                   COALESCE(SUM(c.Quantity * c.UnitPrice), 0) AS Total,
                   COUNT(c.SaleConceptId) AS ConceptCount
            FROM dbo.Sales AS s
            LEFT JOIN dbo.SaleConcepts AS c ON c.SaleId = s.SaleId
            GROUP BY s.SaleId, s.CustomerName, s.CreatedAt
            ORDER BY s.CreatedAt DESC, s.SaleId DESC;
            """;

        var sales = new List<Sale>();
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            sales.Add(new Sale(
                reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2),
                reader.GetDecimal(3), reader.GetInt32(4)));
        }

        return sales;
    }

    public async Task<SaleDetails?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        const string saleSql = """
            SELECT s.SaleId, s.CustomerName, s.CreatedAt,
                   COALESCE(SUM(c.Quantity * c.UnitPrice), 0) AS Total,
                   COUNT(c.SaleConceptId) AS ConceptCount
            FROM dbo.Sales AS s
            LEFT JOIN dbo.SaleConcepts AS c ON c.SaleId = s.SaleId
            WHERE s.SaleId = @SaleId
            GROUP BY s.SaleId, s.CustomerName, s.CreatedAt;
            """;
        const string conceptsSql = """
            SELECT SaleConceptId, SaleId, ProductName, Quantity, UnitPrice
            FROM dbo.SaleConcepts
            WHERE SaleId = @SaleId
            ORDER BY SaleConceptId;
            """;

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        Sale? sale = null;
        await using (var command = new SqlCommand(saleSql, connection))
        {
            command.Parameters.Add("@SaleId", SqlDbType.Int).Value = id;
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (await reader.ReadAsync(cancellationToken))
            {
                sale = new Sale(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2),
                    reader.GetDecimal(3), reader.GetInt32(4));
            }
        }

        if (sale is null) return null;

        var concepts = new List<SaleConcept>();
        await using (var command = new SqlCommand(conceptsSql, connection))
        {
            command.Parameters.Add("@SaleId", SqlDbType.Int).Value = id;
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                concepts.Add(new SaleConcept(reader.GetInt32(0), reader.GetInt32(1),
                    reader.GetString(2), reader.GetInt32(3), reader.GetDecimal(4)));
            }
        }

        return new SaleDetails(sale, concepts);
    }

    public async Task<int> CreateSaleAsync(NewSaleRequest sale, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO dbo.Sales (CustomerName)
            OUTPUT INSERTED.SaleId
            VALUES (@CustomerName);
            """;
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 120).Value = sale.CustomerName;
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }

    public async Task<int> AddConceptAsync(int saleId, NewSaleConcept concept, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO dbo.SaleConcepts (SaleId, ProductName, Quantity, UnitPrice)
            OUTPUT INSERTED.SaleConceptId
            VALUES (@SaleId, @ProductName, @Quantity, @UnitPrice);
            """;
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@SaleId", SqlDbType.Int).Value = saleId;
        command.Parameters.Add("@ProductName", SqlDbType.NVarChar, 160).Value = concept.ProductName;
        command.Parameters.Add("@Quantity", SqlDbType.Int).Value = concept.Quantity;
        command.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = concept.UnitPrice;
        command.Parameters["@UnitPrice"].Precision = 12;
        command.Parameters["@UnitPrice"].Scale = 2;
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }
}
