using Microsoft.Data.SqlClient;

namespace AuthLibrary.Data
{
    public class SqlConnectionFactory(string conn)
    {
        public async Task<SqlConnection> CreateAsync()
        {
            var c = new SqlConnection(conn);
            await c.OpenAsync();
            return c;
        }
    }

}
