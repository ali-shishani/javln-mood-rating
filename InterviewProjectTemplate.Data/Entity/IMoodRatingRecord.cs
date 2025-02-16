using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Entity
{
    public interface IEntityBase
    {
        Guid Id { get; set; }
    }
}
