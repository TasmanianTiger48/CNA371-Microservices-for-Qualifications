using System.Net.Http.Json;
using QualificationInterface.Contracts;
using QualificationInterface.Enums;

var baseUrl = args.Length > 0 ? args[0] : "http://localhost:5000";

using var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };

var request = new EligibilityAssessmentRequest
{
    Prospect = new ProspectDto
    {
        FirstName = Prompt("First name"),
        LastName = Prompt("Last name"),
        Email = Prompt("Email")
    },
    HasNationalSeniorCertificateOrEquivalent = PromptYesNo("Do you have a National Senior Certificate or equivalent?")
};

if (request.HasNationalSeniorCertificateOrEquivalent)
{
    Console.WriteLine("Enter your NSC subject marks (leave blank to skip a subject).");
    foreach (var subjectType in Enum.GetValues<SubjectTypes>())
    {
        var mark = PromptNumber($"  {subjectType} mark (%)");
        if (mark.HasValue)
            request.NationalSeniorCertificateOrEquivalentSubjects.Add(new EligibilityAssessmentRequestSubject
            {
                SubjectTypeId = (int)subjectType,
                Mark = mark.Value
            });
    }
}

foreach (var qualificationType in Enum.GetValues<QualificationTypes>())
{
    if (!PromptYesNo($"Have you completed a {qualificationType}?"))
        continue;

    request.CompletedQualifications.Add(qualificationType == QualificationTypes.BridgingCourse
        ? new EligibilityAssessmentRequestCompletedQualification
        {
            QualificationTypeId = (int)qualificationType,
            Mark = PromptNumber("  Final mark (%)")
        }
        : new EligibilityAssessmentRequestCompletedQualification
        {
            QualificationTypeId = (int)qualificationType,
            Credits = PromptNumber("  Credits")
        });
}

request.HasBridgingCourseTemporaryExemption = PromptYesNo("Do you have a Temporary Senate Discretionary Exemption?");

var response = await httpClient.PostAsJsonAsync("/eligibilityassessment", request);

if (!response.IsSuccessStatusCode)
{
    Console.WriteLine($"Assessment failed: {(int)response.StatusCode} {response.ReasonPhrase}");
    return;
}

var assessment = await response.Content.ReadFromJsonAsync<EligibilityAssessmentDto>();

if (assessment == null)
{
    Console.WriteLine("No assessment was returned.");
    return;
}

Console.WriteLine();
Console.WriteLine($"Assessment {assessment.Identifier} ({assessment.CreatedAt:u})");

foreach (var qualification in assessment.Qualifications)
{
    var status = qualification.IsEligible ? "Eligible" : "Not eligible";
    Console.WriteLine($"  [{qualification.QualificationCode}] {qualification.QualificationName}: {status}");

    for (var i = 0; i < qualification.ReasonsPerPathway.Count; i++)
    {
        Console.WriteLine($"    Pathway {i + 1}:");
        foreach (var reason in qualification.ReasonsPerPathway[i])
            Console.WriteLine($"      - {reason}");
    }
}

static string Prompt(string label)
{
    Console.Write($"{label}: ");
    return Console.ReadLine()?.Trim() ?? string.Empty;
}

static bool PromptYesNo(string label)
{
    return Prompt($"{label} (y/n)").StartsWith("y", StringComparison.OrdinalIgnoreCase);
}

static int? PromptNumber(string label)
{
    while (true)
    {
        var input = Prompt(label);
        if (input.Length == 0)
            return null;
        if (int.TryParse(input, out var value))
            return value;
        Console.WriteLine("  Please enter a whole number.");
    }
}
