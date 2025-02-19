using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Identity
{
    /// <summary>
    /// Email and Phone are part of user account (and need confirmation process)
    /// Profile is additional data
    /// </summary>
    public class UserProfileCreateRequest
    {
        public string? MiddleName { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }

    /// <summary>
    /// Optional changes
    /// </summary>
    public class UserProfileUpdateRequest
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }

    public class UserProfileResponse
    {
        /// <summary>
        /// User Details Id
        /// </summary>
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public bool HasUserProfile { get; set; }

    }
}
