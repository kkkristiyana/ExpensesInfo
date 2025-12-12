using Microsoft.AspNetCore.Mvc;

namespace ExpensesInfo.Controllers
{
    public class PlaygroundController : Controller
    {
        public IActionResult Index() => View();
    }
}
