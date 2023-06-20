using Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using Moq;
using UseCase = Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace Codeflix.Catalog.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
  private readonly UpdateCategoryTestFixture _fixture;

  public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
      => _fixture = fixture;

  [Fact(DisplayName = nameof(UpdateCategory))]
  [Trait("Application", "UpdateCategory - Use Cases")]
  public async Task UpdateCategory()
  {
    var repositoryMock = _fixture.GetRepositoryMock();
    var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
    var validCategory = _fixture.GetValidCategory();
    repositoryMock.Setup(x => x.Get(
      validCategory.Id,
      It.IsAny<CancellationToken>())
    ).ReturnsAsync(validCategory);
    var input = new UseCase.UpdateCategoryInput(
      validCategory.Id,
      _fixture.GetValidCategoryName(),
      _fixture.GetValidCategoryDescription(),
      !validCategory.IsActive
    );
    var useCase = new UseCase.UpdateCategory(
      repositoryMock.Object,
      unitOfWorkMock.Object
    );

    CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be(input.Description);
    output.IsActive.Should().Be(input.IsActive);
    repositoryMock.Verify(x => x.Get(
      validCategory.Id,
      It.IsAny<CancellationToken>()
    ), Times.Once);
    repositoryMock.Verify(x => x.Update(
      validCategory,
      It.IsAny<CancellationToken>()
    ), Times.Once);
    unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
  }
}
