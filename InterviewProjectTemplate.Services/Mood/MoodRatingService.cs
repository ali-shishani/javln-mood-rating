using InterviewProjectTemplate.Config.Provider;
using InterviewProjectTemplate.Models;
using InterviewProjectTemplate.Models.Mood;
using InterviewProjectTemplate.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Services.Mood
{
    public class MoodRatingService : IMoodRatingService
    {
        private readonly ILogger<MoodRatingService> _logger;
        private readonly IMoodRatingRecordRepository _moodRatingRepository;

        public MoodRatingService(ILogger<MoodRatingService> logger, 
            IMoodRatingRecordRepository moodRatingRepository)
        {
            _logger = logger;
            _moodRatingRepository = moodRatingRepository;
        }

        public async Task<(GetMoodRatingOptionsResponse, List<Error> errors)> GetMoodRatingOptions()
        {
            var result = new GetMoodRatingOptionsResponse();
            var errors = new List<Error>();

            var allMoodRatingRecords = await _moodRatingRepository.GetAllAsync();

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
