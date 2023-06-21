using Codeflix.Catalog.Application.Exceptions;
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
    output.IsActive.Should().Be((bool)input.IsActive!);
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

  [Fact(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
  [Trait("Application", "UpdateCategory - Use Cases")]
  public async Task UpdateCategoryWithoutProvidingIsActive()
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
      _fixture.GetValidCategoryDescription()
    );
    var useCase = new UseCase.UpdateCategory(
      repositoryMock.Object,
      unitOfWorkMock.Object
    );

    CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be(input.Description);
    output.IsActive.Should().Be(validCategory.IsActive);
    repositoryMock.Verify(x => x.Get(
      validCategory.Id,
      It.IsAny<CancellationToken>()), Times.Once);
    repositoryMock.Verify(x => x.Update(
      validCategory,
      It.IsAny<CancellationToken>()), Times.Once);
    unitOfWorkMock.Verify(x => x.Commit(
      It.IsAny<CancellationToken>()), Times.Once
    );
  }

  [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
  [Trait("Application", "UpdateCategory - Use Cases")]
  public async Task UpdateCategoryOnlyName()
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
      _fixture.GetValidCategoryName()
    );
    var useCase = new UseCase.UpdateCategory(
      repositoryMock.Object,
      unitOfWorkMock.Object
    );

    CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be(validCategory.Description);
    output.IsActive.Should().Be(validCategory.IsActive);
    repositoryMock.Verify(x => x.Get(
      validCategory.Id,
      It.IsAny<CancellationToken>()), Times.Once);
    repositoryMock.Verify(x => x.Update(
      validCategory,
      It.IsAny<CancellationToken>()), Times.Once);
    unitOfWorkMock.Verify(x => x.Commit(
      It.IsAny<CancellationToken>()), Times.Once
    );
  }

  [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
  [Trait("Application", "UpdateCategory - Use Cases")]
  public async Task ThrowWhenCategoryNotFound()
  {
    var repositoryMock = _fixture.GetRepositoryMock();
    var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
    var input = _fixture.GetValidInput();
    repositoryMock.Setup(x => x.Get(
      input.Id,
      It.IsAny<CancellationToken>())
    ).ThrowsAsync(new NotFoundException($"Category '{input.Id}' not found"));
    var useCase = new UseCase.UpdateCategory(
      repositoryMock.Object,
      unitOfWorkMock.Object
    );

    var task = async ()
      => await useCase.Handle(input, CancellationToken.None);

    await task.Should().ThrowAsync<NotFoundException>();

    repositoryMock.Verify(x => x.Get(
      input.Id,
      It.IsAny<CancellationToken>()
    ), Times.Once);
  }
}
