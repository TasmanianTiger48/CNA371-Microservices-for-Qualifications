namespace QualificationService.Infrastructure.Extensions;

public static class SubjectTypeExtensions
{
    public static SubjectTypes? ToSubjectType(this string subjectTypeName)
    {
        return subjectTypeName switch
        {
            "Mathematics" => SubjectTypes.Mathematics,
            "Physical Sciences" => SubjectTypes.PhysicalSciences,
            "Life Sciences" => SubjectTypes.LifeSciences,
            "Accounting" => SubjectTypes.Accounting,
            "Business Studies" => SubjectTypes.BusinessStudies,
            "Economics" => SubjectTypes.Economics,
            _ => null
        };
    }
}
