using Bogus;
using Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Codeflix.Catalog.EndToEndTests.Common;

public abstract class BaseFixture
{
  public BaseFixture()
  {
    Faker = new Faker();
    WebAppFactory = new CustomWebApplicationFactory<Program>();
    HttpClient = WebAppFactory.CreateClient();
    ApiClient = new ApiClient(HttpClient);
  }

  protected Faker Faker { get; set; }

  public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
  public HttpClient HttpClient { get; set; }
  public ApiClient ApiClient { get; set; }

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
