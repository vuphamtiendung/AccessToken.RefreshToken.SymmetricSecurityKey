using AccessRefreshToken.Model.ListUser;

namespace AccessRefreshToken.Domain.Common
{
    public interface IValidateUser
    {
        public User ValidateUser(string userName, string password);
        public string UserId(string userId);
    }
}
