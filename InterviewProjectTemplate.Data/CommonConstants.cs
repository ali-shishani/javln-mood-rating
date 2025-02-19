using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data
{
    public class ClaimType
    {
        public const string Permissions = "permissions";
    }

    public class ClaimPermission
    {
        public const string Create = "record.create";
        public const string Update = "record.update";
        public const string Delete = "record.delete";
        public const string Read = "record.readlist";
    }
}
