namespace QualificationService.Infrastructure.Repositories;

public interface IQualificationRepository
{
    Task<List<Qualification>> GetAll(CancellationToken token);
}
