using AutoBogus;
using InterviewProjectTemplate.Data;
using InterviewProjectTemplate.Repositories;
using InterviewProjectTemplate.Services.Mood;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Services.Tests.MooRatingServicedTests
{
    [TestClass]
    public class RecordMoodRatingTest
    {
        private AutoMocker _autoMocker;
        private IAutoFaker _faker;
        private Mock<MoodRatingDbContext> _mockMoodRatingDbContext;
        private Mock<IMoodRatingRecordRepository> _mockMoodRatingRecordRepository;
        private MoodRatingService _mockMoodRatingService;

        [TestInitialize]
        public void Setup()
        {
            _autoMocker = new AutoMocker();
            _faker = AutoFaker.Create(builder => builder.WithRecursiveDepth(1).WithTreeDepth(2).WithRepeatCount(1));

            _mockMoodRatingDbContext = _autoMocker.GetMock<MoodRatingDbContext>();
            _mockMoodRatingService = _autoMocker.CreateInstance<MoodRatingService>();
        }

        [TestMethod]
        public async Task RecordMoodRating_Should_Record_Successfully()
        {
            // TODO: implement test logic
        }

        [TestMethod]
        public async Task RecordMoodRating_Should_Not_Record_Duplicates()
        {
            // TODO: implement test logic
        }
    }
}
