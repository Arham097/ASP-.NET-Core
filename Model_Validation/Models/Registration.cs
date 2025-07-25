using System.ComponentModel.DataAnnotations;
namespace Model_Validation.Models
{
    public class Registration
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { set; get; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must bt atleast 6 character long.")]
        public string? Password { set; get; }
        [Required(ErrorMessage = "Confirm Passowrd is Required.")]
        [Compare("Password", ErrorMessage = "Password Do not Match")]
        public string ConfirmPassword { set; get; }

        public static ValueTask<Registration?> BindAsync(HttpContext context)
        {
            var email = context.Request.Query["email"];
            var password = context.Request.Query["pwd1"];
            var confirmPassword = context.Request.Query["pwd2"];

            return new ValueTask<Registration?>(new Registration { Email = email, Password = password, ConfirmPassword = confirmPassword });

        }
    }
}