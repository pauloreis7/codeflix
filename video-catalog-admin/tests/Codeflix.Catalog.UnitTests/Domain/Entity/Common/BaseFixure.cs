using Bogus;

namespace Codeflix.Catalog.UnitTests.UnitTests.Common;

public abstract class BaseFixture
{
  public Faker Faker { get; set; }

  protected BaseFixture() => Faker = new Faker();
}
