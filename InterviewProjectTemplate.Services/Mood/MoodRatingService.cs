using InterviewProjectTemplate.Models;
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
        
        public async Task<(GetMoodRatingOptionsResponse, List<Error> errors)> GetMoodRatingOptions()
        {
            var result = new GetMoodRatingOptionsResponse();
            var errors = new List<Error>();

            // TODO: implement logic

            return (result, errors);
        }


        public async Task<(RecordMoodRatingResponse, List<Error> errors)> RecordMoodRating(RecordMoodRatingRequest request)
        {
            var result = new RecordMoodRatingResponse();
            var errors = new List<Error>();

            // TODO: implement logic

            return (result, errors);
        }
    }
}
