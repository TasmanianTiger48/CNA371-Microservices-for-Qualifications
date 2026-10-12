namespace QualificationService.Infrastructure.Repositories;

public class QualificationRepository : IQualificationRepository
{
    private readonly string _filePath;

    public QualificationRepository(IConfiguration configuration)
    {
        _filePath = configuration["Qualifications:FilePath"] ?? "Infrastructure/Repositories/Data/Qualifications.json";
    }
    
    public async Task<List<Qualification>> GetAll(CancellationToken token)
    {
        await using var stream = File.OpenRead(_filePath);

        var data = await JsonSerializer.DeserializeAsync<List<Qualification>>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
        },
            token);

        return data ?? [];
    }
}
