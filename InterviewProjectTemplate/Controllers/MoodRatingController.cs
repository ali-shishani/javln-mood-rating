using InterviewProjectTemplate.Models;
using InterviewProjectTemplate.Models.Mood;
using InterviewProjectTemplate.Services.Mood;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InterviewProjectTemplate.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoodRatingController : ControllerBase
    {
        private readonly ILogger<MoodRatingController> _logger;
        private readonly IMoodRatingService _moodRatingService;

        public MoodRatingController(
            ILogger<MoodRatingController> logger,
            IMoodRatingService moodRatingService
            )
        {
            _logger = logger;
            _moodRatingService = moodRatingService;
        }

        [AllowAnonymous]
        [HttpGet("GetMoodRatingOptions")]
        public async Task<ApiResponse<GetMoodRatingOptionsResponse>> GetMoodRatingOptions()
        {
            _logger.LogInformation("User is trying to get the mood rating page options");
            var (result, errors) = await _moodRatingService.GetMoodRatingOptions();

            return new ApiResponseBuilder<GetMoodRatingOptionsResponse>()
                .WithErrors(errors)
                .WithData(result)
                .WithHttpStatus(Response, HttpStatusCode.OK)
                .Build();
        }

        [AllowAnonymous]
        [HttpPost("RecordMoodRating")]
        public async Task<ApiResponse<RecordMoodRatingResponse>> RecordMoodRating(RecordMoodRatingRequest request)
        {
            _logger.LogInformation("User is trying to record the mood rating");
            var (result, errors) = await _moodRatingService.RecordMoodRating(request);

            return new ApiResponseBuilder<RecordMoodRatingResponse>()
                .WithErrors(errors)
                .WithData(result)
                .WithHttpStatus(Response, HttpStatusCode.OK)
                .Build();
        }

        // TODO: implement logic
        [HttpGet("GetMoodRatings")]
        public async Task<ApiResponse<GetMoodRatingOptionsResponse>> GetMoodRatings()
        {
            _logger.LogInformation("Admin is trying to record the mood rating");
            var (result, errors) = await _moodRatingService.GetMoodRatingOptions();

            return new ApiResponseBuilder<GetMoodRatingOptionsResponse>()
                .WithErrors(errors)
                .WithData(result)
                .WithHttpStatus(Response, HttpStatusCode.OK)
                .Build();
        }

    }
}
