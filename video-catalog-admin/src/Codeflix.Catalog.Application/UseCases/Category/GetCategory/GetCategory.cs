using MediatR;
using Codeflix.Catalog.Domain.Repository;

namespace Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategory : IRequestHandler<GetCategoryInput, GetCategoryOutput>
{
  private readonly ICategoryRepository _categoryRepository;

  public GetCategory(ICategoryRepository categoryRepository)
    => _categoryRepository = categoryRepository;

  public async Task<GetCategoryOutput> Handle(
    GetCategoryInput request,
    CancellationToken cancellationToken
  )
  {
    var category = await _categoryRepository.Get(request.Id, cancellationToken);
    return GetCategoryOutput.FromCategory(category);
  }
}
