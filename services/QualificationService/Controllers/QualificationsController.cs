namespace QualificationService.Controllers;

[ApiController]
[Route("[controller]")]
public class QualificationsController : ControllerBase
{
    private readonly IQualificationService _qualificationService;

    public QualificationsController(IQualificationService qualificationService)
    {
        _qualificationService = qualificationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QualificationDto>>> Get(CancellationToken token)
    {
        var qualifications = await _qualificationService.GetQualifications(token);
        return qualifications;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<QualificationDto>> Get(Guid id, CancellationToken token)
    {
        var qualification = await _qualificationService.GetQualification(id, token);
        return qualification == null ? NoContent() : qualification;
    }

    [HttpPost("assess")]
    public async Task<ActionResult<List<QualificationAssessmentResponse>>> Post(QualificationAssessmentRequest request, CancellationToken token)
    {
        var results = await _qualificationService.AssessQualificationsEligibility(request, token);
        return results;
    }
}