using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Entity
{
    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        public Guid Id { get; set; }
    }
}
