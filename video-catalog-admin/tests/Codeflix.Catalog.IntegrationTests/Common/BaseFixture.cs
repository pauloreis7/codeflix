using Bogus;

namespace Codeflix.Catalog.IntegrationTests.Common;

public abstract class BaseFixture
{
  public Faker Faker { get; set; }

  protected BaseFixture() => Faker = new Faker();
}
