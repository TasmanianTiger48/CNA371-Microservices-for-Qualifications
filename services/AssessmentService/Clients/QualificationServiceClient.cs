namespace AssessmentService.Clients;

public class QualificationServiceClient : IQualificationServiceClient
{
    private readonly HttpClient _httpClient;

    public QualificationServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<QualificationAssessmentResponse>> AssessAsync(QualificationAssessmentRequest request, CancellationToken token)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "qualifications/assess",
            request,
            token);

        response.EnsureSuccessStatusCode();

        var results = await response.Content.ReadFromJsonAsync<List<QualificationAssessmentResponse>>(token);

        return results ?? [];
    }
}
