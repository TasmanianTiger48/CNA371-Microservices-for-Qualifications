namespace AssessmentService.Domain.Models;

public class Prospect
{
    public Guid Identifier { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}