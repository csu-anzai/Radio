namespace Radio.Models.User
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Registration
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [DisplayName("Repeat Password")]
        public string RepeatPassword { get; set; }
    }
}