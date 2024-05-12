namespace NewHospital.Models
{
    public class RegisterModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PersonalID { get; set; }
        public string? VerificationCode { get; set; }
        public DateTime? VerificationCodeGeneratedTime { get; set; }
        public bool registerByAdmin { get; set; }
        public string? bio { get; set; }
        public string? cv { get; set; }
        public string? photo { get; set; }
    }
}
