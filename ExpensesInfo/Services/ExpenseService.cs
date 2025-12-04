using ExpensesInfo.Models;
using Microsoft.EntityFrameworkCore;
namespace ExpensesInfo.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ExpensesInfoDbContext _context;
        public ExpenseService(ExpensesInfoDbContext context) => _context =
        context;
        public async Task<List<Expense>> GetAllAsync(int? typeId)
        {
            var query = _context.Expenses.Include(e =>
            e.ExpenseType).AsQueryable();
            if (typeId.HasValue)
            {
            }
            query = query.Where(e => e.ExpenseTypeId == typeId.Value);
            return await query.ToListAsync();
        }
        public async Task<Expense?> GetByIdAsync(int id)
        {
            return await _context.Expenses.SingleOrDefaultAsync(e => e.Id == id);
        }
        public async Task CreateAsync(Expense expense)
        {

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Expense expense)
        {
            var existing = await _context.Expenses.SingleOrDefaultAsync(e =>
            e.Id == expense.Id);
            if (existing == null) return;
            existing.Value = expense.Value;
            existing.Description = expense.Description;
            existing.ExpenseTypeId = expense.ExpenseTypeId;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Expenses.SingleOrDefaultAsync(e =>
            e.Id == id);
            if (existing == null) return;
            _context.Expenses.Remove(existing);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ExpenseType>> GetAllTypesAsync()
        {
            return await _context.ExpenseTypes.ToListAsync();
        }
        public async Task<decimal> GetTotalAsync(int? typeId)
        {



            var query = _context.Expenses.AsQueryable();
            if (typeId.HasValue)
            {
                query = query.Where(e => e.ExpenseTypeId == typeId.Value);
            }
            var list = await query.ToListAsync();
            return list.Sum(e => e.Value);
        }
    }
}
