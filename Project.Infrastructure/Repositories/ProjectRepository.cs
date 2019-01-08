using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Domain.AggregatesModel;
using Project.Domain.SeedWork;
using ProjectEntity = Project.Domain.AggregatesModel.Project;

namespace Project.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public ProjectRepository(ProjectContext projectContext)
        {
            _context = projectContext;
        }

        public async Task<ProjectEntity> GetAsync(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Properties)
                .Include(p => p.Contributors)
                .Include(p => p.ProjectVisibleRule)
                .Include(p => p.Viewers)
                .SingleOrDefaultAsync(p => p.Id == id);

            return project;
        }

        public ProjectEntity Add(ProjectEntity project)
        {
            if (project.IsTransient())
            {
                return _context.Add(project).Entity;
                //var result = _context.Add(project).Entity;
                //_context.SaveChanges();
                //return result;
            }
            else
            {
                return project;
            }
        }

        public void Update(ProjectEntity project)
        {
            _context.Update(project);
        }
    }
}
