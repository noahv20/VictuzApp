using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VictuzApp.Data;
using VictuzApp.Models;
using VictuzApp.ViewModels;

namespace VictuzApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: Activities
        public async Task<IActionResult> IndexParticipant()
        {
            return View(await _context.Events.ToListAsync());
        }

        //GET: Register for an activity
        public async Task<IActionResult> RegisterForActivity(int activityId)
        {
            var activity = await _context.Events.FirstOrDefaultAsync(a => a.Id == activityId);
            var participants = await _context.Participants.ToListAsync();

            ParticipantEvent pa = new ParticipantEvent()
            {
                Event = activity
                ,
                Participants = participants
            };
            return View(pa);
        }
        //POST: Register for an activity
        [HttpPost]
        public async Task<IActionResult> RegisterForActivity(int activityId, int selectedParticipantId)
        {
            _context.Database.ExecuteSqlRaw("INSERT INTO ActivityParticipant (ActivitiesId, ParticipantsId) VALUES ({0},{1})", activityId, selectedParticipantId);
            await _context.SaveChangesAsync();

            return RedirectToAction("IndexParticipant");
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }
        // GET: Activity details
        public async Task<IActionResult> DetailsParticipant(int id)
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
