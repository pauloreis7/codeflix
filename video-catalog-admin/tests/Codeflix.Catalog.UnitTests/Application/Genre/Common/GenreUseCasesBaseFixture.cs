using Codeflix.Catalog.UnitTests.Common;
using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Domain.Repository;
using DomainEntity = Codeflix.Catalog.Domain.Entity;
using Moq;

namespace Codeflix.Catalog.UnitTests.Application.Genre.Common;
public class GenreUseCasesBaseFixture : BaseFixture
{
  public string GetValidGenreName()
    => Faker.Commerce.Categories(1)[0];

  public DomainEntity.Genre GetExampleGenre()
    => new(
      GetValidGenreName(),
      GetRandomBoolean()
    );

  public static Mock<IGenreRepository> GetGenreRepositoryMock()
  => new();

  public static Mock<IUnitOfWork> GetUnitOfWorkMock()
    => new();

  public static Mock<ICategoryRepository> GetCategoryRepositoryMock()
    => new();
}
