namespace AssetManagement.Application.Models.DTOs.Users.Responses
{
    public class AuthenticationResponse
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public bool? IsFirstTimeLogin { get; set; }
        public string Token { get; set; }
    }
}