using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheContractMonthlyClaimSystem.Models
{
    public class Document
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
    }

    public class Claims
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lecturer ID is required")]
        [Display(Name = "Lecturer ID")]
        public int LecturerId { get; set; }

        [Required(ErrorMessage = "Claim month is required")]
        [Display(Name = "Claim Month")]
        public string ClaimMonth { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Hours worked must be 0 or more")]
        [Display(Name = "Hours Worked")]
        public double HoursWorked { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Hourly rate must be 0 or more")]
        [Display(Name = "Hourly Rate (R)")]
        [DataType(DataType.Currency)]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Additional Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string Notes { get; set; }

        public string Status { get; set; } = "Pending";

        [Display(Name = "Total Amount (R)")]
        [DataType(DataType.Currency)]
        public double TotalAmount { get; set; }

        [Display(Name = "Submitted Date")]
        [DataType(DataType.DateTime)]
        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Enhanced document properties
        [Display(Name = "Supporting Documents")]
        public List<Document> Documents { get; set; } = new List<Document>();

        [NotMapped]
        [Display(Name = "Upload Supporting Documents")]
        public List<IFormFile> DocumentFiles { get; set; } = new List<IFormFile>();

        public double CalculateTotal()
        {
            TotalAmount = (double)HoursWorked * (double)HourlyRate;
            return TotalAmount;
        }

        public void Submit()
        {
            SubmittedAt = DateTime.Now;
            LastUpdated = DateTime.Now;
            Status = "Submitted";
        }

        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
            LastUpdated = DateTime.Now;
        }

        // Helper method to get file type icon
        public string GetFileIcon(string fileType)
        {
            return fileType.ToLower() switch
            {
                "pdf" => "fa-file-pdf",
                "doc" or "docx" => "fa-file-word",
                "xls" or "xlsx" => "fa-file-excel",
                "jpg" or "jpeg" or "png" or "gif" => "fa-file-image",
                _ => "fa-file"
            };
        }

        // Helper method to get file type color
        public string GetFileColor(string fileType)
        {
            return fileType.ToLower() switch
            {
                "pdf" => "text-danger",
                "doc" or "docx" => "text-primary",
                "xls" or "xlsx" => "text-success",
                "jpg" or "jpeg" or "png" or "gif" => "text-warning",
                _ => "text-secondary"
            };
        }
    }
}