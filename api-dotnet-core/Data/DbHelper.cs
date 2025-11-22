using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace api_dotnet_core.Data
{
    public class DbHelper : IDbHelper
    {
        private readonly string _connectionString;
        public DbHelper(IConfiguration cfg)
        {
            _connectionString = cfg.GetConnectionString("DefaultConnection");
        }


        public async Task<DataTable> ExecuteStoredProcAsync(string procName, params SqlParameter[] parameters)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure };
            if (parameters != null && parameters.Length > 0) cmd.Parameters.AddRange(parameters);


            var dt = new DataTable();
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);
            return dt;
        }
    }
}
