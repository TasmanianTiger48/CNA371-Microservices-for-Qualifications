namespace QualificationService.Infrastructure.Extensions;

public static class QualificationExtentsions
{
    public static QualificationDto ToDto(this Qualification qualification)
    {
        return new QualificationDto
        {
            Identifier = qualification.Identifier,
            Code = qualification.Code,
            Name = qualification.Name
        };
    }

    public static bool ValidateNationalSeniorCertificateOrEquivalentMark(this QualificationAssessmentRequest request, QualificationAdmissionPathwayRequirement rule)
    {
        var ruleSubjectType = rule.SubjectTypeName?.ToSubjectType();

        var hasMatchingSubject = request.NationalSeniorCertificateOrEquivalentSubjects.Any(s =>
            ruleSubjectType != null
            && s.SubjectTypeId == (int)ruleSubjectType
            && (rule.Minimum == null || s.Mark >= rule.Minimum)
            && (rule.Maximum == null || s.Mark <= rule.Maximum)
        );

        return request.HasNationalSeniorCertificateOrEquivalent && hasMatchingSubject;
    }

    public static bool ValidateBridgingCourse(this QualificationAssessmentRequest request, QualificationAdmissionPathwayRequirement rule)
    {
        var bridgingCourses = request.CompletedQualifications
            .Where(q => q.QualificationTypeId == (int)QualificationTypes.BridgingCourse)
            .ToList();

        return bridgingCourses != null
            && bridgingCourses.Any(x =>
            {
                return x.Mark.HasValue
                    && (rule.Minimum == null || x.Mark >= rule.Minimum)
                    && (rule.Maximum == null || x.Mark <= rule.Maximum);
            });
    }

    public static bool ValidateCompletedQualification(this QualificationAssessmentRequest request, QualificationAdmissionPathwayRequirement rule)
    {
        var completedQualifications = request.CompletedQualifications
            .Where(q => (QualificationTypes)q.QualificationTypeId switch
            {
                QualificationTypes.Diploma => rule.Type == RequirementTypes.Diploma,
                QualificationTypes.AdvancedDiploma => rule.Type == RequirementTypes.AdvancedDiploma,
                QualificationTypes.BachelorDegree => rule.Type == RequirementTypes.BachelorDegree,
                _ => false
            })
            .ToList();

        return completedQualifications != null
            && completedQualifications.Any(x =>
            {
                return (rule.Minimum == null || x.Credits >= rule.Minimum)
                    && (rule.Maximum == null || x.Credits <= rule.Maximum);
            });
    }
}
