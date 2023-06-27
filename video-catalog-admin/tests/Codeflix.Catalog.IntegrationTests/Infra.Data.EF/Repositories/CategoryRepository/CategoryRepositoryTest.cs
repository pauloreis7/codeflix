using FluentAssertions;
using Codeflix.Catalog.Infra.Data.EF;
using Repository = Codeflix.Catalog.Infra.Data.EF.Repositories;
using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTest
{
  private readonly CategoryRepositoryTestFixture _fixture;

  public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
    => _fixture = fixture;

  [Fact(DisplayName = nameof(Insert))]
  [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
  public async Task Insert()
  {
    CodeflixCatalogDbContext dbContext = CategoryRepositoryTestFixture.CreateDbContext();
    var exampleCategory = _fixture.GetExampleCategory();
    var categoryRepository = new Repository.CategoryRepository(dbContext);

    await categoryRepository.Insert(exampleCategory, CancellationToken.None);
    await dbContext.SaveChangesAsync(CancellationToken.None);

    var dbCategory = await CategoryRepositoryTestFixture.CreateDbContext(true)
      .Categories.FindAsync(exampleCategory.Id);

    dbCategory.Should().NotBeNull();
    dbCategory!.Name.Should().Be(exampleCategory.Name);
    dbCategory.Description.Should().Be(exampleCategory.Description);
    dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
  }

  [Fact(DisplayName = nameof(Get))]
  [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
  public async Task Get()
  {
    CodeflixCatalogDbContext dbContext = CategoryRepositoryTestFixture.CreateDbContext();
    var exampleCategory = _fixture.GetExampleCategory();
    var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);

    exampleCategoriesList.Add(exampleCategory);
    await dbContext.AddRangeAsync(exampleCategoriesList);
    await dbContext.SaveChangesAsync(CancellationToken.None);

    var categoryRepository = new Repository.CategoryRepository(
      CategoryRepositoryTestFixture.CreateDbContext(true)
    );

    var dbCategory = await categoryRepository.Get(
      exampleCategory.Id,
      CancellationToken.None
    );

    dbCategory.Should().NotBeNull();
    dbCategory.Name.Should().Be(exampleCategory.Name);
    dbCategory.Id.Should().Be(exampleCategory.Id);
    dbCategory.Description.Should().Be(exampleCategory.Description);
    dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
  }

  [Fact(DisplayName = nameof(GetThrowIfNotFound))]
  [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
  public async Task GetThrowIfNotFound()
  {
    CodeflixCatalogDbContext dbContext = CategoryRepositoryTestFixture.CreateDbContext();
    var exampleId = Guid.NewGuid();
    await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList(5));
    await dbContext.SaveChangesAsync(CancellationToken.None);

    var categoryRepository = new Repository.CategoryRepository(dbContext);

    var task = async () => await categoryRepository.Get(
      exampleId,
      CancellationToken.None
    );

    await task.Should().ThrowAsync<NotFoundException>()
      .WithMessage($"Category '{exampleId}' not found");
  }

  [Fact(DisplayName = nameof(Update))]
  [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
  public async Task Update()
  {
    CodeflixCatalogDbContext dbContext = CategoryRepositoryTestFixture.CreateDbContext();
    var exampleCategory = _fixture.GetExampleCategory();
    var newCategoryValues = _fixture.GetExampleCategory();
    var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
    exampleCategoriesList.Add(exampleCategory);
    await dbContext.AddRangeAsync(exampleCategoriesList);
    await dbContext.SaveChangesAsync(CancellationToken.None);

    var categoryRepository = new Repository.CategoryRepository(dbContext);

    exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);
    await categoryRepository.Update(exampleCategory, CancellationToken.None);
    await dbContext.SaveChangesAsync();

    var dbCategory = await CategoryRepositoryTestFixture.CreateDbContext(true)
      .Categories.FindAsync(exampleCategory.Id);

    dbCategory.Should().NotBeNull();
    dbCategory!.Name.Should().Be(exampleCategory.Name);
    dbCategory.Id.Should().Be(exampleCategory.Id);
    dbCategory.Description.Should().Be(exampleCategory.Description);
    dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
  }

  [Fact(DisplayName = nameof(Delete))]
  [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
  public async Task Delete()
  {
    CodeflixCatalogDbContext dbContext = CategoryRepositoryTestFixture.CreateDbContext();
    var exampleCategory = _fixture.GetExampleCategory();
    var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
    exampleCategoriesList.Add(exampleCategory);
    await dbContext.AddRangeAsync(exampleCategoriesList);
    await dbContext.SaveChangesAsync(CancellationToken.None);
    var categoryRepository = new Repository.CategoryRepository(dbContext);

    await categoryRepository.Delete(exampleCategory, CancellationToken.None);
    await dbContext.SaveChangesAsync();

    var dbCategory = await CategoryRepositoryTestFixture.CreateDbContext(true)
      .Categories.FindAsync(exampleCategory.Id);

    dbCategory.Should().BeNull();
  }

  [Fact(DisplayName = nameof(SearchRetursListAndTotal))]
  [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
  public async Task SearchRetursListAndTotal()
  {
    CodeflixCatalogDbContext dbContext = CategoryRepositoryTestFixture.CreateDbContext();
    var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
    await dbContext.AddRangeAsync(exampleCategoriesList);
    await dbContext.SaveChangesAsync(CancellationToken.None);
    var categoryRepository = new Repository.CategoryRepository(dbContext);
    var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

    var output = await categoryRepository.Search(searchInput, CancellationToken.None);

    output.Should().NotBeNull();
    output.Items.Should().NotBeNull();
    output.CurrentPage.Should().Be(searchInput.Page);
    output.PerPage.Should().Be(searchInput.PerPage);
    output.Total.Should().Be(exampleCategoriesList.Count);
    output.Items.Should().HaveCount(exampleCategoriesList.Count);

    foreach (Category outputItem in output.Items)
    {
      var exampleItem = exampleCategoriesList.Find(
        category => category.Id == outputItem.Id
      );
      exampleItem.Should().NotBeNull();
      outputItem.Name.Should().Be(exampleItem!.Name);
      outputItem.Description.Should().Be(exampleItem.Description);
      outputItem.IsActive.Should().Be(exampleItem.IsActive);
      outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
    }
  }

  [Fact(DisplayName = nameof(SearchRetursEmptyWhenPersistenceIsEmpty))]
  [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
  public async Task SearchRetursEmptyWhenPersistenceIsEmpty()
  {
    CodeflixCatalogDbContext dbContext = CategoryRepositoryTestFixture.CreateDbContext();
    var categoryRepository = new Repository.CategoryRepository(dbContext);
    var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

    var output = await categoryRepository.Search(searchInput, CancellationToken.None);

    output.Should().NotBeNull();
    output.Items.Should().NotBeNull();
    output.CurrentPage.Should().Be(searchInput.Page);
    output.PerPage.Should().Be(searchInput.PerPage);
    output.Total.Should().Be(0);
    output.Items.Should().HaveCount(0);
  }

  [Theory(DisplayName = nameof(SearchRetursListAndTotal))]
  [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
  [InlineData(10, 1, 5, 5)]
  [InlineData(10, 2, 5, 5)]
  [InlineData(7, 2, 5, 2)]
  [InlineData(7, 3, 5, 0)]
  public async Task SearchRetursPaginated(
      int quantityCategoriesToGenerate,
      int page,
      int perPage,
      int expectedQuantityItems
  )
  {
    CodeflixCatalogDbContext dbContext = CategoryRepositoryTestFixture.CreateDbContext();
    var exampleCategoriesList =
      _fixture.GetExampleCategoriesList(quantityCategoriesToGenerate);
    await dbContext.AddRangeAsync(exampleCategoriesList);
    await dbContext.SaveChangesAsync(CancellationToken.None);
    var categoryRepository = new Repository.CategoryRepository(dbContext);
    var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

    var output = await categoryRepository.Search(searchInput, CancellationToken.None);

    output.Should().NotBeNull();
    output.Items.Should().NotBeNull();
    output.CurrentPage.Should().Be(searchInput.Page);
    output.PerPage.Should().Be(searchInput.PerPage);
    output.Total.Should().Be(quantityCategoriesToGenerate);
    output.Items.Should().HaveCount(expectedQuantityItems);
    foreach (Category outputItem in output.Items)
    {
      var exampleItem = exampleCategoriesList.Find(
        category => category.Id == outputItem.Id
      );
      exampleItem.Should().NotBeNull();
      outputItem.Name.Should().Be(exampleItem!.Name);
      outputItem.Description.Should().Be(exampleItem.Description);
      outputItem.IsActive.Should().Be(exampleItem.IsActive);
      outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
    }
  }

  [Theory(DisplayName = nameof(SearchByText))]
  [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
  [InlineData("Action", 1, 5, 1, 1)]
  [InlineData("Horror", 1, 5, 3, 3)]
  [InlineData("Horror", 2, 5, 0, 3)]
  [InlineData("Sci-fi", 1, 5, 4, 4)]
  [InlineData("Sci-fi", 1, 2, 2, 4)]
  [InlineData("Sci-fi", 2, 3, 1, 4)]
  [InlineData("Sci-fi Other", 1, 3, 0, 0)]
  [InlineData("Robots", 1, 5, 2, 2)]
  public async Task SearchByText(
    string search,
    int page,
    int perPage,
    int expectedQuantityItemsReturned,
    int expectedQuantityTotalItems
  )
  {
    CodeflixCatalogDbContext dbContext = CategoryRepositoryTestFixture.CreateDbContext();
    var exampleCategoriesList =
    _fixture.GetExampleCategoriesListWithNames(new List<string>() {
        "Action",
        "Horror",
        "Horror - Robots",
        "Horror - Based on Real Facts",
        "Drama",
        "Sci-fi IA",
        "Sci-fi Space",
        "Sci-fi Robots",
        "Sci-fi Future"
      }
    );
    await dbContext.AddRangeAsync(exampleCategoriesList);
    await dbContext.SaveChangesAsync(CancellationToken.None);
    var categoryRepository = new Repository.CategoryRepository(dbContext);
    var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

    var output = await categoryRepository.Search(searchInput, CancellationToken.None);

    output.Should().NotBeNull();
    output.Items.Should().NotBeNull();
    output.CurrentPage.Should().Be(searchInput.Page);
    output.PerPage.Should().Be(searchInput.PerPage);
    output.Total.Should().Be(expectedQuantityTotalItems);
    output.Items.Should().HaveCount(expectedQuantityItemsReturned);
    foreach (Category outputItem in output.Items)
    {
      var exampleItem = exampleCategoriesList.Find(
        category => category.Id == outputItem.Id
      );
      exampleItem.Should().NotBeNull();
      outputItem.Name.Should().Be(exampleItem!.Name);
      outputItem.Description.Should().Be(exampleItem.Description);
      outputItem.IsActive.Should().Be(exampleItem.IsActive);
      outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
    }
  }

  [Theory(DisplayName = nameof(SearchOrdered))]
  [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
  [InlineData("name", SearchOrder.Asc)]
  [InlineData("name", SearchOrder.Desc)]
  [InlineData("id", SearchOrder.Asc)]
  [InlineData("id", SearchOrder.Desc)]
  [InlineData("createdAt", SearchOrder.Asc)]
  [InlineData("createdAt", SearchOrder.Desc)]
  [InlineData("", SearchOrder.Asc)]
  public async Task SearchOrdered(
    string orderBy,
    SearchOrder searchOrder
  )
  {
    CodeflixCatalogDbContext dbContext = CategoryRepositoryTestFixture.CreateDbContext();
    var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
    await dbContext.AddRangeAsync(exampleCategoriesList);
    await dbContext.SaveChangesAsync(CancellationToken.None);
    var categoryRepository = new Repository.CategoryRepository(dbContext);
    var searchInput = new SearchInput(1, 20, "", orderBy, searchOrder);

    var output = await categoryRepository.Search(searchInput, CancellationToken.None);

    var expectedOrderedList = CategoryRepositoryTestFixture.CloneCategoriesListOrdered(
      exampleCategoriesList,
      orderBy,
      searchOrder
    );
    output.Should().NotBeNull();
    output.Items.Should().NotBeNull();
    output.CurrentPage.Should().Be(searchInput.Page);
    output.PerPage.Should().Be(searchInput.PerPage);
    output.Total.Should().Be(exampleCategoriesList.Count);
    output.Items.Should().HaveCount(exampleCategoriesList.Count);

    for (int i = 0; i < expectedOrderedList.Count; i++)
    {
      var expectedItem = expectedOrderedList[i];
      var outputItem = output.Items[i];

      expectedItem.Should().NotBeNull();
      outputItem.Should().NotBeNull();
      outputItem.Name.Should().Be(expectedItem!.Name);
      outputItem.Id.Should().Be(expectedItem.Id);
      outputItem.Description.Should().Be(expectedItem.Description);
      outputItem.IsActive.Should().Be(expectedItem.IsActive);
      outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);
    }
  }
}
