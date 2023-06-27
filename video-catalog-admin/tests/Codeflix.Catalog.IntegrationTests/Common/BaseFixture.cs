using Bogus;
using Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Codeflix.Catalog.IntegrationTests.Common;

public abstract class BaseFixture
{
  public Faker Faker { get; set; }

  protected BaseFixture() => Faker = new Faker();

  public static CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
  {
    var context = new CodeflixCatalogDbContext(
      new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
      .UseInMemoryDatabase("integration-tests-db")
      .Options
    );

    if (preserveData == false)
      context.Database.EnsureDeleted();

    return context;
  }
}
