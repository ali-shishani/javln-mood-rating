using InterviewProjectTemplate.Models.Mood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Services.Mood
{
    public class MoodRatingService : IMoodRatingService
    {
        
        public async Task<GetMoodRatingOptionsResponse> GetMoodRatingOptions()
        {
            var result = new GetMoodRatingOptionsResponse();

            // TODO: implement logic

            return result;
        }


        public async Task<RecordMoodRatingResponse> RecordMoodRating(RecordMoodRatingRequest request)
        {
            var result = new RecordMoodRatingResponse();

            // TODO: implement logic

            return result;
        }
    }
}
