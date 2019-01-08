using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.API.Applications.Commands;
using Project.API.Applications.Queries;
using Project.API.Applications.Service;
using Project.Domain.AggregatesModel;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IRecommendService _recommendService;
        private readonly IProjectQueries _projectQueries;

        public ProjectController(IMediator mediator,
            IRecommendService recommendService,
            IProjectQueries projectQueries)
        {
            _mediator = mediator;
            _recommendService = recommendService;
            _projectQueries = projectQueries;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateProject([FromBody]Domain.AggregatesModel.Project project)
        {
            try
            {
                if (project == null)
                {
                    throw new ArgumentException(nameof(project));
                }

                project.UserId = UserIdentity.UserId;

                var command = new CreateProjectCommand() { Project = project };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("view/{projectId}")]
        public async Task<IActionResult> ViewProject(int projectId)
        {
            if (await _recommendService.IsProjectInRecommend(projectId, UserIdentity.UserId))
            {
                return BadRequest("您没有权限查看项目!");
            }

            var command = new ViewProjectCommand()
            {
                UserId = UserIdentity.UserId,
                Avatar = UserIdentity.Avatar,
                ProjectId = projectId,
                UserName = UserIdentity.Name,
            };

            var result = await _mediator.Send(command);

            return Ok();
        }

        [HttpPut]
        [Route("join/{projectId}")]
        public async Task<IActionResult> JoinProject([FromBody]ProjectContributor contributor)
        {
            if (await _recommendService.IsProjectInRecommend(contributor.ProjectId, UserIdentity.UserId))
            {
                return BadRequest("您没有权限加入项目!");
            }

            var command = new JoinProjectCommand()
            {
                ProjectContributor = contributor
            };

            var result = await _mediator.Send(command);

            return Ok();
        }

        /// <summary>
        /// 获取用户 (自己??)的项目列表 //TBD
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var result = await _projectQueries.GetProjectsByUserId(UserIdentity.UserId);
            return Ok(result);
        }

        [Route("my/{projectId}")]
        [HttpGet]
        public async Task<IActionResult> GetMyProjectDetail(int projectId)
        {
            var result = await _projectQueries.GetProjectDetail(projectId);
            return Ok(result);
        }

        //api/recommend/{projectId}  用户获取推荐项目的 详细信息
        [Route("recommend/{projectId}")]
        [HttpGet]
        public async Task<IActionResult> GetRecommendProjectDetail(int projectId)
        {
            if (!(await _recommendService.IsProjectInRecommend(projectId, UserIdentity.UserId)))
            {
                return BadRequest("没有查看该项目的权限");
            }
            var result = await _projectQueries.GetProjectDetail(projectId);
            return Ok(result);
        }
    }
}
