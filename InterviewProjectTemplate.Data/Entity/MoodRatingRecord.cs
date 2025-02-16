using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Entity
{
    [Table("MoodRatingRecord")]
    public class MoodRatingRecord: IEntityBase
    {
        public Guid Id { get; set; }
    }
}
