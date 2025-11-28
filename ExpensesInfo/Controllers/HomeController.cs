using ExpensesInfo.Models;
using ExpensesInfo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

public class HomeController : Controller
{
    private readonly IExpenseService _expenses;

    public HomeController(IExpenseService expenses)
    {
        _expenses = expenses;
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }
    public async Task<IActionResult> Expenses(int? typeId)
    {
        ViewBag.Types = await _expenses.GetAllTypesAsync();
        ViewBag.SelectedTypeId = typeId;

        var all = await _expenses.GetAllAsync(typeId); ViewBag.TotalExpenses = await _expenses.GetTotalAsync(typeId);

        return View(all);
    }

    public async Task<IActionResult> CreateEditExpense(int? id)
    {
        ViewBag.Types = await _expenses.GetAllTypesAsync();

        if (id == null) return View(new Expense());

        var model = await _expenses.GetByIdAsync(id.Value); if (model == null) return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEditExpenseForm(Expense model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Types = await _expenses.GetAllTypesAsync(); return View("CreateEditExpense", model);
        }

        else
        {
            await _expenses.UpdateAsync(model);
        }

        return RedirectToAction(nameof(Expenses));
    }

    public async Task<IActionResult> DeleteExpense(int id)
    {
        await _expenses.DeleteAsync(id); return RedirectToAction(nameof(Expenses));
    }

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

