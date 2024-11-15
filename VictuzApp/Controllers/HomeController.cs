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
using VictuzApp.Services;
using VictuzApp.ViewModels;

namespace VictuzApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;    
        private readonly ApplicationDbContext _context;
        private BestActivityService _bestActivityService;

        public HomeController(ApplicationDbContext context,ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, BestActivityService bestActivityService)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _bestActivityService = bestActivityService;
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var vm = new List<UserRolesViewModel>();

            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var viewModel = new UserRolesViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles
                };

                vm.Add(viewModel);
            }
            return View(vm);
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
            var e = await _context.Events
                   .Where(e => e.IsSuggestion == false)
                   .OrderBy(e => e.Date)
                   .Take(3) 
                   .ToListAsync();

            var activities = await _bestActivityService.GetDiscountsAsync();

            var vm = new HomePageViewModel()
            {
                BestActivities = activities,
                Events = e
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Sponsoren()
        {
            return View();
        }

        public IActionResult WieZijnWij()
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
