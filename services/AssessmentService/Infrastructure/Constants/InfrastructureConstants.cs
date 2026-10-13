namespace AssessmentService.Infrastructure.Constants;

public static class InfrastructureConstants
{
    public const string AssessmentsKey = "eligibility-assessments";
    public static string AssessmentKey(object identifier) => $"eligibility-assessment:{identifier}";

    public static string ProspectKey(Guid identifier) => $"prospect:{identifier}";
    public static string ProspectEmailKey(string email) => $"prospect-email:{email.Trim().ToLowerInvariant()}";
    public static string ProspectAssessmentsKey(Guid identifier) => $"prospect-eligibility-assessments:{identifier}";
}
