namespace AssessmentService.Contracts;

public class EligibilityAssessmentDto
{
    public Guid Identifier { get; set; }

    public DateTime CreatedAt { get; set; }

    public Prospect? Prospect { get; set; }

    public List<EligibilityAssessmentQualificationDto> Qualifications { get; set; } = [];
}
