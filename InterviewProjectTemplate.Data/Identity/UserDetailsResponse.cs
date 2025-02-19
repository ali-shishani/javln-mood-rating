using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Identity
{
    public class UserDetailsResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateOnly Dob { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime? LastModifiedDateUtc { get; set; }
    }
}
