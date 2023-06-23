using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FluentAssertions;
using Moq;
using UseCase = Codeflix.Catalog.Application.UseCases.Category.ListCategories;

namespace Codeflix.Catalog.UnitTests.Application.ListCategories;

[Collection(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTest
{
  private readonly ListCategoriesTestFixture _fixture;

  public ListCategoriesTest(ListCategoriesTestFixture fixture)
    => _fixture = fixture;

  [Fact(DisplayName = nameof(List))]
  [Trait("Application", "ListCategories - Use Cases")]
  public async Task List()
  {
    var categoriesExampleList = _fixture.GetExampleCategoriesList();
    var repositoryMock = _fixture.GetRepositoryMock();
    var input = _fixture.GetExampleInput();
    var outputRepositorySearch = new SearchOutput<Category>(
      currentPage: input.Page,
      perPage: input.PerPage,
      items: (IReadOnlyList<Category>)categoriesExampleList,
      total: (new Random()).Next(50, 200)
    );
    repositoryMock.Setup(x => x.Search(
      It.Is<SearchInput>(
        searchInput => searchInput.Page == input.Page
        && searchInput.PerPage == input.PerPage
        && searchInput.Search == input.Search
        && searchInput.OrderBy == input.Sort
        && searchInput.Order == input.Dir
      ), It.IsAny<CancellationToken>()
    )).ReturnsAsync(outputRepositorySearch);
    var useCase = new UseCase.ListCategories(repositoryMock.Object);

    var output = await useCase.Handle(input, CancellationToken.None);

    output.Should().NotBeNull();
    output.Page.Should().Be(outputRepositorySearch.CurrentPage);
    output.PerPage.Should().Be(outputRepositorySearch.PerPage);
    output.Total.Should().Be(outputRepositorySearch.Total);
    output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
    ((List<CategoryModelOutput>)output.Items).ForEach(outputItem =>
    {
      var repositoryCategory = outputRepositorySearch.Items
        .FirstOrDefault(x => x.Id == outputItem.Id);
      outputItem.Should().NotBeNull();
      outputItem.Name.Should().Be(repositoryCategory!.Name);
      outputItem.Description.Should().Be(repositoryCategory.Description);
      outputItem.IsActive.Should().Be(repositoryCategory.IsActive);
      outputItem.CreatedAt.Should().Be(repositoryCategory.CreatedAt);
    });
    repositoryMock.Verify(x => x.Search(
      It.Is<SearchInput>(
       searchInput => searchInput.Page == input.Page
        && searchInput.PerPage == input.PerPage
        && searchInput.Search == input.Search
        && searchInput.OrderBy == input.Sort
        && searchInput.Order == input.Dir
      ), It.IsAny<CancellationToken>()
    ), Times.Once);
  }

  [Theory(DisplayName = nameof(ListInputWithoutAllParameters))]
  [Trait("Application", "ListCategories - Use Cases")]
  [MemberData(nameof(GetInputsWithoutAllParameter))]
  public async Task ListInputWithoutAllParameters(
    UseCase.ListCategoriesInput input
  )
  {
    var categoriesExampleList = _fixture.GetExampleCategoriesList();
    var repositoryMock = _fixture.GetRepositoryMock();
    var outputRepositorySearch = new SearchOutput<Category>(
       currentPage: input.Page,
       perPage: input.PerPage,
       items: (IReadOnlyList<Category>)categoriesExampleList,
       total: (new Random()).Next(50, 200)
     );
    repositoryMock.Setup(x => x.Search(
      It.Is<SearchInput>(
        searchInput => searchInput.Page == input.Page
        && searchInput.PerPage == input.PerPage
        && searchInput.Search == input.Search
        && searchInput.OrderBy == input.Sort
        && searchInput.Order == input.Dir
      ), It.IsAny<CancellationToken>()
    )).ReturnsAsync(outputRepositorySearch);
    var useCase = new UseCase.ListCategories(repositoryMock.Object);

    var output = await useCase.Handle(input, CancellationToken.None);

    output.Should().NotBeNull();
    output.Page.Should().Be(outputRepositorySearch.CurrentPage);
    output.PerPage.Should().Be(outputRepositorySearch.PerPage);
    output.Total.Should().Be(outputRepositorySearch.Total);
    output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
    ((List<CategoryModelOutput>)output.Items).ForEach(outputItem =>
    {
      var repositoryCategory = outputRepositorySearch.Items
        .FirstOrDefault(x => x.Id == outputItem.Id);
      outputItem.Should().NotBeNull();
      outputItem.Name.Should().Be(repositoryCategory!.Name);
      outputItem.Description.Should().Be(repositoryCategory.Description);
      outputItem.IsActive.Should().Be(repositoryCategory.IsActive);
      outputItem.CreatedAt.Should().Be(repositoryCategory.CreatedAt);
    });
    repositoryMock.Verify(x => x.Search(
      It.Is<SearchInput>(
       searchInput => searchInput.Page == input.Page
        && searchInput.PerPage == input.PerPage
        && searchInput.Search == input.Search
        && searchInput.OrderBy == input.Sort
        && searchInput.Order == input.Dir
      ), It.IsAny<CancellationToken>()
    ), Times.Once);
  }

  private static IEnumerable<object[]> GetInputsWithoutAllParameter()
  {
    var fixture = new ListCategoriesTestFixture();
    var inputExample = fixture.GetExampleInput();

    yield return new object[] {
      new ListCategoriesInput()
    };

    yield return new object[] {
      new ListCategoriesInput(inputExample.Page)
    };

    yield return new object[] {
      new ListCategoriesInput(
        inputExample.Page,
        inputExample.PerPage
      )
    };

    yield return new object[] {
      new ListCategoriesInput(
        inputExample.Page,
        inputExample.PerPage,
        inputExample.Search
      )
    };

    yield return new object[] {
      new ListCategoriesInput(
        inputExample.Page,
        inputExample.PerPage,
        inputExample.Search,
        inputExample.Sort
      )
    };
  }
}
