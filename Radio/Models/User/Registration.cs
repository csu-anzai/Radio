namespace Radio.Models.User
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Registration
    {
        [Required(ErrorMessage = "A username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "A password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "You must repeat your password.")]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [DisplayName("Repeat Password")]
        public string RepeatPassword { get; set; }
    }
}