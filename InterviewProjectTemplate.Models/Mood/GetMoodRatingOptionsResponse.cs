using InterviewProjectTemplate.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Models.Mood
{
    public class GetMoodRatingOptionsResponse
    {
        public GetMoodRatingOptionsResponse()
        {
            MoodRatingOptions = new List<MoodRatingOption>();
        }

        public List<MoodRatingOption> MoodRatingOptions { get; set; }
    }
}
