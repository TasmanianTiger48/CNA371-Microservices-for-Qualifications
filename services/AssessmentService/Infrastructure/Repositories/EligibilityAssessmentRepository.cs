namespace AssessmentService.Infrastructure.Repositories;

public class EligibilityAssessmentRepository : IEligibilityAssessmentRepository
{
    public EligibilityAssessmentRepository()
    {
    }

    public Task CreateEligibilityAssessment(EligibilityAssessment assessment, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<EligibilityAssessment>> GetAllEligibilityAssessments(CancellationToken token)
    {
        return [];
    }

    public async Task<EligibilityAssessment?> GetEligibilityAssessmentById(int id, CancellationToken token)
    {
        return new EligibilityAssessment();
    }
}
