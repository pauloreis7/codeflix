using Moq;
using DomainEntity = Codeflix.Catalog.Domain.Entity;
using UseCases = Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FluentAssertions;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Exceptions;

namespace Codeflix.Catalog.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
  private readonly CreateCategoryTestFixture _fixture;

  public CreateCategoryTest(CreateCategoryTestFixture fixture)
      => _fixture = fixture;

  [Fact(DisplayName = nameof(CreateCategory))]
  [Trait("Application", "CreateCategory - Use Cases")]
  public async void CreateCategory()
  {
    var repositoryMock = _fixture.GetRepositoryMock();
    var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
    var useCase = new UseCases.CreateCategory(
      repositoryMock.Object, unitOfWorkMock.Object
    );
    var input = _fixture.GetInput();

    var output = await useCase.Handle(input, CancellationToken.None);

    repositoryMock.Verify(
      repository => repository.Insert(
        It.IsAny<DomainEntity.Category>(),
        It.IsAny<CancellationToken>()
      ),
      Times.Once
    );
    unitOfWorkMock.Verify(
      uow => uow.Commit(It.IsAny<CancellationToken>()),
      Times.Once
    );

    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be(input.Description);
    output.IsActive.Should().Be(input.IsActive);
    output.Id.Should().NotBeEmpty();
    output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
  }

  [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
  [Trait("Application", "CreateCategory - Use Cases")]
  public async void CreateCategoryWithOnlyName()
  {
    var repositoryMock = _fixture.GetRepositoryMock();
    var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
    var useCase = new UseCases.CreateCategory(
      repositoryMock.Object, unitOfWorkMock.Object
    );
    var input = new UseCases.CreateCategoryInput(_fixture.GetValidCategoryName());

    var output = await useCase.Handle(input, CancellationToken.None);

    repositoryMock.Verify(
      repository => repository.Insert(
        It.IsAny<DomainEntity.Category>(),
        It.IsAny<CancellationToken>()
      ),
      Times.Once
    );
    unitOfWorkMock.Verify(
      uow => uow.Commit(It.IsAny<CancellationToken>()),
      Times.Once
    );

    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be("");
    output.IsActive.Should().BeTrue();
    output.Id.Should().NotBeEmpty();
    output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
  }

  [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
  [Trait("Application", "CreateCategory - Use Cases")]
  public async void CreateCategoryWithOnlyNameAndDescription()
  {
    var repositoryMock = _fixture.GetRepositoryMock();
    var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
    var useCase = new UseCases.CreateCategory(
      repositoryMock.Object, unitOfWorkMock.Object
    );
    var input = new CreateCategoryInput(
      _fixture.GetValidCategoryName(),
      _fixture.GetValidCategoryDescription()
    );

    var output = await useCase.Handle(input, CancellationToken.None);

    repositoryMock.Verify(
      repository => repository.Insert(
        It.IsAny<DomainEntity.Category>(),
        It.IsAny<CancellationToken>()
      ),
      Times.Once
    );
    unitOfWorkMock.Verify(
      uow => uow.Commit(It.IsAny<CancellationToken>()),
      Times.Once
    );

    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be(input.Description);
    output.IsActive.Should().BeTrue();
    output.Id.Should().NotBeEmpty();
    output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
  }

  [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
  [Trait("Application", "CreateCategory - Use Cases")]
  [MemberData(nameof(GetInvalidInputs))]
  public async void ThrowWhenCantInstantiateAggregate(
    CreateCategoryInput input,
    string exceptionMessage
  )
  {
    var useCase = new UseCases.CreateCategory(
      _fixture.GetRepositoryMock().Object,
      _fixture.GetUnitOfWorkMock().Object
    );

    var task = async () => await useCase.Handle(input, CancellationToken.None);

    await task.Should()
      .ThrowAsync<EntityValidationException>()
      .WithMessage(exceptionMessage);
  }

  private static IEnumerable<object[]> GetInvalidInputs()
  {
    var fixture = new CreateCategoryTestFixture();
    var invalidInputsList = new List<object[]>();
    var invalidInputShortName = fixture.GetInput();

    invalidInputShortName.Name = "ab";

    invalidInputsList.Add(new object[] {
      invalidInputShortName,
      "Name should be at least 3 characters long"
    });

    var invalidInputDescriptionNull = fixture.GetInput();
    invalidInputDescriptionNull.Description = null!;
    invalidInputsList.Add(new object[] {
      invalidInputDescriptionNull,
      "Description should not be null"
    });

    return invalidInputsList;
  }
}
