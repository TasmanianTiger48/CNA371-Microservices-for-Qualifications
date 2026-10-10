using AssessmentService.Contracts;

namespace AssessmentService.Clients;

public interface IQualificationServiceClient
{
    Task<List<QualificationAssessmentResponse>> AssessAsync(QualificationAssessmentRequest request, CancellationToken token);
}
