namespace AssessmentService.Controllers;

[ApiController]
[Route("[controller]")]
public class EligibilityAssessmentController : ControllerBase
{
    private readonly IEligibilityAssessmentService _assessmentService;

    public EligibilityAssessmentController(IEligibilityAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAssessments(CancellationToken token)
    {
        var assessments = await _assessmentService.GetAllEligibilityAssessments(token);
        return Ok(assessments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssessmentById(int id, CancellationToken token)
    {
        var assessment = await _assessmentService.GetEligibilityAssessmentById(id, token);
        return assessment == null ? NotFound() : Ok(assessment);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAssessment([FromBody] EligibilityAssessmentRequest request, CancellationToken token)
    {
        if (request == null)
            return BadRequest();

        var createdAssessment = await _assessmentService.CreateEligibilityAssessment(request, token);
        return createdAssessment == null ? NoContent() : Ok(createdAssessment);
    }
}

