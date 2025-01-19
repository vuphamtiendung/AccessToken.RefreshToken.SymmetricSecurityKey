namespace AccessRefreshToken.Domain.Common
{
    public interface IValidateToken
    {
        public string ValidateToken(string token);
    }
}
