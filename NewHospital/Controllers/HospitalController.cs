using hospital.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using NewHospital.Models;
using SimpleEmailApp.Services.EmailService;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NewHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly HospitalDbContext _hospitalDbcontext;
        private readonly Random _random;
        private readonly DoctorRepository _doctorRepository;

        public DateTime? VerificationCodeGeneratedTime { get; set; }

        public HospitalController(
           HospitalDbContext hospitalDbContext,
        IEmailService emailService,
        DoctorRepository doctorRepository)
        {
            _hospitalDbcontext = hospitalDbContext;
            _emailService = emailService;
            _random = new Random();
            _doctorRepository = doctorRepository;
        }


        [HttpPost("timedata")]
        public async Task<IActionResult> SaveTimeData([FromBody] TimeDataModel[] timeDataArray)
        {
            
                if (timeDataArray == null || timeDataArray.Length == 0)
                {
                    return BadRequest("No data received.");
                }



                _hospitalDbcontext.TimeData.AddRange(timeDataArray);
                await _hospitalDbcontext.SaveChangesAsync();

                return Ok("Data saved successfully.");
         }




        [HttpGet("timedata")]
        public async Task<IActionResult> DataTableTaken(int userId, string doctorId)
        {
            try
            {
                var allPersons = await _hospitalDbcontext.TimeData.ToListAsync();

                if (allPersons == null || allPersons.Count == 0)
                {
                    return BadRequest("No data available.");
                }

                var filteredTimes = allPersons.Where(t => t.DoctorId == doctorId && t.personalID == userId).ToList();

                return Ok(filteredTimes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }







        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.VerificationCode))
            {
                return BadRequest(new { error = "Email and verification code are required." });
            }

            var user = _hospitalDbcontext.Register.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }

            if (user.VerificationCode != request.VerificationCode)
            {
                return Unauthorized(new { error = "Incorrect verification code." });
            }

            if (user.VerificationCodeGeneratedTime.HasValue && DateTime.UtcNow > user.VerificationCodeGeneratedTime.Value.AddMinutes(30))
            {
                return BadRequest(new { error = "Verification code has expired." });
            }
            bool isAdmin = user.registerByAdmin;

            _hospitalDbcontext.SaveChanges();

            return Ok(new { message = "Login successful.", isAdmin = user.registerByAdmin, id = user.Id });
        }


        [HttpPost("email/send")]
        public IActionResult SendVerificationCode(EmailDto request)
        {
            var user = _hospitalDbcontext.Register.FirstOrDefault(u => u.Email == request.To);

            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }

            var verificationCode = _random.Next(1000, 9999).ToString();
            user.VerificationCode = verificationCode;
            user.VerificationCodeGeneratedTime = DateTime.UtcNow;
            _hospitalDbcontext.SaveChanges();
            _emailService.SendEmail(request, verificationCode);

            return Ok();
        }

        [HttpPut]
        [Route("PasswordChangeForPerson")]
        public async Task<IActionResult> PasswordChangeForPerson(RegisterModel request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.VerificationCode))
            {
                return BadRequest(new { error = "Email and verification code are required." });
            }

            var user = _hospitalDbcontext.Register.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }

            if (user.VerificationCode != request.VerificationCode)
            {
                return Unauthorized(new { error = "Incorrect verification code." });
            }

            user.Password = request.Password;
            await _hospitalDbcontext.SaveChangesAsync();
            return Ok(user);
        }



        [HttpPost("email/passwordRecovery")]
        public IActionResult passwordRecovery([FromBody] LoginModel request)
        {
            var user = _hospitalDbcontext.Register.FirstOrDefault(u => u.VerificationCode == request.VerificationCode);

            if (user == null)
            {
                return NotFound(new { error = "User not found or invalid verification code." });
            }

            user.Password = request.Password;
            _hospitalDbcontext.SaveChanges();

            return Ok(new { message = "Password changed successfully." });
        }



        [HttpPost]
        public async Task<IActionResult> AddPerson(RegisterModel register)
        {
            try
            {
                var existingUserByEmail = await _hospitalDbcontext.Register.FirstOrDefaultAsync(r => r.Email == register.Email);
                if (existingUserByEmail != null)
                {
                    return Conflict("User with this email is already registered.");
                }

                var existingUserByPersonalID = await _hospitalDbcontext.Register.FirstOrDefaultAsync(r => r.PersonalID == register.PersonalID);
                if (existingUserByPersonalID != null)
                {
                    return Conflict("User with this PersonalID is already registered.");
                }

                register.Id = Guid.NewGuid();
                _hospitalDbcontext.Register.Add(register);
                await _hospitalDbcontext.SaveChangesAsync();

                return Ok(register);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }



        [HttpGet]
        public async Task<IActionResult> AllPerson()
        {
            var AllPersons = await _hospitalDbcontext.Register.ToListAsync();
            return Ok(AllPersons);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson([FromBody] RegisterModel registerModel, Guid id)
        {
            try
            {
                var existingRegister = await _hospitalDbcontext.Register.FirstOrDefaultAsync(r => r.Id == id);

                if (existingRegister == null)
                {
                    return NotFound("Person not found.");
                }

                existingRegister.Name = registerModel.Name;
                existingRegister.Email = registerModel.Email;

                await _hospitalDbcontext.SaveChangesAsync();
                return Ok(existingRegister);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        [HttpDelete("{personalId}")]
        public async Task<IActionResult> DeletePerson(string personalId)
        {
            try
            {
                var existingRegister = await _hospitalDbcontext.Register.FirstOrDefaultAsync(r => r.PersonalID == personalId);

                if (existingRegister == null)
                {
                    return NotFound("Person not found.");
                }

                _hospitalDbcontext.Register.Remove(existingRegister);
                await _hospitalDbcontext.SaveChangesAsync();

                return Ok("Person deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _doctorRepository.GetDoctorsAsync();
            return Ok(doctors);
        }

        [HttpGet("doctors/{id}")]
        public async Task<IActionResult> GetDoctorById(string id)
        {
            var doctor = await _doctorRepository.GetDoctorByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }


        [HttpGet("OnlyUsers")]
        public async Task<ActionResult<IEnumerable<RegisterModel>>> OnlyUsersMethod(string personalId)
        {
            var users = await _hospitalDbcontext.Register
                .Where(x => x.PersonalID == personalId)
                .ToListAsync();

            if (!users.Any())
            {
                return NotFound();
            }

            return Ok(new { data = users });
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetUser(Guid userId)
        {
            var user = _hospitalDbcontext.Register.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }
            return Ok(user);
        }


    }
}