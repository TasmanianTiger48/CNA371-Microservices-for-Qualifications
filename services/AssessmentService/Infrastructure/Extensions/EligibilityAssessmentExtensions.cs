namespace AssessmentService.Infrastructure.Extensions;

public static class EligibilityAssessmentExtensions
{
    public static QualificationAssessmentRequest ToQualificationAssessmentRequest(this EligibilityAssessmentRequest request)
    {
        return new QualificationAssessmentRequest();
    }

    public static EligibilityAssessmentDto ToDto(this List<QualificationAssessmentResponse> response)
    {
        return new EligibilityAssessmentDto()
        {
            Qualifications = response.Select(x => x.ToDto()).ToList()
        };
    }

    public static EligibilityAssessmentQualificationDto ToDto(this QualificationAssessmentResponse response)
    {
        return new EligibilityAssessmentQualificationDto();
    }

    public static EligibilityAssessmentDto ToDto(this EligibilityAssessment assessment)
    {
        return new EligibilityAssessmentDto();
    }
}
