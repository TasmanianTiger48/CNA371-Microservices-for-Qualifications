namespace QualificationService.Domain.Models;

public class QualificationAdmissionPathway
{
    public Guid Identifier { get; init; }

    public string Code { get; init; }

    public string Name { get; init; }

    public List<QualificationAdmissionPathwayRequirement> Requirements { get; init; }
}
