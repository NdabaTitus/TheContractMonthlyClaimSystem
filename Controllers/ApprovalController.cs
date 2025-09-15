using Microsoft.AspNetCore.Mvc;

namespace TheContractMonthlyClaimSystem.Controllers
{
    public class ApprovalController : Controller
    {
        
        public IActionResult Pending()
        {
            return View();
        }

   
        public IActionResult Approved()
        {
            return View();
        }
    }
}
