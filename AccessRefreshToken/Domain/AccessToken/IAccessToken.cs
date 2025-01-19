namespace AccessRefreshToken.Domain.AccessToken
{
    public interface IAccessToken
    {
        public string GenerateAccessToken(string userId);
    }
}
