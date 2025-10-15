using Microsoft.AspNetCore.Mvc;

namespace TheContractMonthlyClaimSystem.Controllers
{
    public class DocumentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}