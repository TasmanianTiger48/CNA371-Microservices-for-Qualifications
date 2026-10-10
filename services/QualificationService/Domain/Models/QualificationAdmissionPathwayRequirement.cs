namespace QualificationService.Domain.Models;

public class QualificationAdmissionPathwayRequirement
{
    public required string Id { get; init; }

    public required string Description { get; init; }

    public required RequirementTypes Type { get; init; }

    public bool IsMandatory { get; init; } = true;

    public string? Subject { get; init; }

    public decimal? MinimumValue { get; init; }

    public decimal? MaximumValue { get; init; }

    public string? ExpectedValue { get; init; }
}
