using API.DataModel.DTO;
using API.DataModel.Models;

namespace API.Repository.UserRepo
{
    public interface IUserRepository
    {
        public Task<string> getByUserName(string username);
        public Task<bool> updateUserName(string email, string newUserName);
        public Task<string> getUserId(string username);
        public Task<bool> setPatientInfo( PatientInfo patientInfo);

        public Task<Guid> getPatientId(string userId);

        public Task<PatientInfo> getPatientInfoByUserId(string userId);

        public Task<bool> updatePatientInfo(PatientInfo  patientInfo);
    }
}
