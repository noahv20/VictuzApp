using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Diagnostics;
using VictuzApp.Data;
using VictuzApp.Models;
using VictuzApp.ViewModels;

namespace VictuzApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;    
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context,ILogger<HomeController> logger, UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ChangeUserRole(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _context.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var selectList = new List<SelectListItem>();
            foreach (var role in roles) 
            {
                selectList.Add(new SelectListItem(value: role.Name, text: role.Name));
            };



            RoleUser roleUser = new RoleUser()
            {
                User = user,
                UserRoles = userRoles,
                SelectRoles = selectList
            };
            return View(roleUser);
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ChangeRole(string userId, string role)
        {
            if (userId == null || role == null)
            {
                return NotFound();
            }
            
            var user = await _userManager.FindByIdAsync(userId);
            var currentUserRoles = await _userManager.GetRolesAsync(user);

            string userRole = currentUserRoles[0];
            
            await _userManager.RemoveFromRoleAsync(user, userRole);
            await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction(nameof(ChangeUserRole), new { userId });
        }

        public async Task<IActionResult> Index()
        {
            var discounts = await _bestactivityService.GetDiscountsAsync();
            return View(discounts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
