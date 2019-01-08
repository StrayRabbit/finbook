using System.Threading.Tasks;

namespace Project.API.Applications.Queries
{
    public interface IProjectQueries
    {
        Task<dynamic> GetProjectDetail(int projectId);
        Task<dynamic> GetProjectsByUserId(int userId);
    }
}