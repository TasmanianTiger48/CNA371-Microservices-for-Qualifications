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
        var eligibilityAssessment = new EligibilityAssessment()
        {
            Identifier = Guid.NewGuid(),
            Prospect = request.Prospect,
            CreatedAt = DateTime.UtcNow
        };

        var qualificationEligibilityResults = await _qualificationServiceClient.AssessAsync(request.ToQualificationAssessmentRequest(), token);

        eligibilityAssessment.Qualifications = qualificationEligibilityResults.Select(x => x.ToDomain()).ToList();

        await _assessmentRepository.CreateEligibilityAssessment(eligibilityAssessment, token);

        return eligibilityAssessment.ToDto();
    }

    public async Task<IEnumerable<EligibilityAssessmentDto>> GetAllEligibilityAssessments(CancellationToken token)
    {
        var assessments = await _assessmentRepository.GetAllEligibilityAssessments(token);
        return assessments.Select(x => x.ToDto());
    }

    public async Task<EligibilityAssessmentDto?> GetEligibilityAssessmentById(Guid identifier, CancellationToken token)
    {
        var assessment = await _assessmentRepository.GetEligibilityAssessmentById(identifier, token);
        return assessment?.ToDto();
    }
}
