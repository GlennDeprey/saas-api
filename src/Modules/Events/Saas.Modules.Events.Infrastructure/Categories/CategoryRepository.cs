using Saas.Modules.Events.Domain.Categories;
using Saas.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Saas.Modules.Events.Infrastructure.Categories;

internal sealed class CategoryRepository: ICategoryRepository
{
    private readonly EventsDbContext _context;
    public CategoryRepository(EventsDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Insert(Category category)
    {
        _context.Categories.Add(category);
    }
}
