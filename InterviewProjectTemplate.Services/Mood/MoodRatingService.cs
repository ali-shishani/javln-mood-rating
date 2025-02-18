using InterviewProjectTemplate.Config.Provider;
using InterviewProjectTemplate.Data.Entity;
using InterviewProjectTemplate.Models;
using InterviewProjectTemplate.Models.Constant;
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

            return await  Task.FromResult( (result, errors));
        }


        public async Task<(RecordMoodRatingResponse, List<Error> errors)> RecordMoodRating(RecordMoodRatingRequest request)
        {
            var result = new RecordMoodRatingResponse();
            var errors = new List<Error>();
            var currentDateUtc = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);

            var allMoodRatingRecords = await _moodRatingRepository.GetAllAsync();
            if (allMoodRatingRecords.Any(s => s.Email == request.Email && s.CreatedDateUtc == currentDateUtc))
            {
                errors.Add(Error.InvalidRequestError(ErrorConstants.InvalidRequestInputCode, "You already rated your mood today!"));
                return (result, errors);
            }

            var newRecord = new MoodRatingRecord()
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                CreatedDateUtc = currentDateUtc,
                Rating = (int)request.Rating,
                Comment = request.Comment
            };

            _moodRatingRepository.Add(newRecord);
            _moodRatingRepository.SaveChanges();
            result.Id = newRecord.Id;
            result.IsSuccessful = true;

            return await Task.FromResult((result, errors));
        }
    }
}
