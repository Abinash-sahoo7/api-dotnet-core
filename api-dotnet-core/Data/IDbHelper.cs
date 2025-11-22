using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace api_dotnet_core.Data
{
    public interface IDbHelper
    {
        Task<DataTable> ExecuteStoredProcAsync(string procName, params SqlParameter[] parameters);
    }
}
