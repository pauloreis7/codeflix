using Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using System.Net;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory;

[Collection(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTest
{
  private readonly CreateCategoryApiTestFixture _fixture;

  public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
    => _fixture = fixture;

  [Fact(DisplayName = nameof(CreateCategory))]
  [Trait("EndToEnd/API", "Category/Create - Endpoints")]
  public async Task CreateCategory()
  {
    var input = _fixture.GetExampleInput();

    var (response, output) = await _fixture.ApiClient
      .Post<CategoryModelOutput>(
        "/categories",
        input
      );

    response.Should().NotBeNull();
    response!.StatusCode.Should().Be(HttpStatusCode.Created);
    output.Should().NotBeNull();
    output!.Name.Should().Be(input.Name);
    output.Description.Should().Be(input.Description);
    output.IsActive.Should().Be(input.IsActive);
    output.Id.Should().NotBeEmpty();
    output.CreatedAt.Should().NotBeSameDateAs(default);

    var dbCategory = await _fixture.Persistence.GetById(output.Id);

    dbCategory.Should().NotBeNull();
    dbCategory!.Name.Should().Be(input.Name);
    dbCategory.Description.Should().Be(input.Description);
    dbCategory.IsActive.Should().Be(input.IsActive);
    dbCategory.Id.Should().NotBeEmpty();
    dbCategory.CreatedAt.Should().NotBeSameDateAs(default);
  }

  [Fact(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
  [Trait("EndToEnd/API", "Category/Create - Endpoints")]
  public async Task ThrowWhenCantInstantiateAggregate()
  {
    var input = _fixture.GetExampleInput();
    input.Name = "ab";

    var (response, output) = await _fixture.
      ApiClient.Post<ProblemDetails>(
        "/categories",
        input
      );

    response.Should().NotBeNull();
    response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    output.Should().NotBeNull();
    output!.Title.Should().Be("One or more validation errors ocurred");
    output.Type.Should().Be("UnprocessableEntity");
    output.Status.Should().Be((int)StatusCodes.Status422UnprocessableEntity);
    output.Detail.Should().Be("Name should be at least 3 characters long");
  }
}
