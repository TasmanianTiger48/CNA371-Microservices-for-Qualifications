using Microsoft.Extensions.Caching.Memory;

namespace AssessmentService.Infrastructure.Repositories;

public class EligibilityAssessmentRepository : IEligibilityAssessmentRepository
{
    public EligibilityAssessmentRepository()
    {
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
