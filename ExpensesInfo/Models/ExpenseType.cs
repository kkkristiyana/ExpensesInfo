using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpensesInfo.Models
{
    public class ExpenseType
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [Display(Name = "Expense Type Name")]
        public string Name { get; set; } = null!;
    }
}
