using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.UnitTests.Application.Genre.Common;
using Moq;

namespace Codeflix.Catalog.UnitTests.Application.Genre.CreateGenre;

[CollectionDefinition(nameof(CreateGenreTestFixture))]
public class CreateGenreTestFixtureCollection
  : ICollectionFixture<CreateGenreTestFixture>
{ }

public class CreateGenreTestFixture : GenreUseCasesBaseFixture
{
  public CreateGenreInput GetExampleInput()
    => new CreateGenreInput(
      GetValidGenreName(),
      GetRandomBoolean()
    );

  public Mock<IGenreRepository> GetGenreRepositoryMock()
    => new();

  public Mock<IUnitOfWork> GetUnitOfWorkMock()
    => new();
}