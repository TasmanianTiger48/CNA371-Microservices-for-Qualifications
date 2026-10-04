namespace QualificationService.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class QualificationController : ControllerBase
{
    private static readonly QualificationDto[] Qualifications =
    [
        new QualificationDto() {
            Identifier = new Guid("12345678-1234-1234-1234-123456789012"),
            Name = "Bachelor of Arts",
            Description = "This is a bachelor's degree in Arts."
        }
    ];

    [HttpGet]
    public IEnumerable<QualificationDto> Get()
    {
        return Qualifications;
    }

    [HttpGet("{id}")]
    public ActionResult<QualificationDto> Get(Guid id)
    {
        var qualification = Qualifications.FirstOrDefault(q => q.Identifier == id);
        return qualification == null ? NotFound() : qualification;
    }
}