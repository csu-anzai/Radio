﻿namespace Radio.Models.User
{
    using System.ComponentModel.DataAnnotations;

    public class Login
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}