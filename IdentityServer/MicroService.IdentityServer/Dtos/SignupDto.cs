using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.IdentityServer.Dtos
{
    public class SignupDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        public string City { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
