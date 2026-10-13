namespace QualificationService.Contracts;

public class PathwayDto
{
    public string PathwayCode { get; set; } = string.Empty;

    public string PathwayName { get; set; } = string.Empty;

    public List<string> Reasons { get; set; } = [];
}
