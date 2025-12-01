using ExpensesInfo.Models;

namespace ExpensesInfo.Services
{
    public interface IExpenseService
    {

        Task<List<Expense>> GetAllAsync(int? typeId);
        Task<Expense?> GetByIdAsync(int id);
        Task CreateAsync(Expense expense);
        Task UpdateAsync(Expense expense);
        Task DeleteAsync(int id);
        Task<List<ExpenseType>> GetAllTypesAsync();
        Task<decimal> GetTotalAsync(int? typeId);
    }
}