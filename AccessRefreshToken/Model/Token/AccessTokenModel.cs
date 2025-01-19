namespace AccessRefreshToken.Model.Token
{
    public class AccessTokenModel
    {
        public string ? SecretKey { get; set; }
        public string ? Issuer { get; set; }
        public string ? Audience { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
}
