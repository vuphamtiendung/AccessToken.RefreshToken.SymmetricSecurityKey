using AccessRefreshToken.Model.ListUser;

namespace AccessRefreshToken.Domain.Common
{
    public class ValidateUser : IValidateUser
    {
        private readonly ListUser _user;
        public ValidateUser(ListUser user)
        {
            _user = user;
        }

        public string UserId(string userId)
        {
            var id = _user.ListUserPaging().FirstOrDefault(s => s.Id == userId);
            return id.ToString();
        }

        User IValidateUser.ValidateUser(string userName, string password)
        {
            return _user.ListUserPaging()
                        .FirstOrDefault(s => s.UserName == userName && s.Password == password);
        }
    }
}
