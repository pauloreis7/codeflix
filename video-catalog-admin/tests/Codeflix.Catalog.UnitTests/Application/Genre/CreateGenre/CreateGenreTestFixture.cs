using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.UnitTests.Application.Genre.Common;
using Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using Codeflix.Catalog.Domain.Repository;
using Moq;

namespace Codeflix.Catalog.UnitTests.Application.Genre.CreateGenre;

[CollectionDefinition(nameof(CreateGenreTestFixture))]
public class CreateGenreTestFixtureCollection
  : ICollectionFixture<CreateGenreTestFixture>
{ }

public class CreateGenreTestFixture : GenreUseCasesBaseFixture
{
  public CreateGenreInput GetExampleInput()
    => new(
      GetValidGenreName(),
      GetRandomBoolean()
    );

  public static Mock<IGenreRepository> GetGenreRepositoryMock()
    => new();

  public static Mock<IUnitOfWork> GetUnitOfWorkMock()
    => new();
}