using UnitOfWorkInfra = Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace Codeflix.Catalog.IntegrationTests.Infra.Data.EF.UnitOfWork;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest
{
  private readonly UnitOfWorkTestFixture _fixture;

  public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
      => _fixture = fixture;

  [Fact(DisplayName = nameof(Commit))]
  [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
  public async Task Commit()
  {
    var dbContext = UnitOfWorkTestFixture.CreateDbContext();
    var exampleCategoriesList = _fixture.GetExampleCategoriesList();
    await dbContext.AddRangeAsync(exampleCategoriesList);
    var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

    await unitOfWork.Commit(CancellationToken.None);

    var assertDbContext = UnitOfWorkTestFixture.CreateDbContext(true);
    var savedCategories = assertDbContext.Categories.AsNoTracking().ToList();
    savedCategories.Should().HaveCount(exampleCategoriesList.Count);
  }


  [Fact(DisplayName = nameof(Rollback))]
  [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
  public async Task Rollback()
  {
    var dbContext = UnitOfWorkTestFixture.CreateDbContext();
    var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

    var task = async () => await unitOfWork.Rollback(CancellationToken.None);

    await task.Should().NotThrowAsync();
  }
}
