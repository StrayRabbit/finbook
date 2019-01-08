using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.API.Dtos;
using Project.Domain.AggregatesModel;
using Project.Domain.Exceptions;

namespace Project.API.Applications.Commands
{
    public class ViewProjectCommandHandler : IRequestHandler<ViewProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;

        public ViewProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(ViewProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.ProjectId);

            if (project == null)
            {
                throw new ProjectDomainException($"project is not found:{request.ProjectId}");
            }

            if (project.UserId == request.UserId)
            {
                throw  new ProjectDomainException($"you cannot view your own project");
            }

            project.AddViewer(request.UserId, request.UserName, request.Avatar);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
