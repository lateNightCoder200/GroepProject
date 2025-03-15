using API.DataModel.Models;

namespace API.Repository.UserRepo
{
    public interface IUserRepository
    {
        public Task<string> getByUserName(string username);
        public Task<bool> updateUserName(string email, string newUserName);
        public Task<string> getUserId(string username);
        public Task<bool> setPatientInfo( PatientInfo patientInfo);

        public Task<Guid> getPatientUserId(string userId);
    }
}
