using MediatR;

namespace Project.API.Applications.Commands
{
    public class CreateProjectCommand : IRequest<Project.Domain.AggregatesModel.Project>
    {
        public Domain.AggregatesModel.Project Project { get; set; }
    }
}
