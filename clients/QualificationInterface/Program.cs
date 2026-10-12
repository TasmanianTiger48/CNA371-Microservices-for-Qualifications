using System.Net.Http.Json;
using QualificationInterface.Contracts;

var baseUrl = args.Length > 0 ? args[0] : "http://localhost:5000";

using var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };

var request = new EligibilityAssessmentRequest
{
    Prospect = new ProspectDto
    {
        Identifier = Guid.NewGuid(),
        FirstName = Prompt("First name"),
        LastName = Prompt("Last name"),
        Email = Prompt("Email")
    }
};

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

    foreach (var reason in qualification.Reasons)
        Console.WriteLine($"    - {reason}");
}

static string Prompt(string label)
{
    Console.Write($"{label}: ");
    return Console.ReadLine()?.Trim() ?? string.Empty;
}
