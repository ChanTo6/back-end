namespace NewHospital.Models
{
    public class LoginModel
    {
        public string ?Email { get; set; }
        public string ?Password { get; set; }
        public string ?VerificationCode { get; set; }
        public  DateTime? VerificationCodeGeneratedTime { get; set; }
    }
}
