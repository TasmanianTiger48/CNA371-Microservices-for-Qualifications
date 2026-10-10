namespace QualificationService.Domain.Models;

public class QualificationAdmissionPathway
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required List<QualificationAdmissionPathwayRequirement> Requirements { get; init; }

    public bool RequiresDiscretionaryApproval { get; init; }
}
