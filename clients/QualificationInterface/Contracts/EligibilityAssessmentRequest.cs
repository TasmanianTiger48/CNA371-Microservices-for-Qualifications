namespace QualificationInterface.Contracts;

public class EligibilityAssessmentRequest
{
    public ProspectDto Prospect { get; set; } = new();

    public bool HasNationalSeniorCertificateOrEquivalent { get; set; }

    public List<EligibilityAssessmentRequestSubject> NationalSeniorCertificateOrEquivalentSubjects { get; set; } = [];

    public List<EligibilityAssessmentRequestCompletedQualification> CompletedQualifications { get; set; } = [];

    public bool HasBridgingCourseTemporaryExemption { get; set; }
}
