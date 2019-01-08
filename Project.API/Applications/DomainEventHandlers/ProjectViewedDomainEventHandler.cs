using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Project.API.Applications.IntegrationEvents;
using Project.Domain.Events;

namespace Project.API.Applications.DomainEventHandlers
{
    public class ProjectViewedDomainEventHandler : INotificationHandler<ProjectViewedEvent>
    {
        private readonly ICapPublisher _capPublisher;

        public ProjectViewedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public Task Handle(ProjectViewedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectViewedIntergrationEvent()
            {
                Avatar = notification.Avatar,
                Company = notification.Company,
                Introduction = notification.Introduction,
                Viewer = notification.Viewer,
            };

            _capPublisher.Publish("finbook.projectapi.projectviewed", @event);

            return Task.CompletedTask;
        }
    }
}
