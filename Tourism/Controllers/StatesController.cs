using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourism.DataAccess;
using Tourism.Models;

namespace Tourism.Controllers
{
    public class StatesController : Controller
    {
        private readonly TourismContext _context;

        public StatesController(TourismContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? timezone)
        {
            var states = _context.States.AsEnumerable();
            if(timezone != null)
            {
                states = states.Where(s => s.TimeZone == timezone);
                ViewData["SearchTimezone"] = timezone;
            }
            ViewData["AllTimezones"] = _context.States.Select(s => s.TimeZone).Distinct().ToList();

            return View(states);
        }

		public IActionResult New()
		{
			return View();
		}

        [HttpPost]
        [Route("/states/")]
        public IActionResult Create(State state)
        {
            _context.Add(state);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        [Route("states/{stateId:int}")]
        public IActionResult Show(int stateId)
        {
            var state = _context.States.Find(stateId);
            return View(state);
        }
        [Route("/states/{id:int}/edit")]
        public IActionResult Edit(int id)
        {
            var state = _context.States.Find(id);
            return View(state);
        }
        [HttpPost]
        [Route("/states/{id:int}")]
        public IActionResult Update(int id, State state)
        {
            state.Id = id;
            _context.States.Update(state);
            _context.SaveChanges();

            return Redirect($"/states/{state.Id}");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var state = _context.States.Find(id);
            _context.States.Remove(state);
            _context.SaveChanges();
            return Redirect("/states");
        }
    }
}
