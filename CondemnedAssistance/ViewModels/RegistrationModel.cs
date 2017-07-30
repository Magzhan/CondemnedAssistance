using System.ComponentModel.DataAnnotations;

namespace CondemnedAssistance.ViewModels {
    public class RegistrationModel {

        [Required(ErrorMessage = "Не указан логин")]
        [StringLength(12, ErrorMessage = "Длина символов не должна превышать 12", MinimumLength = 12)]
        public string Login { get; set; }

        [Required(ErrorMessage = ("Не указан Email"))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Не указан телефон")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
