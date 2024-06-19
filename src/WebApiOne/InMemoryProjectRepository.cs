namespace WebApiOne;

public class InMemoryProjectRepository
{
    private readonly List<Project> _projects = [];

    public void Add(Project project)
    {
        _projects.Add(project);
    }

    public IEnumerable<Project> GetAll() => _projects;

    public Project? GetById(string id) => _projects.FirstOrDefault(p => p.Id == id);
}