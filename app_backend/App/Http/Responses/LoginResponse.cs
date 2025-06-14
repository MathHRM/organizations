namespace app_backend.App.Http.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;

        public UserResponse User { get; set; } = null!;
    }
} 