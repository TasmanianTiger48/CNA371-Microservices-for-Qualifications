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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get(CancellationToken token)
    {
        var qualifications = await _qualificationService.GetQualifications(token);
        return Ok(qualifications);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get(Guid id, CancellationToken token)
    {
        var qualification = await _qualificationService.GetQualification(id, token);
        return qualification == null ? NoContent() : Ok(qualification);
    }

    [HttpPost("assess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(QualificationAssessmentRequest request, CancellationToken token)
    {
        var results = await _qualificationService.AssessQualificationsEligibility(request, token);
        return Ok(results);
    }
}