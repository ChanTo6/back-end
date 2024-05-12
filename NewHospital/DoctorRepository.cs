using hospital.Data;
using Microsoft.EntityFrameworkCore;
using NewHospital.Models;

namespace NewHospital
{
    public class DoctorRepository
    {
        private readonly HospitalDbContext _context;

        public DoctorRepository(HospitalDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            var doctorsFromDb = await _context.Doctors.ToListAsync();
            if (doctorsFromDb.Count == 0)
            {
                return GetHardcodedDoctors();
            }
            return doctorsFromDb;
        }


        public async Task<Doctor> GetDoctorByIdAsync(string id)
        {
            var doctorFromDb = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorID == id); 
            if (doctorFromDb == null)
            {
                return GetHardcodedDoctors().FirstOrDefault(d => d.DoctorID == id);
            }
            return doctorFromDb;
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor));
            }
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }


        private List<Doctor> GetHardcodedDoctors()
        {
            return new List<Doctor>
            {
                new Doctor { email="email1@gmail.com",DoctorID = "1", Name = "გიორგი", Surname = "ხორავა", Bio = "კარდიოლოგი / არითმოლოგი", PictureUrl = "https://s3-alpha-sig.figma.com/img/52d2/de8c/416a4210d59382f4a3488ddd51eb72fb?Expires=1715558400&Key-Pair-Id=APKAQ4GOSFWCVNEHN3O4&Signature=Fd~HiHBJGb7NRJ8Sq8djDzgKCLknM1d1Qb79Ew46oOVNejqzV1ZbZXIE99lit~Y0Ji3Aj7imov9~o9tCHWoBSSe7ZgGhZiqS6NJ7Sm1emaUzMZLzVU71rOJyFhJqSKJju8fTwxjmCLePodSFcOe2BG2wKtrpjtIqiMjxyM5kKnFsq19OEzpfoJ8IQtfJLijKXOPGnXUdc1fcgQgoFob0UJ0zTxrO~1zToBCR2JT2qY7zvzlEHIHD0dGdllzVb1NoKPPSC2aObDk9Ps0iXdRvQ4S5x55Pf04ptOw3teCCQM7qs7Ix51q5RhD5XB~e3QgfExkx-~UrHy~OVJNxJU7qZw__" },
                new Doctor { email="email2@gmail.com", DoctorID = "2", Name = "ნატალია", Surname = "გოგოხია", Bio = "ბავშვთა და მოზრდილთა კარდიოლოგი", PictureUrl = "https://s3-alpha-sig.figma.com/img/304f/df9c/87b3c163476290e32537033d8ca790e9?Expires=1715558400&Key-Pair-Id=APKAQ4GOSFWCVNEHN3O4&Signature=AmeoykWzHV6cNt3DQQb6dTtHlgGPQJkw~VEVMc9H39LuDOdLiGfK8swzXTYxBSU40VSx0ouhaaZKqwXkbsWdi2sRun3yiV~bhCJaupIS0O2~4aLZdG4bzw5WZOmwbv8XvEExhaOO6u8XA9o2QcGodnmtG~RP1bnChQziEdUEL8gCFMpTvEzmEXvkCbZcnRjpKwR8ivH2LY3uL9OKm8z3YI9tGgcP0nTS-ZnyqTGmPyEylJICXi~Uc90rsx1thbC6J77fXHB5dx70GNhHLfAUmpMShPShFPku8GK3NVkonw6lyKrulBgVBCVPA1iMzshMaQp~jJS~j4XFOsY8DyyH1w__" },
                new Doctor { email="email3@gmail.com", DoctorID = "3", Name = "ანა", Surname = "დვალი", Bio = "კარდიოქირურგი", PictureUrl = "https://s3-alpha-sig.figma.com/img/679e/0457/0dee445efd3161d5d4aac1cf697f8ee5?Expires=1715558400&Key-Pair-Id=APKAQ4GOSFWCVNEHN3O4&Signature=Mb7FO~-bRt2X1Skgo5DNZJQYust9N54K6NktfwCkYghTRCuWvkfKM3-HZ9z6K0YEDTsb~y9mC1GvLhAoIUWqsbvnuZGHc8EqmHhhMw7okG5SChrJb1nmLLY9L5YnSA0rfaM6NIEt05h0DqqGRS~y1S4EOxQkRGCjGAa6HtqfpQMGuRIaTIlyT98SpNOPK4mmpwvXKJgqm6DAklPmEYztOodDyST2kPFw9bsVe7NnSXii2SDFQf4CpleQB3LhT4PgPy4185~0QJEn-2ijV8NIuoPlgy5IvWUPBtmo4ifn1MXcpILU7CyAEWRN1ntTdZRptzoRb2bKO8KRMMtFZ2heLQ__" },
                new Doctor { email="email3@gmail.com", DoctorID = "4", Name = "გიორგი", Surname = "კარდიოლოგი / არითმოლოგი", Bio = "კარდიოქირურგი", PictureUrl = "https://s3-alpha-sig.figma.com/img/a367/37a4/e8cb173bb6619b9823922071da00ace8?Expires=1715558400&Key-Pair-Id=APKAQ4GOSFWCVNEHN3O4&Signature=qkAb5oHmHN4NtEzQ7U5gBVH-CO9Q-CFJfy6HDja-7QXxKDU-tnd9oFv3BQwiNfyEPFubNssv66Dr9FQl12O9-C~3LScQd3-Z5dERTLAYoe-ecVgOAt1FvpPjaaQXkxKc0uzm6k7e-oEOH5XTWPacXK7QXJWPF7BTWlfEKzwTeGM~5XYhmfuj6s5FcilB1cu9WGWwB4pV5-ifHTGDeZHt1U3bVcxr3qAC7fN4IT5pfK2boxbPZQ9D17S~TQ6IoQdi7Wadkau--m-tFwO~fvEbpxTSxAB-A0DyGCmEszAyHRL0Nnanxfhsp758Mup56J23cEkeIYpRqDHKjE9XQxrT~Q__" },
                new Doctor { email="email4@gmail.com", DoctorID = "5", Name = "ბარბარე", Surname = "ქორთუა", Bio = "კარდიოქირურგი", PictureUrl = "https://s3-alpha-sig.figma.com/img/e3e7/9fa1/38b961af041a5124a6945565a223f17e?Expires=1715558400&Key-Pair-Id=APKAQ4GOSFWCVNEHN3O4&Signature=Qh3rPZLIs0Xx243lnmr6n3v1jGzouJHIwx2t-TZCqxUQOWwbBuc-9BLGm6ZyqylU-bREQNMlEjml7CRdpetPOoVug96x-sobOuR4V19m4r6ywVxbOYzs~xBLQwJ2xILIyKpJhHJPZopBnUqjRtJ8voEoV0e~t7jTPPe6JJHRYXusnSsEYMijWiB7R-Ex2zmfsY9uIZvzEPbgmukEfV9i0heIo06dz-k6ABvnY-ng7UTkjItzZLAHJ8gzoJgWdotbeTtWDuEdct272o45Zu-wYxIQNjsLs5PirQ6kj9IeFfutN~XrfvpGuUtanofo1AuIc~B5sMm9r1Xs1F1627hiqw__" },
                new Doctor { email="email5@gmail.com", DoctorID = "6", Name = "სოფია", Surname = "გოგიაშვილი", Bio = "ბავშვთა და მოზრდილთა კარდიოლოგი", PictureUrl = "https://s3-alpha-sig.figma.com/img/d65c/51d4/ccf86b3ffa508d9f598796102aa1084f?Expires=1715558400&Key-Pair-Id=APKAQ4GOSFWCVNEHN3O4&Signature=X~f5QendTxzYUcZEhV95PixASptL2CJaYz70HvLANDEhJd1ibJ7AqemZabh4r8hcPir5d-lDzjko5euOu3J6fXieXeVQPaMBPZLQRBa4~KERggb6zli~M-D4YlsvWNqyUAu7R0LXK6qL5Meif0W5C2LpmNIeINkHmMx2JmSkIHEnmN5q82AdvL1OOgmrGF3HUuHKIVaY9TSR5rbcui46rd~Z0yGaBQX6Xw-pkytTFLOLOgNCQfZQNCudcU2H9XpBS0PGiKiuR1Ts8P-NUDz-1sit60zuRDKYW~Qr767l8lVK3~NTaOCWa3lFVv6mJp9-tvgu-puJfXetqz5Y8t5K-Q__" },
            };
        }
    }
   }
