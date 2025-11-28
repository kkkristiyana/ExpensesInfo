using ExpensesInfo.Models;


namespace ExpensesInfo.Services
{
    public interface IExpenseTypeService
    {
        Task<List<ExpenseType>> GetAllAsync();
        Task<ExpenseType?> GetByIdAsync(int id); Task CreateAsync(ExpenseType type);
        Task UpdateAsync(ExpenseType type);
        Task DeleteAsync(int id);
    }
}
