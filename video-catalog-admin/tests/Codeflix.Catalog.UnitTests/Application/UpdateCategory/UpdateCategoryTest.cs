using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Codeflix.Catalog.Domain.Exceptions;
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

  [Theory(DisplayName = nameof(ThrowWhenCantUpdateCategory))]
  [Trait("Application", "UpdateCategory - Use Cases")]
  [MemberData(nameof(GetInvalidInputs))]
  public async Task ThrowWhenCantUpdateCategory(
    UpdateCategoryInput input,
    string expectedExceptionMessage
  )
  {
    var exampleCategory = _fixture.GetValidCategory();
    input.Id = exampleCategory.Id;
    var repositoryMock = _fixture.GetRepositoryMock();
    var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
    repositoryMock.Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()))
      .ReturnsAsync(exampleCategory);
    var useCase = new UseCase.UpdateCategory(
      repositoryMock.Object,
      unitOfWorkMock.Object
    );

    var task = async ()
      => await useCase.Handle(input, CancellationToken.None);

    await task.Should().ThrowAsync<EntityValidationException>()
      .WithMessage(expectedExceptionMessage);

    repositoryMock.Verify(x => x.Get(
      exampleCategory.Id,
      It.IsAny<CancellationToken>()
    ), Times.Once);
  }

  private static IEnumerable<object[]> GetInvalidInputs()
  {
    var fixture = new UpdateCategoryTestFixture();
    var invalidInputsList = new List<object[]>();
    var invalidInputShortName = fixture.GetValidInput();

    invalidInputShortName.Name = "ab";

    invalidInputsList.Add(new object[] {
      invalidInputShortName,
      "Name should be at least 3 characters long"
    });

    var invalidInputDescriptionNull = fixture.GetValidInput();
    invalidInputDescriptionNull.Description = fixture.GetInvalidTooLongDescription();
    invalidInputsList.Add(new object[] {
      invalidInputDescriptionNull,
      "Description should be less or equal 10000 characters long"
    });

    return invalidInputsList;
  }
}
