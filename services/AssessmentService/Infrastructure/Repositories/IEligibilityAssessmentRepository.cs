namespace AssessmentService.Infrastructure.Repositories;

public interface IEligibilityAssessmentRepository
{
    public Task<IEnumerable<EligibilityAssessment>> GetAllEligibilityAssessments(CancellationToken token);

    public Task<EligibilityAssessment?> GetEligibilityAssessmentById(Guid identifier, CancellationToken token);

    public Task<IEnumerable<EligibilityAssessment>> GetEligibilityAssessmentsByProspect(Guid prospectIdentifier, CancellationToken token);

    public Task CreateEligibilityAssessment(EligibilityAssessment assessment, CancellationToken token);
}
