namespace QualificationService.Contracts;

public class QualificationAssessmentResponse
{
    public bool IsEligible { get; set; }

    public Guid QualificationIdentifier { get; set; }

    public string QualificationCode { get; set; } = string.Empty;

    public string QualificationName { get; set; } = string.Empty;

    public List<string> Reasons { get; set; } = [];
}
