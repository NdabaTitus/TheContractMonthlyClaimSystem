namespace TheContractMonthlyClaimSystem.Models
{
    public class LecturerProfile
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string EmployeeNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


        public void SubmitClaim() { }
        public void ViewClaimStatus() { }
        public void UploadDocument() { }
    }
}