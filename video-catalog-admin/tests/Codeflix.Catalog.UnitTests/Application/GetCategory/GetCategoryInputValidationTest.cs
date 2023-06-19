using Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FluentAssertions;

namespace Codeflix.Catalog.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryInputValidatorTest
{
  private readonly GetCategoryTestFixture _fixture;

  public GetCategoryInputValidatorTest(GetCategoryTestFixture fixture)
      => _fixture = fixture;

  [Fact(DisplayName = nameof(ValidationOk))]
  [Trait("Application", "GetCategoryInputValidation - UseCases")]
  public void ValidationOk()
  {
    var validInput = new GetCategoryInput(Guid.NewGuid());
    var validator = new GetCategoryInputValidator();

    var validationResult = validator.Validate(validInput);

    validationResult.Should().NotBeNull();
    validationResult.IsValid.Should().BeTrue();
    validationResult.Errors.Should().HaveCount(0);
  }
}
