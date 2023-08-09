using Bogus;

namespace Codeflix.Catalog.UnitTests.Common;

public abstract class BaseFixture
{
  public Faker Faker { get; set; }

  protected BaseFixture() => Faker = new Faker();

  public static bool GetRandomBoolean()
    => new Random().NextDouble() < 0.5;
}
