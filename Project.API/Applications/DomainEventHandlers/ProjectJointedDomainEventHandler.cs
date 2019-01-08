using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Project.API.Applications.IntegrationEvents;
using Project.Domain.Events;

namespace Project.API.Applications.DomainEventHandlers
{
    public class ProjectJointedDomainEventHandler : INotificationHandler<ProjectJoinedEvent>
    {
        private readonly ICapPublisher _capPublisher;

        public ProjectJointedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public Task Handle(ProjectJoinedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectJoinedIntergrationEvent()
            {
                Avatar = notification.Avatar,
                Company = notification.Company,
                Introduction = notification.Introduction,
                Contributor = notification.Contributor,
            };

            _capPublisher.Publish("finbook.projectapi.projectjoined", @event);

            return Task.CompletedTask;
        }
    }
}
