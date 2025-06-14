namespace app_backend.App.Http.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;

        public UserResponse User { get; set; } = null!;
    }

    public class UserResponse
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string Email { get; set; } = null!;
    }
} 