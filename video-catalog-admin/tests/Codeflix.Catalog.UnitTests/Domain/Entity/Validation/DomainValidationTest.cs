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
    string? value = null;

    Action action = () => DomainValidation.NotNull(value, "FieldName");

    action.Should()
    .Throw<EntityValidationException>()
    .WithMessage("FieldName should not be null");
  }

  [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
  [Trait("Domain", "DomainValidation - Validation")]
  [InlineData("")]
  [InlineData("   ")]
  [InlineData(null)]
  public void NotNullOrEmptyThrowWhenEmpty(string? target)
  {
    Action action = () => DomainValidation.NotNullOrEmpty(target, "FieldName");

    action.Should()
    .Throw<EntityValidationException>()
    .WithMessage("FieldName should not be null or empty");
  }

  [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
  [Trait("Domain", "DomainValidation - Validation")]
  public void NotNullOrEmptyOk()
  {
    var target = Faker.Commerce.ProductName();

    Action action = () => DomainValidation.NotNullOrEmpty(target, "FieldName");

    action.Should().NotThrow();
  }

  [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
  [Trait("Domain", "DomainValidation - Validation")]
  [InlineData("123", 5)]
  [InlineData("1234567890", 15)]
  [InlineData("abcdefg", 10)]
  [InlineData("Lorem ipsum dolor sit amet", 30)]
  [InlineData("consectetur adipiscing elit", 35)]
  public void MinLengthThrowWhenLess(string target, int minLength)
  {
    Action action = () => DomainValidation.MinLength(target, minLength, "FieldName");

    action.Should().Throw<EntityValidationException>().WithMessage($"FieldName should not be less than {minLength} characters long");
  }

  [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
  [Trait("Domain", "DomainValidation - Validation")]
  [InlineData("123", 2)]
  [InlineData("1234567890", 9)]
  [InlineData("abcdefg", 5)]
  [InlineData("Lorem ipsum dolor sit amet", 20)]
  [InlineData("consectetur adipiscing elit", 25)]
  public void MinLengthOk(string target, int minLength)
  {
    Action action = () => DomainValidation.MinLength(target, minLength, "FieldName");

    action.Should().NotThrow();
  }
}
