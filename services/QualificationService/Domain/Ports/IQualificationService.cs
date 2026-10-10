namespace QualificationService.Domain.Ports;

public interface IQualificationService
{
    Task<QualificationDto?> GetQualification(Guid identifier, CancellationToken token);

    Task<List<QualificationDto>> GetQualifications(CancellationToken token);

    Task<List<QualificationAssessmentResponse>> AssessQualificationsEligibility(QualificationAssessmentRequest request, CancellationToken token);
}
