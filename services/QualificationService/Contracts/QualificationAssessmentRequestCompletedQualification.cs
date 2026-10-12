namespace QualificationService.Contracts;

public class QualificationAssessmentRequestCompletedQualification
{
    public int QualificationTypeId { get; set; }

    public int? Mark { get; set; }

    public int? Credits { get; set; }
}
