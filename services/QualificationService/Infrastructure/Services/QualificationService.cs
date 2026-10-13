namespace QualificationService.Infrastructure.Services;

public class QualificationService : IQualificationService
{
    IQualificationRepository _qualificationRepository;

    public QualificationService(IQualificationRepository qualificationRepository)
    {
        _qualificationRepository = qualificationRepository;
    }

    public async Task<List<QualificationAssessmentResponse>> AssessQualificationsEligibility(QualificationAssessmentRequest request, CancellationToken token)
    {
        var qualifications = await _qualificationRepository.GetAll(token);

        var results = new List<QualificationAssessmentResponse>();

        foreach (var qualification in qualifications)
        {
            var anyPathwayEligible = false;
            List<PathwayDto> missingRequirementsPerPathway = [];
            foreach (var admissionPathway in qualification.Pathways)
            {
                var isEligible = true;
                var missingPathwayRequirements = new List<string>();
                admissionPathway.Requirements
                    .ForEach(requirement =>
                    {
                        switch (requirement.Type)
                        {
                            case RequirementTypes.NationalSeniorCertificateOrEquivalent:
                                if (!request.HasNationalSeniorCertificateOrEquivalent)
                                {
                                    isEligible = false;
                                    missingPathwayRequirements.Add(requirement.Description);
                                }
                                break;
                            case RequirementTypes.NationalSeniorCertificateOrEquivalentMark:
                                if (!request.ValidateNationalSeniorCertificateOrEquivalentMark(requirement))
                                {
                                    isEligible = false;
                                    missingPathwayRequirements.Add(requirement.Description);
                                }
                                break;
                            case RequirementTypes.BridgingCourse:
                                if (!request.ValidateBridgingCourse(requirement))
                                {
                                    isEligible = false;
                                    missingPathwayRequirements.Add(requirement.Description);
                                }
                                break;
                            case RequirementTypes.HasBridgingCourseTemporaryExemption:
                                if (!request.HasBridgingCourseTemporaryExemption)
                                {
                                    isEligible = false;
                                    missingPathwayRequirements.Add(requirement.Description);
                                }
                                break;
                            case RequirementTypes.Diploma:
                            case RequirementTypes.AdvancedDiploma:
                            case RequirementTypes.BachelorDegree:
                                if (!request.ValidateCompletedQualification(requirement))
                                {
                                    isEligible = false;
                                    missingPathwayRequirements.Add(requirement.Description);
                                }
                                break;

                            default:
                                throw new NotImplementedException($"Requirement type {requirement.Type} not implemented");
                        }
                    });

                if (isEligible)
                {
                    anyPathwayEligible = true;
                    break;
                }
                else
                {
                    missingRequirementsPerPathway.Add(new PathwayDto
                    {
                        PathwayCode = admissionPathway.Code,
                        PathwayName = admissionPathway.Name,
                        Reasons = missingPathwayRequirements
                    });
                }
            }

            var result = new QualificationAssessmentResponse
            {
                IsEligible = anyPathwayEligible,
                QualificationIdentifier = qualification.Identifier,
                QualificationCode = qualification.Code,
                QualificationName = qualification.Name,
                ReasonsPerPathway = anyPathwayEligible ? [] : missingRequirementsPerPathway
            };
            results.Add(result);
        }

        return results;
    }

    public async Task<QualificationDto?> GetQualification(Guid identifier, CancellationToken token)
    {
        var qualifications = await _qualificationRepository.GetAll(token);

        return qualifications?.FirstOrDefault(q => q.Identifier == identifier)?.ToDto();
    }

    public async Task<List<QualificationDto>> GetQualifications(CancellationToken token)
    {
        var qualifications = await _qualificationRepository.GetAll(token);

        return qualifications.Select(q => q.ToDto()).ToList();
    }
}
