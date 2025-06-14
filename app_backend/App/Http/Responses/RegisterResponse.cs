using System.Text.Json.Serialization;
using app_backend.App.Models;

namespace app_backend.App.Http.Responses
{
    public class RegisterResponse
    {
        public string Token { get; set; } = null!;
    }
}