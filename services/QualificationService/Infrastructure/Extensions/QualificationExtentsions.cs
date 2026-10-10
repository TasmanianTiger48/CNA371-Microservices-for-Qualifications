namespace QualificationService.Infrastructure.Extensions;

public static class QualificationExtentsions
{
    public static QualificationDto ToDto(this Qualification qualification)
    {
        return new QualificationDto
        {
            Identifier = qualification.Identifier,
            Code = qualification.Code,
            Name = qualification.Name
        };
    }
}
