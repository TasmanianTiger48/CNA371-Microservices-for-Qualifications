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
    public IActionResult GetAssessments(CancellationToken token)
    {
        var assessments = _assessmentService.GetAllEligibilityAssessments(token);
        return Ok(assessments);
    }

    [HttpGet("{id}")]
    public IActionResult GetAssessmentById(int id, CancellationToken token)
    {
        var assessment = _assessmentService.GetEligibilityAssessmentById(id, token);
        if (assessment == null)
        {
            return NotFound();
        }
        return Ok(assessment);
    }

    [HttpPost]
    public IActionResult CreateAssessment([FromBody] EligibilityAssessmentRequest request, CancellationToken token)
    {
        if (request == null)
            return BadRequest();

        var createdAssessment = _assessmentService.CreateEligibilityAssessment(request, token);
        return createdAssessment == null ? NoContent() : Ok(createdAssessment);
    }
}

