namespace AssessmentService.Infrastructure.Services;

public class ProspectService : IProspectService
{
    private readonly IProspectRepository _prospectRepository;

    public ProspectService(IProspectRepository prospectRepository)
    {
        _prospectRepository = prospectRepository;
    }

    public async Task<Prospect> CreateProspect(ProspectDto prospect, CancellationToken token)
    {
        return await _prospectRepository.CreateProspect(prospect, token);
    }

    public async Task<Prospect?> GetProspectByEmail(string email, CancellationToken token)
    {
        return await _prospectRepository.GetProspectByEmail(email, token);
    }

    public async Task<Prospect?> GetProspectById(Guid identifier, CancellationToken token)
    {
        return await _prospectRepository.GetProspectById(identifier, token);
    }
}
