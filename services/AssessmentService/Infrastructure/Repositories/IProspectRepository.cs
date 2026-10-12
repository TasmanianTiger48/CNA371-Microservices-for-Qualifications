namespace AssessmentService.Infrastructure.Repositories;

public interface IProspectRepository
{
    public Task<Prospect?> GetProspectById(Guid identifier, CancellationToken token);

    public Task<Prospect?> GetProspectByEmail(string email, CancellationToken token);

    public Task<Prospect> CreateProspect(ProspectDto prospect, CancellationToken token);
}
