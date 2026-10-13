namespace AssessmentService.Infrastructure.Services;

public class EligibilityAssessmentService : IEligibilityAssessmentService
{
    IEligibilityAssessmentRepository _assessmentRepository;
    IProspectRepository _prospectRepository;
    IQualificationServiceClient _qualificationServiceClient;

    public EligibilityAssessmentService(IEligibilityAssessmentRepository assessmentRepository, IProspectRepository prospectRepository, IQualificationServiceClient qualificationServiceClient)
    {
        _assessmentRepository = assessmentRepository;
        _prospectRepository = prospectRepository;
        _qualificationServiceClient = qualificationServiceClient;
    }

    public async Task<EligibilityAssessmentDto?> CreateEligibilityAssessment(EligibilityAssessmentRequest request, CancellationToken token)
    {
        var prospect = await _prospectRepository.CreateProspect(request.Prospect.ToDto(), token);

        var eligibilityAssessment = new EligibilityAssessment()
        {
            Identifier = Guid.NewGuid(),
            Prospect = prospect,
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

    public async Task<IEnumerable<EligibilityAssessmentDto>?> GetEligibilityAssessmentsByProspectEmail(string email, CancellationToken token)
    {
        var prospect = await _prospectRepository.GetProspectByEmail(email, token);
        if (prospect == null)
            return null;

        var assessments = await _assessmentRepository.GetEligibilityAssessmentsByProspect(prospect.Identifier, token);
        return assessments.Select(x => x.ToDto());
    }
}
