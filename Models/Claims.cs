namespace TheContractMonthlyClaimSystem.Models
{
    public class Claims
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }
        public string ClaimMonth { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; }
        public double TotalAmount { get; set; }
        public DateTime SubmittedAt { get; set; }

        // Methods
        public double CalculateTotal() => HoursWorked * HourlyRate;
        public void Submit() { }
        public void UpdateStatus(string newStatus) { Status = newStatus; }
    }
}