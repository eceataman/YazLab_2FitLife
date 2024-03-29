﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string UserMail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string UserPass { get; set; }
        public string SecurityQuestion { get; set; }

    }
}