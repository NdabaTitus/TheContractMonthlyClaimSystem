using Microsoft.AspNetCore.Mvc;

namespace TheContractMonthlyClaimSystem.Controllers
{
    public class DocumentController : Controller
    {
      
        public IActionResult Upload()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Upload(IFormFile document)
        {
            if (document != null && document.Length > 0)
            {
               
                ViewBag.Message = "Document uploaded successfully!";
            }
            else
            {
                ViewBag.Message = "Please select a valid file.";
            }

            return View();
        }
    }
}
