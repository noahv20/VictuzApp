using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VictuzApp.ViewModels
{
    public class RoleUser
    {
        public IdentityUser User { get; set; }
        public IEnumerable<SelectListItem> SelectRoles { get; set; }
        public IEnumerable<string> UserRoles {  get; set; }
    }
}
