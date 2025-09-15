using Microsoft.AspNetCore.Mvc;
using TheContractMonthlyClaimSystem.Models;

namespace TheContractMonthlyClaimSystem.Controllers
{
    public class ClaimController : Controller
    {
        
        public IActionResult Submission()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submission(Claims claim)
        {
            if (!ModelState.IsValid)
            {
              
                return View(claim);
            }

            claim.CalculateTotal();
            claim.Submit();

            return RedirectToAction("Tracking");
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
