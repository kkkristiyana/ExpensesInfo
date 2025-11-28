using ExpensesInfo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ExpensesInfo.Services;

public class ExpenseTypesController : Controller
{
    private readonly IExpenseTypeService _types;

    public ExpenseTypesController(IExpenseTypeService types)
    {
        _types = types;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _types.GetAllAsync(); return View(list);
    }

    public async Task<IActionResult> CreateEdit(int? id)
    {
        if (id == null) return View(new ExpenseType());

        var model = await _types.GetByIdAsync(id.Value); if (model == null) return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEdit(ExpenseType model)
    {
        if (!ModelState.IsValid) return View(model); if (model.Id == 0) await _types.CreateAsync(model); else await _types.UpdateAsync(model);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _types.DeleteAsync(id); return RedirectToAction(nameof(Index));
    }
}

