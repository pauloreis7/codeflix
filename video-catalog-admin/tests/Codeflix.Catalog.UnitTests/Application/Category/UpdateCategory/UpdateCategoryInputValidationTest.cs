using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;

namespace Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryInputValidationTest
{
  private readonly UpdateCategoryTestFixture _fixture;

  public UpdateCategoryInputValidationTest(UpdateCategoryTestFixture fixture)
    => _fixture = fixture;

  [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
  [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
  public void DontValidateWhenEmptyGuid()
  {
    var input = _fixture.GetValidInput(Guid.Empty);
    var validator = new UpdateCategoryInputValidator();

    var validateResult = validator.Validate(input);

    validateResult.Should().NotBeNull();
    validateResult.IsValid.Should().BeFalse();
    validateResult.Errors.Should().HaveCount(1);
    validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
  }

  [Fact(DisplayName = nameof(ValidateWhenValid))]
  [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
  public void ValidateWhenValid()
  {
    var input = _fixture.GetValidInput();
    var validator = new UpdateCategoryInputValidator();

    var validateResult = validator.Validate(input);

    validateResult.Should().NotBeNull();
    validateResult.IsValid.Should().BeTrue();
    validateResult.Errors.Should().HaveCount(0);
  }
}