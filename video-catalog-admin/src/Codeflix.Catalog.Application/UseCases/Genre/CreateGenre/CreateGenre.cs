using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Genre.Common;
using Codeflix.Catalog.Domain.Repository;
using DomainEntity = Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;

public class CreateGenre : ICreateGenre
{
  private readonly IGenreRepository _genreRepository;
  private readonly IUnitOfWork _unitOfWork;

  public CreateGenre(
    IGenreRepository genreRepository,
    IUnitOfWork unitOfWork
  )
  {
    (_genreRepository, _unitOfWork) = (genreRepository, unitOfWork);
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

    request.CategoriesIds?.ForEach(genre.AddCategory);

    await _genreRepository.Insert(genre, cancellationToken);
    await _unitOfWork.Commit(cancellationToken);

    return GenreModelOutput.FromGenre(genre);
  }
}