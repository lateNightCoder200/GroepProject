namespace API.Repository.UserRepo
{
    public interface IUserRepository
    {
        public Task<string> getByUserName(string username);
        public Task<bool> updateUserName(string username , string newUserName);
    }
}
