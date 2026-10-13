namespace AssessmentService.Infrastructure.Repositories;

public class ProspectRepository : IProspectRepository
{
    private readonly IDatabase _database;

    public ProspectRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<Prospect?> GetProspectById(Guid identifier, CancellationToken token)
    {
        var value = await _database.StringGetAsync(InfrastructureConstants.ProspectKey(identifier));
        return value.HasValue ? JsonSerializer.Deserialize<Prospect>(value.ToString()) : null;
    }

    public async Task<Prospect?> GetProspectByEmail(string email, CancellationToken token)
    {
        var identifier = await _database.StringGetAsync(InfrastructureConstants.ProspectEmailKey(email));
        return identifier.HasValue ? await GetProspectById(Guid.Parse(identifier.ToString()), token) : null;
    }

    public async Task<Prospect> CreateProspect(ProspectDto prospect, CancellationToken token)
    {
        var existing = await GetProspectByEmail(prospect.Email, token);

        if (existing != null)
        {
            existing.FirstName = prospect.FirstName;
            existing.LastName = prospect.LastName;

            await _database.StringSetAsync(
                InfrastructureConstants.ProspectKey(existing.Identifier),
                JsonSerializer.Serialize(existing));

            return existing;
        }
        
        var created = new Prospect
        {
            Identifier = prospect.Identifier == null || prospect.Identifier == Guid.Empty ? Guid.NewGuid() : prospect.Identifier.Value,
            FirstName = prospect.FirstName,
            LastName = prospect.LastName,
            Email = prospect.Email
        };

        await _database.StringSetAsync(InfrastructureConstants.ProspectKey(created.Identifier), JsonSerializer.Serialize(created));
        await _database.StringSetAsync(InfrastructureConstants.ProspectEmailKey(created.Email), created.Identifier.ToString());

        return created;
    }

    
}
