using CodeReviewInsight.Application.Models;

namespace CodeReviewInsight.Application.Repositories;

public interface IDayOff
{
    Task<IEnumerable<DayOff>> GetAllAsync(DateOnly from, DateOnly to);
}
