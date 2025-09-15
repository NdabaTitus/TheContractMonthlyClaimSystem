using Microsoft.AspNetCore.Mvc;

namespace TheContractMonthlyClaimSystem.Controllers
{
    public class ClaimController : Controller
    {
        public IActionResult Submission()
        {
            return View();
        }

        public IActionResult Approval()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

        public IActionResult Tracking()
        {
            return View();
        }
    }
}