﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 6)]
        public string Password { get; set; }
    }
}