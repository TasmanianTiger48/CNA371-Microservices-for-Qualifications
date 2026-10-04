namespace QualificationService.Models;

public class QualificationDto
{
    public Guid Identifier { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
