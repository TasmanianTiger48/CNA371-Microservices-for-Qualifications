namespace QualificationService.Contracts;

public class QualificationAssessmentRequest
{
    public bool HasNationalSeniorCertificateOrEquivalent { get; set; }

    public List<QualificationAssessmentRequestSubject> NationalSeniorCertificateOrEquivalentSubjects { get; set; }

    public List<QualificationAssessmentRequestCompletedQualification> CompletedQualifications { get; set; }

    public bool HasBridgingCourseTemporaryExemption { get; set; }
}
