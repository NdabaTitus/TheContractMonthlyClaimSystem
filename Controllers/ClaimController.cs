using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TheContractMonthlyClaimSystem.Models;

namespace TheContractMonthlyClaimSystem.Controllers
{
    public class ClaimController : Controller
    {
        
        [HttpGet]
        public IActionResult Submission()
        {
            var claim = new Claims();
            return View(claim);
        }

        [HttpPost]
        public IActionResult Submission(Claims claim)
        {
            if (!ModelState.IsValid)
            {
            
                return View(claim);
            }

            claim.TotalAmount = claim.CalculateTotal();
            claim.Status = "Submitted";
            claim.Submit();
            claim.SubmittedAt = DateTime.Now;

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
