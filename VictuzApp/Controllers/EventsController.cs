using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VictuzApp.Data;
using VictuzApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace VictuzApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles ="Admin,BoardMember")]
        public async Task<IActionResult> ViewRegistrations(int eventId)
        {
            var e = await _context.Events.Include(e => e.Users)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            return View(e);
        }
        [HttpGet]
        public async Task<IActionResult> RegisterForActivity(int eventId)
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
        public async Task<IActionResult> RegisterActivityGuest(int eventId, string name, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                user = new IdentityUser()
                {
                    Email = email,
                    UserName = name
                };
                await _userManager.CreateAsync(user);
            }
            string userId = await _userManager.GetUserIdAsync(user);
            _context.Database.ExecuteSqlRaw("INSERT INTO EventParticipant (EventsId, UsersId) VALUES ({0},{1})", eventId, userId);
            await _context.SaveChangesAsync();

            return View("RegistrationSucces");
        }
        //Register for an activity
        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> RegisterActivity(int eventId)
        {
            string userId = _userManager.GetUserId(User);
            _context.Database.ExecuteSqlRaw("INSERT INTO EventParticipant (EventsId, UsersId) VALUES ({0},{1})", eventId, userId);
            await _context.SaveChangesAsync();

            return View("RegistrationSucces");
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.ToListAsync();
            var sortedEvents = events.OrderByDescending(item => item.Date).ToList();
            return View(sortedEvents);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
