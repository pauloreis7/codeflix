using Bogus;

namespace Codeflix.Catalog.UnitTests.Domain.Entity.Common;

public abstract class BaseFixture
{
  public Faker Faker { get; set; }

  protected BaseFixture() => Faker = new Faker();
}
