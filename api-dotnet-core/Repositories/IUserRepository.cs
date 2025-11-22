using api_dotnet_core.DTOs;

namespace api_dotnet_core.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto> GetByEmailAsync(string email);
        Task<bool> CreateUserAsync(UserDto user);
    }
}
