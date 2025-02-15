using InterviewProjectTemplate.Models.Mood;
using InterviewProjectTemplate.Services.Mood;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace InterviewProjectTemplate.Controllers
{
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

        [HttpGet("GetMoodRatingOptions")]
        public async Task<ActionResult<IEnumerable<GetMoodRatingOptionsResponse>>> GetMoodRatingOptions()
        {
            _logger.LogInformation("User is trying to get the mood rating page options");
            var result = await _moodRatingService.GetMoodRatingOptions();
            return Ok(result);
        }

        [HttpPost("RecordMoodRating")]
        public async Task<ActionResult<RecordMoodRatingResponse>> RecordMoodRating(RecordMoodRatingRequest request)
        {
            _logger.LogInformation("User is trying to record the mood rating");
            var result = await _moodRatingService.RecordMoodRating(request);
            var url = $"{Request.GetDisplayUrl()}/{result.Id}";
            return Created(url, result);
        }
    }
}
