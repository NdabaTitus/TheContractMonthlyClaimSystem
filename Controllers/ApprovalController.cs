using Microsoft.AspNetCore.Mvc;
using TheContractMonthlyClaimSystem.Services;

namespace TheContractMonthlyClaimSystem.Controllers
{
    public class ApprovalController : Controller
    {
        private readonly ClaimService _claimService;
        private readonly IWebHostEnvironment _environment;

        public ApprovalController(ClaimService claimService, IWebHostEnvironment environment)
        {
            _claimService = claimService;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var pendingClaims = _claimService.GetPendingClaims();
            return View(pendingClaims);
        }

        [HttpPost]
        public IActionResult ApproveClaim(int id)
        {
            _claimService.UpdateClaimStatus(id, "Approved");
            TempData["SuccessMessage"] = "Claim approved successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RejectClaim(int id)
        {
            _claimService.UpdateClaimStatus(id, "Rejected");
            TempData["SuccessMessage"] = "Claim rejected successfully!";
            return RedirectToAction("Index");
        }

        // Document viewing for managers
        public IActionResult ViewDocument(int claimId, string fileName)
        {
            var document = _claimService.GetDocument(claimId, fileName);
            if (document == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_environment.WebRootPath, document.FilePath.TrimStart('/'));
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var contentType = GetContentType(document.FileType);
            return PhysicalFile(filePath, contentType);
        }

        // Document download for managers
        public IActionResult DownloadDocument(int claimId, string fileName)
        {
            var document = _claimService.GetDocument(claimId, fileName);
            if (document == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_environment.WebRootPath, document.FilePath.TrimStart('/'));
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", document.FileName);
        }

        private string GetContentType(string fileType)
        {
            return fileType.ToLower() switch
            {
                "pdf" => "application/pdf",
                "doc" => "application/msword",
                "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "xls" => "application/vnd.ms-excel",
                "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "jpg" or "jpeg" => "image/jpeg",
                "png" => "image/png",
                _ => "application/octet-stream"
            };
        }

        public IActionResult Approved()
        {
            var approvedClaims = _claimService.GetAllClaims().Where(c => c.Status == "Approved").ToList();
            return View(approvedClaims);
        }

        public IActionResult Pending()
        {
            return RedirectToAction("Index");
        }
    }
}