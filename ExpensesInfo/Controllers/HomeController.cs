using System.Diagnostics;
using ExpensesInfo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpensesInfo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExpensesInfoDbContext _context;

        public HomeController(ILogger<HomeController> logger, ExpensesInfoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Expenses(int? typeId)
        {
            var types = _context.ExpenseTypes.ToList();
            ViewBag.Types = types;
            ViewBag.SelectedTypeId = typeId;
            var query = _context.Expenses
            .Include(e => e.ExpenseType)
            .AsQueryable();
            if (typeId.HasValue)
            {
                query = query.Where(e => e.ExpenseTypeId == typeId.Value);
            }
            var allExpenses = query.ToList();
            ViewBag.TotalExpenses = allExpenses.Sum(e => e.Value);

            return View(allExpenses);
        }
        public IActionResult CreateEditExpense(int? id)
        {
            var types =_context.ExpenseTypes.ToList();
            ViewBag.Types=types;
            if (id == null)
            {
                return View(new Expense());
            }
            var expense = _context.Expenses.SingleOrDefault(e => e.Id == id);
            if (expense == null) return NotFound();
            return View(expense);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEditExpenseForm(Expense model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Types = _context.ExpenseTypes.ToList();
                return View("CreateEditExpense", model);
            }
            if (model.Id == 0)
            {
                
                _context.Expenses.Add(model);
            }
            else
            {
                var existing =_context.Expenses.SingleOrDefault(x=>x.Id == model.Id);
                if (existing == null) return NotFound();
                existing.Value=model.Value;
                existing.Description = model.Description;
                existing.ExpenseTypeId= model.ExpenseTypeId;
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Expenses));
        }
        public IActionResult DeleteExpense(int id)
        {
            var expense = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            if (expense == null) return NotFound();
            _context.Expenses.Remove(expense);
            _context.SaveChanges();
            return RedirectToAction(nameof(Expenses));
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
