namespace QualificationInterface.Contracts;

public class EligibilityAssessmentQualificationDto
{
    public Guid QualificationIdentifier { get; set; }

    public string QualificationCode { get; set; } = string.Empty;

    public string QualificationName { get; set; } = string.Empty;

    public bool IsEligible { get; set; }

    public List<PathwayDto> ReasonsPerPathway { get; set; } = [];
}
