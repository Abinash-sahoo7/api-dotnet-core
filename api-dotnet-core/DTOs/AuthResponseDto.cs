namespace api_dotnet_core.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}
