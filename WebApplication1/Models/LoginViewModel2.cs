using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class LoginViewModel2
    {
        [Required]
        [EmailAddress]
        public string CoachMail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string CoachPassword { get; set; }
    }
}