using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Repository;
using Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace Codeflix.Catalog.Infra.Data.EF.Repositories;

public class CategoryRepository : ICategoryRepository
{
  private readonly CodeflixCatalogDbContext _context;

  private DbSet<Category> _categories => _context.Set<Category>();

  public CategoryRepository(CodeflixCatalogDbContext context)
    => _context = context;

  public async Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
  {
    var total = await _categories.CountAsync();
    var items = await _categories.ToListAsync();

    return new(input.Page, input.PerPage, total, items);
  }

  public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
  {
    var category = await _categories.AsNoTracking().FirstOrDefaultAsync(
      category => category.Id == id,
      cancellationToken
    );

    NotFoundException.ThrowIfNull(category, $"Category '{id}' not found");

    return category!;
  }

  public async Task Insert(
    Category aggregate,
    CancellationToken cancellationToken
  )
    => await _categories.AddAsync(aggregate, cancellationToken);

  public Task Update(Category aggregate, CancellationToken _)
    => Task.FromResult(_categories.Update(aggregate));

  public Task Delete(Category aggregate, CancellationToken _)
    => Task.FromResult(_categories.Remove(aggregate));
}
