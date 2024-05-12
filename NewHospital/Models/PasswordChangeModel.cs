namespace NewHospital.Models
{
    public class PasswordChangeModel
    {
        public string? Email { get; set; }
        public string? VerificationCode { get; set; }
        public string? NewPassword { get; set; }
    }
}
