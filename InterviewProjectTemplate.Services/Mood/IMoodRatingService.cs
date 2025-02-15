using InterviewProjectTemplate.Models.Mood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Services.Mood
{
    public interface IMoodRatingService
    {
        Task<GetMoodRatingOptionsResponse> GetMoodRatingOptions();
        Task<RecordMoodRatingResponse> RecordMoodRating(RecordMoodRatingRequest request);
    }
}
