namespace AssessmentService.Contracts;

public class QualificationDto
{
    public Guid Identifier { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
