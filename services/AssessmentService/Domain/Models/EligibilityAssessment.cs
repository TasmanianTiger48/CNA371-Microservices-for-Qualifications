namespace AssessmentService.Domain.Models;

public class EligibilityAssessment
{
    public Guid Identifier { get; set; }

    public DateTime CreatedAt { get; set; }

    public Prospect? Prospect { get; set; }

    public List<EligibilityAssessmentQualification> Qualifications { get; set; } = [];
}
