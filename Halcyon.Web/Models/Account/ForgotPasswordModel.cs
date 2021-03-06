﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Halcyon.Web.Models.Account
{
    public class ForgotPasswordModel
    {
        [DisplayName("Email Address")]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}