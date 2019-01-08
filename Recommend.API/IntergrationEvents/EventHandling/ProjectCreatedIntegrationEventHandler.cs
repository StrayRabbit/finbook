using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using Recommend.API.Data;
using Recommend.API.IntergrationEvents.Events;
using Recommend.API.Models;
using Recommend.API.Services;
using Resilience;

namespace Recommend.API.IntergrationEvents.EventHandling
{
    public class ProjectCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly RecommendContext _context;
        private readonly IUserService _userService;
        private readonly IContactService _contactService;
        private readonly ILogger<ProjectCreatedIntegrationEventHandler> _logger;
        private readonly IHttpClient _httpClient;

        public ProjectCreatedIntegrationEventHandler(RecommendContext context,
            IUserService userService,
            IContactService contactService,
            ILogger<ProjectCreatedIntegrationEventHandler> logger,
            IHttpClient httpClient)
        {
            _context = context;
            _userService = userService;
            _contactService = contactService;
            _logger = logger;
            _httpClient = httpClient;
        }

        [CapSubscribe("finbook.projectapi.projectcreated")]
        public async Task CreateRecommendFromProject(ProjectCreatedIntergrationEvent @event)
        {
            var fromUser = await _userService.GetBaseUserInfoAsync(@event.UserId, CancellationToken.None);
            var contacts = await _contactService.GetContactsByUserId(@event.UserId);

            foreach (var contact in contacts)
            {
                var recommend = new ProjectRecommend()
                {
                    FromUserId = @event.UserId,
                    Company = @event.Company,
                    Tags = @event.Tags,
                    ProjectId = @event.ProjectId,
                    ProjectAvatar = @event.ProjectAvatar,
                    FinStage = @event.FinStage,
                    RecommendTime = DateTime.Now,
                    CreateTime = @event.CreateTime,
                    Introduction = @event.Introduction,
                    RecommednType = EnumRecommendType.Friend,
                    FromUserAvatar = fromUser.Avatar,
                    FromUserName = fromUser.Name,
                    UserId = contact.UserId,
                };

                _context.ProjectRecommends.Add(recommend);
            }

            _logger.LogTrace($"finbook.projectapi.projectcreated 接收成功!");
            _context.SaveChanges();
        }
    }
}
