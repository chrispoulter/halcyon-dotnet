﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Halcyon.Web.Models.User
{
    public class UpdateUserModel
    {
        public UpdateUserModel()
        {
            Roles = new List<string>();
        }

        [DisplayName("Email Address")]
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string EmailAddress { get; set; }

        [DisplayName("First Name")]
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [DisplayName("Date Of Birth")]
        [Required]
        public DateTime? DateOfBirth { get; set; }

        public List<string> Roles { get; set; }
    }
}