namespace AssessmentService.Domain.Models;

public class EligibilityAssessmentQualification
{
    public Guid QualificationIdentifier { get; set; }

    public string QualificationCode { get; set; } = string.Empty;

    public string QualificationName { get; set; } = string.Empty;

    public bool IsEligible { get; set; }

    public List<string> Reasons { get; set; } = [];
}
