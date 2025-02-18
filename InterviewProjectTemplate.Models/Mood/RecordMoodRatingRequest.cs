using InterviewProjectTemplate.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Models.Mood
{
    public class RecordMoodRatingRequest
    {
        public int Rating { get; set; }

        public string Email { get; set; }

        public string Comment { get; set; }
    }
}
