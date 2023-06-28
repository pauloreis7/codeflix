using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Codeflix.Catalog.Infra.Data.EF;
using Codeflix.Catalog.Infra.Data.EF.Repositories;
using DomainEntity = Codeflix.Catalog.Domain.Entity;
using ApplicationUseCase = Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Domain.Exceptions;

namespace Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
  private readonly UpdateCategoryTestFixture _fixture;

  public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
    => _fixture = fixture;

  [Theory(DisplayName = nameof(UpdateCategory))]
  [Trait("Integration/Application", "UpdateCategory - Use Cases")]
  [MemberData(nameof(GetCategoriesToUpdate))]
  public async Task UpdateCategory(
    DomainEntity.Category exampleCategory,
    UpdateCategoryInput input
  )
  {
    var dbContext = UpdateCategoryTestFixture.CreateDbContext();
    await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
    var trackingInfo = await dbContext.AddAsync(exampleCategory);
    dbContext.SaveChanges();
    trackingInfo.State = EntityState.Detached;
    var repository = new CategoryRepository(dbContext);
    var unitOfWork = new UnitOfWork(dbContext);
    var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

    var output = await useCase.Handle(input, CancellationToken.None);

    var dbCategory = await UpdateCategoryTestFixture.CreateDbContext(true)
      .Categories.FindAsync(output.Id);
    dbCategory.Should().NotBeNull();
    dbCategory!.Name.Should().Be(input.Name);
    dbCategory.Description.Should().Be(input.Description);
    dbCategory.IsActive.Should().Be((bool)input.IsActive!);
    dbCategory.CreatedAt.Should().Be(output.CreatedAt);
    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be(input.Description);
    output.IsActive.Should().Be((bool)input.IsActive!);
  }

  [Theory(DisplayName = nameof(UpdateCategoryWithoutIsActive))]
  [Trait("Integration/Application", "UpdateCategory - Use Cases")]
  [MemberData(nameof(GetCategoriesToUpdate))]
  public async Task UpdateCategoryWithoutIsActive(
    DomainEntity.Category exampleCategory,
    UpdateCategoryInput exampleInput
  )
  {
    var input = new UpdateCategoryInput(
      exampleInput.Id,
      exampleInput.Name,
      exampleInput.Description
    );
    var dbContext = UpdateCategoryTestFixture.CreateDbContext();
    await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
    var trackingInfo = await dbContext.AddAsync(exampleCategory);
    dbContext.SaveChanges();
    trackingInfo.State = EntityState.Detached;
    var repository = new CategoryRepository(dbContext);
    var unitOfWork = new UnitOfWork(dbContext);
    var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

    var output = await useCase.Handle(input, CancellationToken.None);

    var dbCategory = await UpdateCategoryTestFixture.CreateDbContext(true)
      .Categories.FindAsync(output.Id);
    dbCategory.Should().NotBeNull();
    dbCategory!.Name.Should().Be(input.Name);
    dbCategory.Description.Should().Be(input.Description);
    dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    dbCategory.CreatedAt.Should().Be(output.CreatedAt);
    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be(input.Description);
    output.IsActive.Should().Be(exampleCategory.IsActive);
  }

  [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
  [Trait("Integration/Application", "UpdateCategory - Use Cases")]
  [MemberData(nameof(GetCategoriesToUpdate))]
  public async Task UpdateCategoryOnlyName(
    DomainEntity.Category exampleCategory,
    UpdateCategoryInput exampleInput
  )
  {
    var input = new UpdateCategoryInput(
      exampleInput.Id,
      exampleInput.Name
    );
    var dbContext = UpdateCategoryTestFixture.CreateDbContext();
    await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
    var trackingInfo = await dbContext.AddAsync(exampleCategory);
    dbContext.SaveChanges();
    trackingInfo.State = EntityState.Detached;
    var repository = new CategoryRepository(dbContext);
    var unitOfWork = new UnitOfWork(dbContext);
    var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

    var output = await useCase.Handle(input, CancellationToken.None);

    var dbCategory = await UpdateCategoryTestFixture.CreateDbContext(true)
      .Categories.FindAsync(output.Id);
    dbCategory.Should().NotBeNull();
    dbCategory!.Name.Should().Be(input.Name);
    dbCategory.Description.Should().Be(exampleCategory.Description);
    dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    dbCategory.CreatedAt.Should().Be(output.CreatedAt);
    output.Should().NotBeNull();
    output.Name.Should().Be(input.Name);
    output.Description.Should().Be(exampleCategory.Description);
    output.IsActive.Should().Be(exampleCategory.IsActive);
  }

  [Fact(DisplayName = nameof(UpdateThrowsWhenNotFoundCategory))]
  [Trait("Integration/Application", "UpdateCategory - Use Cases")]
  public async Task UpdateThrowsWhenNotFoundCategory()
  {
    var input = _fixture.GetValidInput();
    var dbContext = UpdateCategoryTestFixture.CreateDbContext();
    await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
    dbContext.SaveChanges();
    var repository = new CategoryRepository(dbContext);
    var unitOfWork = new UnitOfWork(dbContext);
    var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

    var task = async () => await useCase.Handle(input, CancellationToken.None);

    await task.Should().ThrowAsync<NotFoundException>()
      .WithMessage($"Category '{input.Id}' not found");
  }

  [Fact(DisplayName = nameof(UpdateThrowsWhenCantInstantiateCategory))]
  [Trait("Integration/Application", "UpdateCategory - Use Cases")]
  public async Task UpdateThrowsWhenCantInstantiateCategory(

  )
  {
    var dbContext = UpdateCategoryTestFixture.CreateDbContext();
    var exampleCategories = _fixture.GetExampleCategoriesList();
    await dbContext.AddRangeAsync(exampleCategories);
    dbContext.SaveChanges();
    var repository = new CategoryRepository(dbContext);
    var unitOfWork = new UnitOfWork(dbContext);
    var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);
    var input = _fixture.GetInvalidInputShortName();
    input.Id = exampleCategories[0].Id;

    var task = async () => await useCase.Handle(input, CancellationToken.None);

    await task.Should().ThrowAsync<EntityValidationException>()
      .WithMessage("Name should be at least 3 characters long");
  }

  public static IEnumerable<object[]> GetCategoriesToUpdate()
  {
    var fixture = new UpdateCategoryTestFixture();
    for (int indice = 0; indice < 4; indice++)
    {
      var exampleCategory = fixture.GetExampleCategory();
      var exampleInput = fixture.GetValidInput(exampleCategory.Id);
      yield return new object[] {
        exampleCategory, exampleInput
      };
    }
  }
}
