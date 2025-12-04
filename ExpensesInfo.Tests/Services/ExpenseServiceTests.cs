using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpensesInfo.Models;
using ExpensesInfo.Services;
using ExpensesInfo.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ExpensesInfo.Tests.Services
{
    public class ExpenseServiceTests
    {
        [Fact]
        public async Task GetAllAsync_Should_Filter_By_Type()
        {
            using var db =
DbFactory.CreateInMemory(nameof(GetAllAsync_Should_Filter_By_Type)); var typeFood = new ExpenseType { Name = "Food" }; var typeFuel = new ExpenseType { Name = "Fuel" }; db.ExpenseTypes.AddRange(typeFood, typeFuel); db.Expenses.AddRange(
                new Expense { Value = 10, ExpenseType = typeFood },
                new Expense { Value = 20, ExpenseType = typeFuel }); await db.SaveChangesAsync();

            var svc = new ExpenseService(db);

            var onlyFood = await svc.GetAllAsync(typeFood.Id);

            onlyFood.Should().HaveCount(1); onlyFood[0].ExpenseTypeId.Should().Be(typeFood.Id);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Expense()
        {
            using var db =
DbFactory.CreateInMemory(nameof(CreateAsync_Should_Add_Expense)); var svc = new ExpenseService(db);

            var e = new Expense { Value = 15, ExpenseTypeId = 1 }; await svc.CreateAsync(e);

            var count = await db.Expenses.CountAsync(); count.Should().Be(1);
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Fields()
        {
            using var db =
DbFactory.CreateInMemory(nameof(UpdateAsync_Should_Modify_Fields));
            var e = new Expense { Value = 5,  ExpenseTypeId = 1 }; db.Expenses.Add(e); await db.SaveChangesAsync();

            var svc = new ExpenseService(db);
            e.Value = 99;
            e.Description = "updated"; await svc.UpdateAsync(e);

            var updated = await db.Expenses.SingleAsync(); updated.Value.Should().Be(99); updated.Description.Should().Be("updated");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Expense()
        {
            using var db =
DbFactory.CreateInMemory(nameof(DeleteAsync_Should_Remove_Expense));
            var e = new Expense { Value = 7, ExpenseTypeId = 1 }; db.Expenses.Add(e); await db.SaveChangesAsync();

            var svc = new ExpenseService(db); await svc.DeleteAsync(e.Id);

            (await db.Expenses.CountAsync()).Should().Be(0);
        }

        [Fact]
        public async Task GetTotalAsync_Should_Sum_Filtered()
        {
            using var db =
DbFactory.CreateInMemory(nameof(GetTotalAsync_Should_Sum_Filtered)); var t1 = new ExpenseType { Name = "A" }; var t2 = new ExpenseType { Name = "B" }; db.ExpenseTypes.AddRange(t1, t2); db.Expenses.AddRange(
                new Expense { Value = 10, ExpenseType = t1 },
                new Expense { Value = 5, ExpenseType = t1 },
                new Expense { Value = 2, ExpenseType = t2 }); await db.SaveChangesAsync();

            var svc = new ExpenseService(db); var sumT1 = await svc.GetTotalAsync(t1.Id);

            sumT1.Should().Be(15);
        }
    }
}

