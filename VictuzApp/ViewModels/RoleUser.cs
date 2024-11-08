using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using VictuzApp.Models;

namespace VictuzApp.ViewModels
{
    public class RoleUser
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<SelectListItem> SelectRoles { get; set; }
        public IEnumerable<string> UserRoles {  get; set; }
    }
}
