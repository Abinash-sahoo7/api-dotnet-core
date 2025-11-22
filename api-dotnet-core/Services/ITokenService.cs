using api_dotnet_core.DTOs;

namespace api_dotnet_core.Services
{
    public interface ITokenService
    {
        string GenerateToken(UserDto user);
    }
}
