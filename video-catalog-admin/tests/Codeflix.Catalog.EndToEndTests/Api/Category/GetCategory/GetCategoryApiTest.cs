using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.EndToEndTests.Extensions.DateTime;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Codeflix.Catalog.EndToEndTests.Api.Category.GetCategoryById;

[Collection(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTest : IDisposable
{
  private readonly GetCategoryApiTestFixture _fixture;

  public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
    => _fixture = fixture;

  [Fact(DisplayName = nameof(GetCategory))]
  [Trait("EndToEnd/API", "Category/Get - Endpoints")]
  public async Task GetCategory()
  {
    var exampleCategoriesList = _fixture.GetExampleCategoriesList(5);
    await _fixture.Persistence.InsertList(exampleCategoriesList);
    var exampleCategory = exampleCategoriesList[2];

    var (response, output) = await _fixture.ApiClient.Get<CategoryModelOutput>(
      $"/categories/{exampleCategory.Id}"
    );

    response.Should().NotBeNull();
    response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    output.Should().NotBeNull();
    output!.Id.Should().Be(exampleCategory.Id);
    output.Name.Should().Be(exampleCategory.Name);
    output.Description.Should().Be(exampleCategory.Description);
    output.IsActive.Should().Be(exampleCategory.IsActive);
    output.CreatedAt.TrimMillisseconds().Should().Be(
      output.CreatedAt.TrimMillisseconds()
    );
  }

  [Fact(DisplayName = nameof(ErrorWhenNotFound))]
  [Trait("EndToEnd/API", "Category/Get - Endpoints")]
  public async Task ErrorWhenNotFound()
  {
    var randomGuid = Guid.NewGuid();

    var (response, output) = await _fixture.ApiClient.Get<ProblemDetails>(
      $"/categories/{randomGuid}"
    );

    response.Should().NotBeNull();
    response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
    output.Should().NotBeNull();
    output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
    output.Type.Should().Be("NotFound");
    output.Title.Should().Be("Not Found");
    output.Detail.Should().Be($"Category '{randomGuid}' not found");
  }

  public void Dispose() => _fixture.CleanPersistence();
}
