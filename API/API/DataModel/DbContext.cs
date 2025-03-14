using Microsoft.Data.SqlClient;
using System.Data;

namespace API.DataModel
{
    public class DbContext
    {

        private readonly string _sqlConnectionString;

        public DbContext(string connectionString)
        {
            _sqlConnectionString = connectionString;    
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_sqlConnectionString);
        }
    }
}
