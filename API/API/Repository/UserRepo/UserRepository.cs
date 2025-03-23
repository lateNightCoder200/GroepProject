
using API.DataModel;
using API.DataModel.DTO;
using Dapper;
using System.Net.NetworkInformation;

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

<<<<<<< Updated upstream
            var parameters = new { NewUserName = newUserName, UserName = username };
=======
            return await connection.QueryFirstOrDefaultAsync<string>(query, new { Email = email });
        }


        //I use this method to check if user already stored his patient info
        public async Task<Guid> getPatientId(string userId)
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT Id FROM Patient WHERE UserId = @UserId";

            return await connection.QueryFirstOrDefaultAsync<Guid>(query, new { UserId = userId });
        }

        public async Task<bool> updateUserName(string email, string newUserName)
        {
            using var connection = _context.CreateConnection();

            string query = "UPDATE  auth.AspNetUsers SET UserName = @NewUserName WHERE Email = @Email";

            var parameters = new { NewUserName = newUserName, Email = email };
>>>>>>> Stashed changes

            int rowsAffected = await connection.ExecuteAsync(query, parameters);

            return rowsAffected > 0;
        }
<<<<<<< Updated upstream
=======

        public async Task<bool> setPatientInfo(PatientInfo patientInfo)
        {
            using var connection = _context.CreateConnection();

            string query = "INSERT INTO Patient (Id , UserId, FirstName, LastName, BirthDate, City, Hospital)" +
                "VALUES (@Id , @UserId , @FirstName , @LastName , @BirthDate , @City , @Hospital) ";

            int rowAffected = await connection.ExecuteAsync(query, patientInfo);

            return rowAffected > 0;
                           
        }

        public async Task<PatientInfo> getPatientInfoByUserId(string userId)
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT * FROM Patient WHERE UserId = @UserId";

            return await connection.QueryFirstOrDefaultAsync<PatientInfo>(query, new { UserId = userId });
        }

        public async Task<bool> updatePatientInfo(PatientInfo patientInfo)
        {
            using var connection = _context.CreateConnection();

            string query = @"UPDATE Patient 
                 SET UserId = @UserId, 
                     FirstName = @FirstName, 
                     LastName = @LastName, 
                     BirthDate = @BirthDate, 
                     City = @City, 
                     Hospital = @Hospital
                 WHERE Id = @Id";

            int rowsAffected = await connection.ExecuteAsync(query, patientInfo);

            return rowsAffected > 0;
        }
>>>>>>> Stashed changes
    }
}
