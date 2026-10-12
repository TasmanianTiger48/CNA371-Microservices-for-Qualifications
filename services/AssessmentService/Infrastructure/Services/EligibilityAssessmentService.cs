namespace AssessmentService.Infrastructure.Services;

public class EligibilityAssessmentService : IEligibilityAssessmentService
{
    IEligibilityAssessmentRepository _assessmentRepository;
    IQualificationServiceClient _qualificationServiceClient;

    public EligibilityAssessmentService(IEligibilityAssessmentRepository assessmentRepository, IQualificationServiceClient qualificationServiceClient)
    {
        _assessmentRepository = assessmentRepository;
        _qualificationServiceClient = qualificationServiceClient;
    }

    public async Task<EligibilityAssessmentDto?> CreateEligibilityAssessment(EligibilityAssessmentRequest request, CancellationToken token)
    {
        var qualification = await _qualificationServiceClient.AssessAsync(request, token);
        return qualification?.ToDto();
    }

    public async Task<IEnumerable<EligibilityAssessmentDto>> GetAllEligibilityAssessments(CancellationToken token)
    {
        var assessments = await _assessmentRepository.GetAllEligibilityAssessments(token);
        return assessments.Select(x => x.ToDto());
    }

    public async Task<EligibilityAssessmentDto?> GetEligibilityAssessmentById(int id, CancellationToken token)
    {
        var assessment = await _assessmentRepository.GetEligibilityAssessmentById(id, token);
        return assessment?.ToDto();
    }
}
