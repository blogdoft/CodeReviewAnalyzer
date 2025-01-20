using CodeReviewAnalyzer.Application.Models;

namespace CodeReviewAnalyzer.Application.Repositories;

public interface IDayOff
{
    Task<IEnumerable<DayOff>> GetAllAsync(DateOnly from, DateOnly to);
}
