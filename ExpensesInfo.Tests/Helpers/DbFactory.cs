using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensesInfo.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesInfo.Tests.Helpers
{
    public static class DbFactory
    {
        public static ExpensesInfoDbContext CreateInMemory(string dbName)
        {
            var options = new DbContextOptionsBuilder<ExpensesInfoDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options; return new ExpensesInfoDbContext(options);
        }
    }
}

