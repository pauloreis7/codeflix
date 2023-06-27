using Codeflix.Catalog.Infra.Data.EF;
using Codeflix.Catalog.Infra.Data.EF.Repositories;
using ApplicationUseCases = Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FluentAssertions;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
  private readonly CreateCategoryTestFixture _fixture;

  public CreateCategoryTest(CreateCategoryTestFixture fixture)
    => _fixture = fixture;

  [Fact(DisplayName = nameof(CreateCategory))]
  [Trait("Integration/Application", "CreateCategory - Use Cases")]
  public async void CreateCategory()
  {
    var dbContext = CreateCategoryTestFixture.CreateDbContext();
    var repository = new CategoryRepository(dbContext);
    var unitOfWork = new UnitOfWork(dbContext);
    var useCase = new ApplicationUseCases.CreateCategory(
      repository,
      unitOfWork
    );
    var input = _fixture.GetInput();

    var output = await useCase.Handle(input, CancellationToken.None);

    var dbCategory = await CreateCategoryTestFixture.CreateDbContext(true)
      .Categories.FindAsync(output.Id);

    dbCategory.Should().NotBeNull();
    dbCategory!.Name.Should().Be(input.Name);
    dbCategory.Description.Should().Be(input.Description);
    dbCategory.IsActive.Should().Be(input.IsActive);
    dbCategory.CreatedAt.Should().Be(output.CreatedAt);
    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be(input.Description);
    output.IsActive.Should().Be(input.IsActive);
    output.Id.Should().NotBeEmpty();
    output.CreatedAt.Should().NotBeSameDateAs(default);
  }

  [Fact(DisplayName = nameof(CreateCategoryOnlyWithName))]
  [Trait("Integration/Application", "CreateCategory - Use Cases")]
  public async void CreateCategoryOnlyWithName()
  {
    var dbContext = CreateCategoryTestFixture.CreateDbContext();
    var repository = new CategoryRepository(dbContext);
    var unitOfWork = new UnitOfWork(dbContext);
    var useCase = new ApplicationUseCases.CreateCategory(
      repository,
      unitOfWork
    );
    var input = new CreateCategoryInput(_fixture.GetInput().Name);

    var output = await useCase.Handle(input, CancellationToken.None);

    var dbCategory = await CreateCategoryTestFixture.CreateDbContext(true)
      .Categories.FindAsync(output.Id);

    dbCategory.Should().NotBeNull();
    dbCategory!.Name.Should().Be(input.Name);
    dbCategory.Description.Should().Be("");
    dbCategory.IsActive.Should().Be(true);
    dbCategory.CreatedAt.Should().Be(output.CreatedAt);
    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be("");
    output.IsActive.Should().Be(true);
    output.Id.Should().NotBeEmpty();
    output.CreatedAt.Should().NotBeSameDateAs(default);
  }

  [Fact(DisplayName = nameof(CreateCategoryOnlyWithNameAndDescription))]
  [Trait("Integration/Application", "CreateCategory - Use Cases")]
  public async void CreateCategoryOnlyWithNameAndDescription()
  {
    var dbContext = CreateCategoryTestFixture.CreateDbContext();
    var repository = new CategoryRepository(dbContext);
    var unitOfWork = new UnitOfWork(dbContext);
    var useCase = new ApplicationUseCases.CreateCategory(
      repository,
      unitOfWork
    );
    var exampleInput = _fixture.GetInput();
    var input = new CreateCategoryInput(
      exampleInput.Name,
      exampleInput.Description
    );

    var output = await useCase.Handle(input, CancellationToken.None);

    var dbCategory = await CreateCategoryTestFixture.CreateDbContext(true)
      .Categories.FindAsync(output.Id);

    dbCategory.Should().NotBeNull();
    dbCategory!.Name.Should().Be(input.Name);
    dbCategory.Description.Should().Be(input.Description);
    dbCategory.IsActive.Should().Be(true);
    dbCategory.CreatedAt.Should().Be(output.CreatedAt);
    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be(input.Description);
    output.IsActive.Should().Be(true);
    output.Id.Should().NotBeEmpty();
    output.CreatedAt.Should().NotBeSameDateAs(default);
  }

  [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
  [Trait("Integration/Application", "CreateCategory - Use Cases")]
  [MemberData(nameof(GetInvalidInputs))]
  public async void ThrowWhenCantInstantiateCategory(
    CreateCategoryInput input,
    string expectedExceptionMessage
  )
  {
    var dbContext = CreateCategoryTestFixture.CreateDbContext();
    var repository = new CategoryRepository(dbContext);
    var unitOfWork = new UnitOfWork(dbContext);
    var useCase = new ApplicationUseCases.CreateCategory(
      repository,
      unitOfWork
    );

    var task = async () => await useCase.Handle(input, CancellationToken.None);

    await task.Should().ThrowAsync<EntityValidationException>()
      .WithMessage(expectedExceptionMessage);

    var dbCategoriesList = CreateCategoryTestFixture.CreateDbContext(true)
      .Categories.AsNoTracking()
      .ToList();

    dbCategoriesList.Should().HaveCount(0);
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
