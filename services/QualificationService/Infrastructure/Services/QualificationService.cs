namespace QualificationService.Infrastructure.Services;

public class QualificationService : IQualificationService
{
    IQualificationRepository _qualificationRepository;

    public QualificationService(IQualificationRepository qualificationRepository)
    {
        _qualificationRepository = qualificationRepository;
    }

    public async Task<List<QualificationAssessmentResponse>> AssessQualificationsEligibility(QualificationAssessmentRequest request, CancellationToken token)
    {
        var qualifications = await _qualificationRepository.GetAll(token);

        var results = new List<QualificationAssessmentResponse>();

        foreach (var qualification in qualifications)
        {
            var isEligible = true;
            results.Add(new QualificationAssessmentResponse
            {
                IsEligible = isEligible,
                QualificationIdentifier = qualification.Identifier,
                QualificationCode = qualification.Code,
                QualificationName = qualification.Name
            });
        }

        return results;
    }

    public async Task<QualificationDto?> GetQualification(Guid identifier, CancellationToken token)
    {
        var qualifications = await _qualificationRepository.GetAll(token);

        return qualifications?.FirstOrDefault(q => q.Identifier == identifier)?.ToDto();
    }

    public async Task<List<QualificationDto>> GetQualifications(CancellationToken token)
    {
        var qualifications = await _qualificationRepository.GetAll(token);

        return qualifications.Select(q => q.ToDto()).ToList();
    }
}
