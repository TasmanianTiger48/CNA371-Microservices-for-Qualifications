namespace AssessmentService.Infrastructure.Extensions;

public static class ProspectExtensions
{
    public static ProspectDto ToDto(this Prospect prospect)
    {
        return new ProspectDto
        {
            Identifier = prospect.Identifier,
            FirstName = prospect.FirstName,
            LastName = prospect.LastName,
            Email = prospect.Email
        };
    }
}
