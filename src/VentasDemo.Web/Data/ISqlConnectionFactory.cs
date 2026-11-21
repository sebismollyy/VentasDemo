using Microsoft.Data.SqlClient;

namespace VentasDemo.Web.Data;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}
