using ExpensesInfo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExpensesInfo.Controllers
{
    public class ExpenseTypesController : Controller
    {
        private readonly ExpensesInfoDbContext _context;

        public ExpenseTypesController(ExpensesInfoDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var types = await _context.ExpenseTypes.ToListAsync();
            return View(types);
        }

        public async Task<IActionResult> CreateEdit(int? id)
        {
            if (id == null)
            {
                return View(new ExpenseType());
            }

            var type =await _context.ExpenseTypes.SingleOrDefaultAsync(x => x.Id == id);
            if (type == null) return NotFound();

            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEdit(ExpenseType model)
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
                var existing =await _context.ExpenseTypes.SingleOrDefaultAsync(x => x.Id == model.Id);
                if (existing == null) return NotFound();

                existing.Name = model.Name;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var type =await _context.ExpenseTypes.SingleOrDefaultAsync(x => x.Id == id);
            if (type == null) return NotFound();

            _context.ExpenseTypes.Remove(type);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
