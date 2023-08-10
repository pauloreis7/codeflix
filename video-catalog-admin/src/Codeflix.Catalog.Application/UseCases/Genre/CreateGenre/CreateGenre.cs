using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Genre.Common;
using Codeflix.Catalog.Domain.Repository;
using DomainEntity = Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;

public class CreateGenre : ICreateGenre
{
  private readonly ICategoryRepository _categoryRepository;
  private readonly IGenreRepository _genreRepository;
  private readonly IUnitOfWork _unitOfWork;

  public CreateGenre(
    IGenreRepository genreRepository,
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository
  )
  {
    (_genreRepository, _unitOfWork, _categoryRepository) = (genreRepository, unitOfWork, categoryRepository);
  }

  public async Task<GenreModelOutput> Handle(
    CreateGenreInput request,
    CancellationToken cancellationToken
  )
  {
    var genre = new DomainEntity.Genre(
      request.Name,
      request.IsActive
    );

    if ((request.CategoriesIds?.Count ?? 0) > 0)
    {
      await ValidateCategoriesIds(request, cancellationToken);

      request.CategoriesIds?.ForEach(genre.AddCategory);
    }

    await _genreRepository.Insert(genre, cancellationToken);
    await _unitOfWork.Commit(cancellationToken);

    return GenreModelOutput.FromGenre(genre);
  }

  private async Task ValidateCategoriesIds(
    CreateGenreInput request,
    CancellationToken cancellationToken
  )
  {
    var IdsInPersistence = await _categoryRepository
      .GetIdsListByIds(
        request.CategoriesIds!,
        cancellationToken
      );

    if (IdsInPersistence.Count < request.CategoriesIds!.Count)
    {
      var notFoundIds = request.CategoriesIds
        .FindAll(x => !IdsInPersistence.Contains(x));

      var notFoundIdsAsString = string.Join(", ", notFoundIds);

      throw new RelatedAggregateException(
        $"Related category ids not found: {notFoundIdsAsString}"
      );
    }
  }
}