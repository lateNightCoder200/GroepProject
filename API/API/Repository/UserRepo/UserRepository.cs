
using API.DataModel;
using API.DataModel.DTO;
using API.DataModel.Models;
using Dapper;
using System.Collections.Generic;
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

        public async Task<string> getUserNameByUserName(string username)
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT UserName FROM auth.AspNetUsers WHERE UserName = @username";

            return await connection.QueryFirstOrDefaultAsync<string>(query, new { Username = username });
        }

        public async Task<string> getUserNameByEmail(string Email)
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT UserName FROM auth.AspNetUsers WHERE email = @email";

            return await connection.QueryFirstOrDefaultAsync<string>(query, new { email = Email });
        }

        public async Task<string> getUserId(string email)
        {
            using var connection = _context.CreateConnection();

            string query = "SELECT Id FROM auth.AspNetUsers WHERE Email = @Email";

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

            int rowsAffected = await connection.ExecuteAsync(query, parameters);

            return rowsAffected > 0;
        }

        public async Task<bool> setPatientInfo(PatientInfo patientInfo)
        {
            using var connection = _context.CreateConnection();

            string query = "INSERT INTO Patient (Id , UserId, FirstName, LastName, BirthDate, City, Hospital, DoctorName, treatmentDate, treatmentPlan)" +
                "VALUES (@Id , @UserId , @FirstName , @LastName , @BirthDate , @City , @Hospital, @DoctorName, @treatmentDate, @treatmentPlan) ";

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
                     Hospital = @Hospital,
                     DoctorName = @DoctorName , 
                     treatmentDate = @treatmentDate,     
                     treatmentPlan = @treatmentPlan
                 WHERE Id = @Id";

            int rowsAffected = await connection.ExecuteAsync(query, patientInfo);

            return rowsAffected > 0;
        }

        public async Task<bool> addNote(Notes note)
        {
            using var connection = _context.CreateConnection();

            string query = "INSERT INTO Notes (UserId, Name, Note) VALUES (@UserId, @Name, @Note)";


            int rowAffected = await connection.ExecuteAsync(query, note);

            return rowAffected > 0;


        }

        public async Task<List<Notes>> getNotesByEmail(string userId)
        {
        
            using var connection = _context.CreateConnection();

            string query = "SELECT *  FROM Notes WHERE UserId = @userId";

            return (await connection.QueryAsync<Notes>(query, new { UserId = userId })).ToList();



        }

        public async Task<Notes> getNoteByName(string name , string userId)
        {

            using var connection = _context.CreateConnection();

            string query = "SELECT *  FROM Notes WHERE Name = @name AND UserId = @userId";

            return await connection.QueryFirstOrDefaultAsync<Notes>(query, new { Name = name , UserId = userId });


        }
    }

}