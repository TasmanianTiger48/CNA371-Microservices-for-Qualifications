namespace AssessmentService.Infrastructure.Extensions;

public static class EligibilityAssessmentExtensions
{
    public static QualificationAssessmentRequest ToQualificationAssessmentRequest(this EligibilityAssessmentRequest request)
    {
        return new QualificationAssessmentRequest();
    }

    public static EligibilityAssessment ToDomain(this List<QualificationAssessmentResponse> response, EligibilityAssessment eligibilityAssessment)
    {
        return new EligibilityAssessment()
        {
            Identifier = eligibilityAssessment.Identifier,
            CreatedAt = eligibilityAssessment.CreatedAt,
            Prospect = eligibilityAssessment.Prospect,
            Qualifications = response.Select(x => x.ToDomain()).ToList()
        };
    }

    public static EligibilityAssessmentQualification ToDomain(this QualificationAssessmentResponse response)
    {
        return new EligibilityAssessmentQualification()
        {
            QualificationIdentifier = response.QualificationIdentifier,
            QualificationCode = response.QualificationCode,
            QualificationName = response.QualificationName,
            IsEligible = response.IsEligible,
            Reasons = response.Reasons
        };
    }

    public static EligibilityAssessmentDto ToDto(this EligibilityAssessment assessment)
    {
        return new EligibilityAssessmentDto()
        {
            Identifier = assessment.Identifier,
            CreatedAt = assessment.CreatedAt,
            Prospect = assessment.Prospect,
            Qualifications = assessment.Qualifications.Select(x => x.ToDto()).ToList()
        };
    }

    public static EligibilityAssessmentQualificationDto ToDto(this EligibilityAssessmentQualification qualification)
    {
        return new EligibilityAssessmentQualificationDto()
        {
            QualificationIdentifier = qualification.QualificationIdentifier,
            QualificationCode = qualification.QualificationCode,
            QualificationName = qualification.QualificationName,
            IsEligible = qualification.IsEligible,
            Reasons = qualification.Reasons
        };
    }
}
