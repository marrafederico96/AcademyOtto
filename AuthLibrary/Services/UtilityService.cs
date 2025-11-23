using Microsoft.Data.SqlClient;
using System.Data;

namespace AuthLibrary.Services
{
    /// <summary>
    /// Provides utility methods for working with SQL parameters and related database operations.
    /// </summary>
    public static class UtilityService
    {
        public static SqlParameter CreateParam(string name, SqlDbType type, object? value, int size = 50)
        {
            object sqlValue = value switch
            {
                null => DBNull.Value,
                string s when string.IsNullOrWhiteSpace(s) => DBNull.Value,
                string s => s.Trim(),
                _ => value
            };

            return new SqlParameter(name, type, size) { Value = sqlValue };
        }
    }
}
