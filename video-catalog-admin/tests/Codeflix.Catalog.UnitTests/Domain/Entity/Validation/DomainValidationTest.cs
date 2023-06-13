using FluentAssertions;
using Bogus;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.Domain.Validation;

namespace Codeflix.Catalog.UnitTests.Domain.Entity.Validation;

public class DomainValidationTest
{
  public Faker Faker { get; set; } = new Faker();

  [Fact(DisplayName = nameof(NotNullOk))]
  [Trait("Domain", "DomainValidation - Validation")]
  public void NotNullOk()
  {
    var value = Faker.Commerce.ProductName();

    Action action = () => DomainValidation.NotNull(value, "Value");

    action.Should().NotThrow();
  }

  [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
  [Trait("Domain", "DomainValidation - Validation")]
  public void NotNullThrowWhenNull()
  {
    string value = null;

    Action action = () => DomainValidation.NotNull(value, "FieldName");

    action.Should()
    .Throw<EntityValidationException>()
    .WithMessage("FieldName should not be null");
  }
}