using System.Threading.Tasks;

namespace AuthApiDemo
{
    public interface ITokenProvider
    {
        Task<TokenResponse> GetToken(TokenRequest request);
    }
}