namespace MyRealEstate.Web.Models.Account
{
    public class LoginViewModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }

    public class RegisterViewModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [System.ComponentModel.DataAnnotations.StringLength(120, MinimumLength = 3, ErrorMessage = "الاسم يجب أن يكون بين 3 و 120 حرفا")]
        public string FullName { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "رقم الجوال مطلوب")]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^05[0-9]{8}$", ErrorMessage = "أدخل رقم جوال سعودي صحيح يبدأ بـ 05")]
        public string Mobile { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "رقم الهوية مطلوب")]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهوية يجب أن يكون 10 أرقام")]
        public string NationalId { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [System.ComponentModel.DataAnnotations.StringLength(100, MinimumLength = 8, ErrorMessage = "كلمة المرور يجب أن تكون 8 أحرف على الأقل")]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "كلمتا المرور غير متطابقتين")]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        public bool AgreeToTerms { get; set; }

        public string? ReturnUrl { get; set; }
    }
}

