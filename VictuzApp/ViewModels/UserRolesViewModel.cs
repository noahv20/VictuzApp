using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using VictuzApp.Models;

namespace VictuzApp.ViewModels
{
    public class UserRolesViewModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}
