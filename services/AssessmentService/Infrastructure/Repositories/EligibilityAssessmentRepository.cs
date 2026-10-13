namespace AssessmentService.Infrastructure.Repositories;

public class EligibilityAssessmentRepository : IEligibilityAssessmentRepository
{
    private readonly IDatabase _database;

    public EligibilityAssessmentRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task CreateEligibilityAssessment(EligibilityAssessment assessment, CancellationToken token)
    {
        await _database.StringSetAsync(InfrastructureConstants.AssessmentKey(assessment.Identifier), JsonSerializer.Serialize(assessment));
        await _database.SetAddAsync(InfrastructureConstants.AssessmentsKey, assessment.Identifier.ToString());

        if (assessment.Prospect != null)
            await _database.SetAddAsync(InfrastructureConstants.ProspectAssessmentsKey(assessment.Prospect.Identifier), assessment.Identifier.ToString());
    }

    public async Task<IEnumerable<EligibilityAssessment>> GetAllEligibilityAssessments(CancellationToken token)
    {
        return await GetEligibilityAssessments(InfrastructureConstants.AssessmentsKey);
    }

    public async Task<IEnumerable<EligibilityAssessment>> GetEligibilityAssessmentsByProspect(Guid prospectIdentifier, CancellationToken token)
    {
        return await GetEligibilityAssessments(InfrastructureConstants.ProspectAssessmentsKey(prospectIdentifier));
    }

    public async Task<EligibilityAssessment?> GetEligibilityAssessmentById(Guid identifier, CancellationToken token)
    {
        var value = await _database.StringGetAsync(InfrastructureConstants.AssessmentKey(identifier));
        return value.HasValue ? JsonSerializer.Deserialize<EligibilityAssessment>(value.ToString()) : null;
    }

    private async Task<IEnumerable<EligibilityAssessment>> GetEligibilityAssessments(string setKey)
    {
        var ids = await _database.SetMembersAsync(setKey);
        if (ids.Length == 0)
            return [];

        var values = await _database.StringGetAsync(ids.Select(id => (RedisKey)InfrastructureConstants.AssessmentKey(id!)).ToArray());

        return values
            .Where(value => value.HasValue)
            .Select(value => JsonSerializer.Deserialize<EligibilityAssessment>(value.ToString())!)
            .ToList();
    }
}
