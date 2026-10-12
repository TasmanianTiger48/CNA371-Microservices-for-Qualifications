namespace QualificationService.Domain.Models;

public class QualificationAdmissionPathwayRequirement
{
    public RequirementTypes Type { get; set; }

    public string? SubjectTypeName { get; init; }

    public decimal? Minimum { get; init; }

    public decimal? Maximum { get; init; }

    public string Description { get; init; } = string.Empty;
}
