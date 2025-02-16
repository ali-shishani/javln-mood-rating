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
        public string Email { get; set; }

        public MoodRatingOption Rating { get; set; }

        public string Comment { get; set; }
    }
}
