namespace AssessmentService.Ports;

public interface IEligibilityAssessmentService
{
    public Task<IEnumerable<EligibilityAssessmentDto>> GetAllEligibilityAssessments(CancellationToken token);

    public Task<EligibilityAssessmentDto?> GetEligibilityAssessmentById(int id, CancellationToken token);

    public Task<EligibilityAssessmentDto?> CreateEligibilityAssessment(EligibilityAssessmentRequest request, CancellationToken token);
}
