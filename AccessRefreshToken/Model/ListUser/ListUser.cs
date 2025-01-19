namespace AccessRefreshToken.Model.ListUser
{
    public class ListUser
    {
        public List<User> ListUserPaging()
        {
            var listUser = new List<User>()
            {
                new User(){Id = Guid.NewGuid().ToString(), UserName = "Admin", Password = "Password"},
                new User(){Id = Guid.NewGuid().ToString(), UserName = "dungvpt", Password = "dungvpt"}
            };
            return listUser;
        }
    }
}
