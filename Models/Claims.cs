using System;
using System.ComponentModel.DataAnnotations;

namespace TheContractMonthlyClaimSystem.Models
{
    public class Claims
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lecturer ID is required")]
        public int LecturerId { get; set; }

        [Required(ErrorMessage = "Claim month is required")]
        public string ClaimMonth { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Hours worked must be 0 or more")]
        public double HoursWorked { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Hourly rate must be 0 or more")]
        public decimal HourlyRate { get; set; }

        public string Status { get; set; }

        public double TotalAmount { get; set; }

        public DateTime SubmittedAt { get; set; }

        // Calculate total and update TotalAmount
        public double CalculateTotal()
        {
            TotalAmount = (double)HoursWorked * (double)HourlyRate;
            return TotalAmount;
        }

        // Mark claim as submitted
        public void Submit()
        {
            SubmittedAt = DateTime.Now;
            Status = "Submitted";
        }

        // Update status
        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
        }
    }
}
