using Bogus;
using Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Codeflix.Catalog.EndToEndTests.Common;

public abstract class BaseFixture
{
  public BaseFixture() => Faker = new Faker("pt_BR");

  protected Faker Faker { get; set; }

  public CodeflixCatalogDbContext CreateDbContext()
  {
    var context = new CodeflixCatalogDbContext(
      new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
      .UseInMemoryDatabase("end2end-tests-db")
      .Options
    );

    return context;
  }
}
