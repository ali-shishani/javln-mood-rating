using InterviewProjectTemplate.Data;
using InterviewProjectTemplate.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Repositories
{
    public class MoodRatingRecordRepository : GenericRepository<ApplicationRole>, IMoodRatingRecordRepository
    {
        public MoodRatingRecordRepository(MoodRatingDbContext dbContext) : base(dbContext)
        {
        }
    }
}
