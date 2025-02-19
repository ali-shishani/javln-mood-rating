using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Identity
{
    public class UserRequest
    {
        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly Dob { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string RoleName { get; set; }

        public Guid UserId { get; set; }
    }
}
