namespace AssessmentService.Ports;

public interface IEligibilityAssessmentService
{
    public Task<IEnumerable<EligibilityAssessmentDto>> GetAllEligibilityAssessments(CancellationToken token);

    public Task<EligibilityAssessmentDto?> GetEligibilityAssessmentById(Guid identifier, CancellationToken token);

    public Task<IEnumerable<EligibilityAssessmentDto>?> GetEligibilityAssessmentsByProspectEmail(string email, CancellationToken token);

    public Task<EligibilityAssessmentDto?> CreateEligibilityAssessment(EligibilityAssessmentRequest request, CancellationToken token);
}
