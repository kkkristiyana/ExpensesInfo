using ExpensesInfo.Models;
using ExpensesInfo.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            var mockSvc = new Mock<IExpenseService>();
            var controller = new HomeController(mockSvc.Object);

            var model = new Expense 
            { 
                Value = 10, 
                ExpenseTypeId = 1 , 
                //Description = "e" 
            };
            var result = await controller.CreateEditExpenseForm(model) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(HomeController.Expenses), result.ActionName);
            mockSvc.Verify(s => s.CreateAsync(It.IsAny<Expense>()), Times.Once);
        }
        //dopulnitelni zadachi 2)
        [Fact]
        public async Task Expenses_Should_Pass_TypeId_To_Service_And_Return_Filtered_Model()
        {
            // arrange
            var mockSvc = new Mock<IExpenseService>();

            mockSvc.Setup(s => s.GetAllAsync(5))
                .ReturnsAsync(new List<Expense>
                {
                    new Expense { Value = 10, ExpenseTypeId = 5 },
                    new Expense { Value = 20, ExpenseTypeId = 5 }
                });

            var controller = new HomeController(mockSvc.Object);

            // act
            var result = await controller.Expenses(5);

            // assert
            mockSvc.Verify(s => s.GetAllAsync(5), Times.Once);

            var view = result as ViewResult;
            view.Should().NotBeNull();

            var model = view!.Model as List<Expense>;
            model.Should().HaveCount(2);
            model![0].ExpenseTypeId.Should().Be(5);
        }
    }
}

