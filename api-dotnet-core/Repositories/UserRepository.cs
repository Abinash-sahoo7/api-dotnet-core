using api_dotnet_core.Data;
using api_dotnet_core.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace api_dotnet_core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbHelper _db;
        public UserRepository(IDbHelper db)
        {
            _db = db;
        }


        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var p = new SqlParameter("@Email", SqlDbType.NVarChar, 256) { Value = email };
            var dt = await _db.ExecuteStoredProcAsync("usp_GetUserByEmail", p);
            if (dt.Rows.Count == 0) return null;
            var r = dt.Rows[0];
            return new UserDto
            {
                Id = r.Field<int>("Id"),
                FullName = r.Field<string>("FullName"),
                Email = r.Field<string>("Email"),
                PasswordHash = r.Field<string>("PasswordHash")
            };
        }


        public async Task<bool> CreateUserAsync(UserDto user)
        {
            var p1 = new SqlParameter("@FullName", SqlDbType.NVarChar, 100) { Value = user.FullName };
            var p2 = new SqlParameter("@Email", SqlDbType.NVarChar, 256) { Value = user.Email };
            var p3 = new SqlParameter("@PasswordHash", SqlDbType.NVarChar, -1) { Value = user.PasswordHash };


            // we expect the stored proc to return a single row with Success bit or throw error
            var dt = await _db.ExecuteStoredProcAsync("usp_CreateUser", p1, p2, p3);
            if (dt.Rows.Count == 0) return false;
            var ok = dt.Rows[0].Table.Columns.Contains("Success") ? dt.Rows[0].Field<bool>("Success") : true;
            return ok;
        }
    }
}
