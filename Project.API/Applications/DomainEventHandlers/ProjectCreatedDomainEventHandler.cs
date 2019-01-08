using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Microsoft.Extensions.Logging;
using Project.API.Applications.IntegrationEvents;
using Project.Domain.Events;

namespace Project.API.Applications.DomainEventHandlers
{
    public class ProjectCreatedDomainEventHandler : INotificationHandler<ProjectCreatedEvent>
    {
        private readonly ICapPublisher _capPublisher;
        private readonly ILogger<ProjectCreatedDomainEventHandler> _logger;

        public ProjectCreatedDomainEventHandler(ICapPublisher capPublisher,
            ILogger<ProjectCreatedDomainEventHandler> logger)
        {
            _capPublisher = capPublisher;
            _logger = logger;
        }

        public Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectCreatedIntergrationEvent()
            {
                CreateTime = DateTime.Now,
                Company = notification.Project.Company,
                FinStage = notification.Project.FinStage,
                Introduction = notification.Project.Introduction,
                ProjectAvatar = notification.Project.Avatar,
                Tags = notification.Project.Tags,
                ProjectId = notification.Project.Id,
                UserId = notification.Project.UserId
            };

            _capPublisher.Publish("finbook.projectapi.projectcreated", @event);
            _logger.LogTrace($"finbook.projectapi.projectcreated 已发送!");

            return Task.CompletedTask;
        }
    }
}
