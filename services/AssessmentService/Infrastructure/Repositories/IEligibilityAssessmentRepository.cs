namespace AssessmentService.Infrastructure.Repositories;

public interface IEligibilityAssessmentRepository
{
    public Task<IEnumerable<EligibilityAssessment>> GetAllEligibilityAssessments(CancellationToken token);

    public Task<EligibilityAssessment?> GetEligibilityAssessmentById(int id, CancellationToken token);

    public Task CreateEligibilityAssessment(EligibilityAssessment assessment, CancellationToken token);
}
