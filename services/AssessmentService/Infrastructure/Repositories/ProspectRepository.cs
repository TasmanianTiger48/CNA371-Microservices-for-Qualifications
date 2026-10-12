namespace AssessmentService.Infrastructure.Repositories;

public class ProspectRepository : IProspectRepository
{
    public ProspectRepository()
    {
    }

    public async Task<Prospect?> GetProspectById(Guid identifier, CancellationToken token)
    {
        return new Prospect();
    }

    public async Task<Prospect?> GetProspectByEmail(string email, CancellationToken token)
    {
        return new Prospect();
    }

    public async Task<Prospect> CreateProspect(ProspectDto prospect, CancellationToken token)
    {
        return new Prospect();
    }
}
