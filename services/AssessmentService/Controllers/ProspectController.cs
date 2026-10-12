namespace AssessmentService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProspectController : ControllerBase
{
    private readonly IProspectService _prospectService;

    public ProspectController(IProspectService prospectService)
    {
        _prospectService = prospectService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProspectById(Guid id, CancellationToken token)
    {
        var prospect = await _prospectService.GetProspectById(id, token);
        return prospect == null ? NotFound() : Ok(prospect);
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetProspectByEmail(string email, CancellationToken token)
    {
        var prospect = await _prospectService.GetProspectByEmail(email, token);
        return prospect == null ? NotFound() : Ok(prospect);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProspect([FromBody] ProspectDto prospect, CancellationToken token)
    {
        if (prospect == null)
            return BadRequest();
        var createdProspect = await _prospectService.CreateProspect(prospect, token);
        return createdProspect == null ? NoContent() : Ok(createdProspect);
    }
}

