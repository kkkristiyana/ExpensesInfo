using System.ComponentModel.DataAnnotations;

namespace ExpensesInfo.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Range(0,double.MaxValue)]
        public decimal Value { get; set; }
        [Required]
        public string? Description { get; set; }
        [Display(Name ="Expense Type")]
        [Required]
        public int ExpenseTypeId {  get; set; }
        public DateTime Date { get; set; }
        public ExpenseType? ExpenseType { get; set; }
    }
}
