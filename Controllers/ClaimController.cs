using Microsoft.AspNetCore.Mvc;
using TheContractMonthlyClaimSystem.Models;
using TheContractMonthlyClaimSystem.Services;

namespace TheContractMonthlyClaimSystem.Controllers
{
    public class ClaimController : Controller
    {
        private readonly ClaimService _claimService;
        private readonly IWebHostEnvironment _environment;

        public ClaimController(ClaimService claimService, IWebHostEnvironment environment)
        {
            _claimService = claimService;
            _environment = environment;
        }

        public IActionResult Submission()
        {
            return View(new Claims());
        }

        [HttpPost]
        public async Task<IActionResult> Submission(Claims claim)
        {
            if (!ModelState.IsValid)
            {
                return View(claim);
            }

            // Handle document uploads
            if (claim.DocumentFiles != null && claim.DocumentFiles.Any())
            {
                foreach (var document in claim.DocumentFiles)
                {
                    if (document.Length > 0)
                    {
                        // Validate file type and size
                        var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx", ".jpg", ".png", ".doc", ".xls" };
                        var maxFileSize = 5 * 1024 * 1024; // 5MB

                        var extension = Path.GetExtension(document.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("DocumentFiles", "Invalid file type. Allowed types: PDF, DOC, DOCX, XLS, XLSX, JPG, PNG");
                            return View(claim);
                        }

                        if (document.Length > maxFileSize)
                        {
                            ModelState.AddModelError("DocumentFiles", "File size too large. Maximum size is 5MB.");
                            return View(claim);
                        }

                        // Save file
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + document.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await document.CopyToAsync(fileStream);
                        }

                        // Create document metadata
                        var fileDocument = new Document
                        {
                            FileName = document.FileName,
                            FilePath = $"/uploads/{uniqueFileName}",
                            FileType = extension.TrimStart('.').ToUpper(),
                            FileSize = document.Length,
                            UploadDate = DateTime.Now
                        };

                        claim.Documents.Add(fileDocument);
                    }
                }
            }

            claim.CalculateTotal();
            _claimService.AddClaim(claim);

            TempData["SuccessMessage"] = "Claim submitted successfully!";
            return RedirectToAction("Tracking");
        }

        // New action to download documents
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

        // New action to view document in browser
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

        public IActionResult Tracking()
        {
            var claims = _claimService.GetAllClaims();
            return View(claims);
        }
    }
}