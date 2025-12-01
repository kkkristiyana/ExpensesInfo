using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpensesInfo.Models;
using ExpensesInfo.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ExpensesInfo.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task Expenses_Should_Return_View_With_Model()
        {
            var mockSvc = new Mock<IExpenseService>();
            mockSvc.Setup(s => s.GetAllTypesAsync()).ReturnsAsync(new List<ExpenseType>());
            mockSvc.Setup(s => s.GetAllAsync(null)).ReturnsAsync(new List<Expense> { new Expense { Value = 1 } }); mockSvc.Setup(s => s.GetTotalAsync(null)).ReturnsAsync(1m); var controller = new HomeController(mockSvc.Object);

            var result = await controller.Expenses(null) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<List<Expense>>(result.Model); var model = (List<Expense>)result.Model!;
            Assert.Single(model);
        }

        [Fact]
        public async Task CreateEditExpenseForm_InvalidModel_Should_Return_Same_View()
        {
            var mockSvc = new Mock<IExpenseService>(); var controller = new HomeController(mockSvc.Object); controller.ModelState.AddModelError("Value", "Required");

            var model = new Expense();
            var result = await controller.CreateEditExpenseForm(model) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("CreateEditExpense", result.ViewName);
        }

        [Fact]
        public async Task
        CreateEditExpenseForm_Valid_New_Should_Redirect_And_Call_Create()
        {
            var mockSvc = new Mock<IExpenseService>(); var controller = new HomeController(mockSvc.Object);

            var model = new Expense { Value = 10, Date = DateTime.Today, ExpenseTypeId = 1 };
            var result = await controller.CreateEditExpenseForm(model) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(HomeController.Expenses), result.ActionName);
            mockSvc.Verify(s => s.CreateAsync(It.IsAny<Expense>()), Times.Once);
        }
    }
}

