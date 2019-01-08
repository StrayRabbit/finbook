using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recommend.API.Data;
using Recommend.API.Services;

namespace Recommend.API.Controllers
{
    [Route("api/recommends")]
    public class RecommendController : BaseController
    {
        private readonly RecommendContext _context;
        private readonly IUserService _userService;

        public RecommendController(RecommendContext context,
            IUserService userService)
        {
            _context = context;
            _userService = userService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var result = _context.ProjectRecommends
                .AsNoTracking()
                .Where(r => r.UserId == UserIdentity.UserId)
                .ToList();

            return Ok(result);
        }

        [HttpGet, Route("{userId}")]
        public IActionResult Get(int userId)
        {
            var result = _userService.GetBaseUserInfoAsync(userId, CancellationToken.None);

            return Ok(result);
        }
    }
}
