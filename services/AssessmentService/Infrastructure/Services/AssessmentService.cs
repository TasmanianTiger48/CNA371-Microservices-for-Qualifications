namespace AssessmentService.Infrastructure.Services;

public class AssessmentService : IAssessmentService
{
    IAssessmentRepository _assessmentRepository;
    IQualificationServiceClient _qualificationServiceClient;

    public AssessmentService(IAssessmentRepository assessmentRepository, IQualificationServiceClient qualificationServiceClient)
    {
        _assessmentRepository = assessmentRepository;
        _qualificationServiceClient = qualificationServiceClient;
    }
}
