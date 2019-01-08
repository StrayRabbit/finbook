using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Applications.Service
{
    public class TestRecommendService : IRecommendService
    {
        public Task<bool> IsProjectInRecommend(int projectId, int userId)
        {
            return Task.FromResult(true);
        }
    }
}
