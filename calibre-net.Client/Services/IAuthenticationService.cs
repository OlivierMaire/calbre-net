using calibre_net.Client.Models;

namespace calibre_net.Client.Services;

public interface IAuthenticationService
{

    Task<Guid?> SignInAsync(SignInModel data);
    Task<AuthenticationModel> RefreshTokenAsync(string accessToken, string refreshToken);
}