using ExpensesInfo.Models;
using ExpensesInfo.Services;
using ExpensesInfo.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesInfo.Tests.Services
{
    public class ExpenseTypesServicesTests
    {
        //dopulnitelni zadachi 1)
        [Fact]
        public async Task GetAllAsync_Should_Return_All_Types()
        {
            using var db = DbFactory.CreateInMemory(nameof(GetAllAsync_Should_Return_All_Types));

            db.ExpenseTypes.AddRange( 
                new ExpenseType { Name = "Food" },
                new ExpenseType { Name = "Fuel" }
            );
            await db.SaveChangesAsync();

            var svc = new ExpenseTypeService(db);

            var types = await svc.GetAllAsync();

            types.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Correct_Type()
        {
            using var db = DbFactory.CreateInMemory(nameof(GetByIdAsync_Should_Return_Correct_Type));

            var et = new ExpenseType { Name = "TestType" };
            db.ExpenseTypes.Add(et);
            await db.SaveChangesAsync();

            var svc = new ExpenseTypeService(db);

            var found = await svc.GetByIdAsync(et.Id);

            found.Should().NotBeNull();
            found!.Name.Should().Be("TestType");
        }

        [Fact]
        public async Task CreateAsync_Should_Add_ExpenseType()
        {
            using var db = DbFactory.CreateInMemory(nameof(CreateAsync_Should_Add_ExpenseType));

            var svc = new ExpenseTypeService(db);

            await svc.CreateAsync(new ExpenseType { Name = "NewType" });

            (await db.ExpenseTypes.CountAsync()).Should().Be(1);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Fields()
        {
            using var db = DbFactory.CreateInMemory(nameof(UpdateAsync_Should_Update_Fields));

            var et = new ExpenseType { Name = "OldName" };
            db.ExpenseTypes.Add(et);
            await db.SaveChangesAsync();

            var svc = new ExpenseTypeService(db);

            et.Name = "Updated";
            await svc.UpdateAsync(et);

            (await db.ExpenseTypes.SingleAsync()).Name.Should().Be("Updated");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Type()
        {
            using var db = DbFactory.CreateInMemory(nameof(DeleteAsync_Should_Remove_Type));

            var et = new ExpenseType { Name = "X" };
            db.ExpenseTypes.Add(et);
            await db.SaveChangesAsync();

            var svc = new ExpenseTypeService(db);
            await svc.DeleteAsync(et.Id);

            (await db.ExpenseTypes.CountAsync()).Should().Be(0);
        }
    }
}
