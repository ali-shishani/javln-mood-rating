using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Identity
{
    public interface IUserAccountService
    {
        Task<IEnumerable<RoleInfo>> GetUserRoles(Guid userId);

    }
}
