using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VictuzApp.Data;
using VictuzApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using VictuzApp.ViewModels;

namespace VictuzApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, BoardMember")]
        public async Task<IActionResult> Approve(int id)
        {
            var e = await _context.Events.Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            e.IsSuggestion = false;

            _context.Update(e);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewSuggestions");

        }

        [Authorize (Roles = "Admin, BoardMember")]
        public async Task<IActionResult> ViewSuggestions(string searchTerm)
        {

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var events = from e in _context.Events
                             where e.IsSuggestion == true
                             select e;
                events = events.Where(e => e.Title.Contains(searchTerm)); // Filteren op titel
                return View(events);
            }
            else
            {
                var events = await _context.Events.Where(e => e.IsSuggestion == true)
                    .ToListAsync();
                var sortedEvents = events.OrderByDescending(item => item.Date).ToList();
                return View(sortedEvents);
            }

        }

        [Authorize(Roles = "Member")]

        public async Task<IActionResult> Suggestion()
        {
            return View();
        }
        [Authorize(Roles = "Member")]
        [HttpPost]
        public async Task<IActionResult> Suggestion([Bind("Id,Title,Description,Date,MaxParticipants")] Event activity)
        {
            activity.IsSuggestion = true;
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
           
        }
        [Authorize(Roles ="Admin,BoardMember")]
        public async Task<IActionResult> ViewRegistrations(int? eventId)
        {
            if (eventId == null)
            {
                return NotFound();
            }
            var e = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            var eu = await _context.EventUsers.Where(e => e.EventId == eventId)
                .ToListAsync();
            if(eu != null)
            {
                var vm = new RegstrationsViewModel();
                var users = new List<ApplicationUser>();
                foreach (var item in eu)
                    {
                        users.Add(await _context.Users.Where(u => u.Id == item.UserId)
                            .FirstOrDefaultAsync());
                    }
                vm.Event = e;
                vm.Users = users;
                return View(vm);
            }else
            {
                var vm = new RegstrationsViewModel()
                {
                    Event = e
                };
                return View(vm);
            }

            
        }
        [HttpGet]
        public async Task<IActionResult> RegisterForActivity(int? eventId)
        {
            if (eventId == null)
            {
                return NotFound();
            }

            var e = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == eventId);
            if (e == null)
            {
                return NotFound();
            }

            return View(e);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterActivityGuest(int? eventId, string? name, string? email)
        {
            if(eventId == null|| name== null|| email == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    Email = email,
                    UserName = name
                };
                await _userManager.CreateAsync(user);
            }
            string userId = await _userManager.GetUserIdAsync(user);
            _context.Database.ExecuteSqlRaw("INSERT INTO EventUsers (EventId, UserId) VALUES ({0},{1})", eventId, userId);
            await _context.SaveChangesAsync();

            return View("RegistrationSucces");
        }

        // Register for an activity
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegisterActivity(int eventId)
        {
            string userId = _userManager.GetUserId(User);
            _context.Database.ExecuteSqlRaw("INSERT INTO EventUsers (EventId, UserId) VALUES ({0},{1})", eventId, userId);
            await _context.SaveChangesAsync();

            return View("RegistrationSucces");
        }

        // GET: Activities
        // Toegevoegde zoekfunctionaliteit
        public async Task<IActionResult> Index(string searchTerm)
        {
            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var events = from e in _context.Events 
                             where e.IsSuggestion== false
                             select e;
                events = events.Where(e => e.Title.Contains(searchTerm)); // Filteren op titel
                return View(events);
            }
            else
            {
                var events = await _context.Events.Where(e=> e.IsSuggestion == false)
                    .ToListAsync();
                var sortedEvents = events.OrderByDescending(item => item.Date).ToList();
                return View(sortedEvents);
            }

        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Date,MaxParticipants")] Event activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Events.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Date,MaxParticipants")] Event activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Events.FindAsync(id);
            if (activity != null)
            {
                _context.Events.Remove(activity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
