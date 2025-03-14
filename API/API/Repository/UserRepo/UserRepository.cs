
using API.DataModel;
using API.DataModel.DTO;
using Dapper;

namespace API.Repository.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _context;

        public UserRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<string> getByUserName(string username)
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT UserName FROM auth.AspNetUsers WHERE UserName = @username";

            return await connection.QueryFirstOrDefaultAsync<string>(query, new { Username = username });
        }

        public async Task<bool> updateUserName(string username , string newUserName)
        {
            using var connection = _context.CreateConnection();

            string query = "UPDATE  auth.AspNetUsers SET UserName = @NewUserName WHERE UserName = @UserName";

            var parameters = new { NewUserName = newUserName, UserName = username };

            int rowsAffected = await connection.ExecuteAsync(query, parameters);

            return rowsAffected > 0;
        }
    }
}
