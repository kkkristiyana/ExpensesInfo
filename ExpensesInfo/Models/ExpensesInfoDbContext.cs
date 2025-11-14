using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ExpensesInfo.Models
{
    public class ExpensesInfoDbContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseType> ExpenseTypes {  get; set; }
        public ExpensesInfoDbContext(DbContextOptions<ExpensesInfoDbContext> options):base(options) 
        { 

        }
    }
}
