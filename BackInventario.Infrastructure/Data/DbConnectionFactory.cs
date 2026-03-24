using System.Data;
using Microsoft.Data.SqlClient;

namespace BackInventario.Infrastructure.Data;

public class DbConnectionFactory(string connectionString)
{
    public IDbConnection CreateConnection() => new SqlConnection(connectionString);
}
