using AccessRefreshToken.Domain.AccessToken;
using AccessRefreshToken.Domain.RefreshToken;

namespace AccessRefreshToken.Model.Access
{
    public class TokenModel
    {
        public string ? AccessToken { get; set; }
        public string ? RefreshToken { get; set; }  
    }
}
