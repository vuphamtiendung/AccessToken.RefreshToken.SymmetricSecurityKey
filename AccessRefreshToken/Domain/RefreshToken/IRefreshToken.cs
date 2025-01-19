namespace AccessRefreshToken.Domain.RefreshToken
{
    public interface IRefreshToken
    {
        string GenerateRefreshToken(string userId);
    }
}
