namespace AccessRefreshToken.Model.Token
{
    public class RefreshTokenModel
    {
        public string ? Token { get; set; }
        public string ? UserId { get; set; }
        public DateTime Expiration { get; set; }
        public bool isRevoke { get; set; }
    }
}
