using InterviewProjectTemplate.Data.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Identity
{
    //public interface IEmailSender
    //{
    //    Task SendEmailAsync(
    //      string email, string subject, string message);
    //}

    public class CustomEmailSender : IEmailSender<ApplicationUser>
    {
        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
        }

        public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
        }
    }
}
