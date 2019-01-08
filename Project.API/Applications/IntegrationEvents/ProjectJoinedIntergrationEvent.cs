using Project.Domain.AggregatesModel;

namespace Project.API.Applications.IntegrationEvents
{
    public class ProjectJoinedIntergrationEvent
    {
        public string Company { get; set; }
        public string Introduction { get; set; }
        public string Avatar { get; set; }

        public ProjectContributor Contributor { get; set; }
    }
}
