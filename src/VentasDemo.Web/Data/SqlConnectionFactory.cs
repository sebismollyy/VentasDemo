using Microsoft.Data.SqlClient;

namespace VentasDemo.Web.Data;

public sealed class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
{
    public SqlConnection CreateConnection()
    {
        var connectionString = configuration.GetConnectionString("VentasDb");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Falta configurar la cadena de conexión 'VentasDb'.");
        }

        return new SqlConnection(connectionString);
    }
}
