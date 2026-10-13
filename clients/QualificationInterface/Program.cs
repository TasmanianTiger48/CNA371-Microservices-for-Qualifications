using System.Net;
using System.Net.Http.Json;
using QualificationInterface.Contracts;
using QualificationInterface.Enums;

var baseUrl = args.Length > 0 ? args[0] : Environment.GetEnvironmentVariable("ASSESSMENT_SERVICE_URL") ?? "http://localhost:5000";

using var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 - New assessment");
    Console.WriteLine("2 - List all assessments");
    Console.WriteLine("3 - Assessments for a prospect");
    Console.WriteLine("4 - View assessment by ID");
    Console.WriteLine("5 - Clear");
    Console.WriteLine("0 - Exit");

    var option = Prompt("Choose an option (0-5): ");
    Console.WriteLine();

    try
    {
        switch (option)
        {
            case "1": await RunAssessment(httpClient); break;
            case "2": await ListAssessments(httpClient); break;
            case "3": await ListProspectAssessments(httpClient); break;
            case "4": await ViewAssessment(httpClient); break;
            case "5": Console.Clear(); break;
            case "0": return;
            default: Console.WriteLine("Unknown option."); break;
        }
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Could not reach the assessment service: {ex.Message}");
    }
}

static async Task RunAssessment(HttpClient httpClient)
{
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
        var qualificationTypePrompt = qualificationType == QualificationTypes.BridgingCourse
            ? "Bridging course"
            : $"{qualificationType} related to IT";

        if (!PromptYesNo($"Have you completed a {qualificationTypePrompt}?"))
            continue;

        var isBridgingCourse = qualificationType == QualificationTypes.BridgingCourse;

        request.CompletedQualifications.Add(isBridgingCourse
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

        if (isBridgingCourse)
        {
            request.HasBridgingCourseTemporaryExemption = PromptYesNo("  Do you have a Senate Discretionary Temporary Exemption?");
        }
    }

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
    PrintAssessment(assessment);
}

static async Task ListAssessments(HttpClient httpClient)
{
    var assessments = await httpClient.GetFromJsonAsync<List<EligibilityAssessmentDto>>("/eligibilityassessment");
    PrintAssessmentSummaries(assessments ?? []);
}

static async Task ListProspectAssessments(HttpClient httpClient)
{
    var email = Prompt("Prospect email");

    var response = await httpClient.GetAsync($"/eligibilityassessment/prospect/{Uri.EscapeDataString(email)}");

    if (response.StatusCode == HttpStatusCode.NotFound)
    {
        Console.WriteLine("No prospect found with that email.");
        return;
    }

    response.EnsureSuccessStatusCode();

    var assessments = await response.Content.ReadFromJsonAsync<List<EligibilityAssessmentDto>>();
    PrintAssessmentSummaries(assessments ?? []);
}

static async Task ViewAssessment(HttpClient httpClient)
{
    if (!Guid.TryParse(Prompt("Assessment ID"), out var identifier))
    {
        Console.WriteLine("That is not a valid assessment ID.");
        return;
    }

    var response = await httpClient.GetAsync($"/eligibilityassessment/{identifier}");

    if (response.StatusCode == HttpStatusCode.NotFound)
    {
        Console.WriteLine("No assessment found with that ID.");
        return;
    }

    response.EnsureSuccessStatusCode();

    var assessment = await response.Content.ReadFromJsonAsync<EligibilityAssessmentDto>();
    if (assessment != null)
        PrintAssessment(assessment);
}

static void PrintAssessment(EligibilityAssessmentDto assessment)
{
    Console.WriteLine($"Assessment {assessment.Identifier} ({assessment.CreatedAt:u})");

    if (assessment.Prospect != null)
        Console.WriteLine($"Prospect: {assessment.Prospect.FirstName} {assessment.Prospect.LastName} <{assessment.Prospect.Email}>");

    foreach (var qualification in assessment.Qualifications)
    {
        var status = qualification.IsEligible ? "Eligible" : "Not eligible";
        Console.WriteLine($"  [{qualification.QualificationCode}] {qualification.QualificationName}: {status}");

        for (var i = 0; i < qualification.ReasonsPerPathway.Count; i++)
        {
            Console.WriteLine($"    Pathway {i + 1}: {qualification.ReasonsPerPathway[i].PathwayName} ({qualification.ReasonsPerPathway[i].PathwayCode})");
            foreach (var reason in qualification.ReasonsPerPathway[i].Reasons)
                Console.WriteLine($"      - {reason}");
        }
    }
}

static void PrintAssessmentSummaries(List<EligibilityAssessmentDto> assessments)
{
    if (assessments.Count == 0)
    {
        Console.WriteLine("No assessments found.");
        return;
    }

    foreach (var assessment in assessments.OrderByDescending(a => a.CreatedAt))
    {
        var eligible = assessment.Qualifications.Where(q => q.IsEligible).Select(q => q.QualificationCode).ToList();
        var prospect = assessment.Prospect == null ? "Unknown prospect" : $"{assessment.Prospect.FirstName} {assessment.Prospect.LastName} <{assessment.Prospect.Email}>";

        Console.WriteLine($"{assessment.Identifier}  {assessment.CreatedAt:u}  {prospect}");
        Console.WriteLine($"  Eligible for: {(eligible.Count == 0 ? "none" : string.Join(", ", eligible))}");
    }
}

static string Prompt(string label)
{
    Console.Write($"{label}: ");

    var input = Console.ReadLine();
    if (input == null)
        Environment.Exit(0);

    return input.Trim();
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
