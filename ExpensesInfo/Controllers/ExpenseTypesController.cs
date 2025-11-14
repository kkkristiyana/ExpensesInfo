using ExpensesInfo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesInfo.Controllers
{
    public class ExpenseTypesController : Controller
    {
        private readonly ExpensesInfoDbContext _context;

        public ExpenseTypesController(ExpensesInfoDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var types = _context.ExpenseTypes.ToList();
            return View(types);
        }

        public IActionResult CreateEdit(int? id)
        {
            if (id == null)
            {
                return View(new ExpenseType());
            }

            var type = _context.ExpenseTypes.SingleOrDefault(x => x.Id == id);
            if (type == null) return NotFound();

            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEdit(ExpenseType model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Id == 0)
            {
                _context.ExpenseTypes.Add(model);
            }
            else
            {
                var existing = _context.ExpenseTypes.SingleOrDefault(x => x.Id == model.Id);
                if (existing == null) return NotFound();

                existing.Name = model.Name;
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var type = _context.ExpenseTypes.SingleOrDefault(x => x.Id == id);
            if (type == null) return NotFound();

            _context.ExpenseTypes.Remove(type);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
