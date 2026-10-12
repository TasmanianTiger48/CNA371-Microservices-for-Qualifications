namespace QualificationService.Domain.Models;

public class Qualification
{
    public Guid Identifier { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public List<QualificationAdmissionPathway> Pathways { get; set; } = new List<QualificationAdmissionPathway>();
}
