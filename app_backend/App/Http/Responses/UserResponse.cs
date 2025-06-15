namespace app_backend.App.Http.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public OrganizationResponse Organization { get; set; } = null!;
    }
}