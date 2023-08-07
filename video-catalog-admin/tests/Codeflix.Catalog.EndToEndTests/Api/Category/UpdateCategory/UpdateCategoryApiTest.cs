using Codeflix.Catalog.Api.ApiModels.Category;
using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Api.ApiModels.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTest : IDisposable
{
  private readonly UpdateCategoryApiTestFixture _fixture;

  public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
    => _fixture = fixture;

  [Fact(DisplayName = nameof(UpdateCategory))]
  [Trait("EndToEnd/API", "Category/Update - Endpoints")]
  public async void UpdateCategory()
  {
    var exampleCategoriesList = _fixture.GetExampleCategoriesList(5);
    await _fixture.Persistence.InsertList(exampleCategoriesList);
    var exampleCategory = exampleCategoriesList[2];
    var input = _fixture.GetExampleInput();

    var (response, output) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelOutput>>(
      $"/categories/{exampleCategory.Id}",
      input
    );

    response.Should().NotBeNull();
    response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    output!.Data.Should().NotBeNull();
    output.Data.Id.Should().Be(exampleCategory.Id);
    output.Data.Name.Should().Be(input.Name);
    output.Data.Description.Should().Be(input.Description);
    output.Data.IsActive.Should().Be((bool)input.IsActive!);

    var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

    dbCategory.Should().NotBeNull();
    dbCategory!.Name.Should().Be(input.Name);
    dbCategory.Description.Should().Be(input.Description);
    dbCategory.IsActive.Should().Be((bool)input.IsActive!);
  }

  [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
  [Trait("EndToEnd/API", "Category/Update - Endpoints")]
  public async void UpdateCategoryOnlyName()
  {
    var exampleCategoriesList = _fixture.GetExampleCategoriesList(5);
    await _fixture.Persistence.InsertList(exampleCategoriesList);
    var exampleCategory = exampleCategoriesList[2];
    var input = new UpdateCategoryApiInput(
      _fixture.GetValidCategoryName()
    );

    var (response, output) = await _fixture.ApiClient
      .Put<ApiResponse<CategoryModelOutput>>(
        $"/categories/{exampleCategory.Id}",
        input
      );

    response.Should().NotBeNull();
    response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
    output!.Data.Should().NotBeNull();
    output.Data.Id.Should().Be(exampleCategory.Id);
    output.Data.Name.Should().Be(input.Name);
    output.Data.Description.Should().Be(exampleCategory.Description);
    output.Data.IsActive.Should().Be(exampleCategory.IsActive);

    var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

    dbCategory.Should().NotBeNull();
    dbCategory!.Name.Should().Be(input.Name);
    dbCategory.Description.Should().Be(exampleCategory.Description);
    dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
  }

  [Fact(DisplayName = nameof(ErrorWhenNotFound))]
  [Trait("EndToEnd/API", "Category/Update - Endpoints")]
  public async void ErrorWhenNotFound()
  {
    var randomGuid = Guid.NewGuid();
    var input = _fixture.GetExampleInput();

    var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
      $"/categories/{randomGuid}",
      input
    );

    response.Should().NotBeNull();
    response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
    output.Should().NotBeNull();
    output!.Title.Should().Be("Not Found");
    output.Type.Should().Be("NotFound");
    output.Status.Should().Be((int)StatusCodes.Status404NotFound);
    output.Detail.Should().Be($"Category '{randomGuid}' not found");
  }

  [Fact(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
  [Trait("EndToEnd/API", "Category/Update - Endpoints")]
  public async void ErrorWhenCantInstantiateAggregate()
  {
    var exampleCategoriesList = _fixture.GetExampleCategoriesList(5);
    await _fixture.Persistence.InsertList(exampleCategoriesList);
    var exampleCategory = exampleCategoriesList[2];
    var input = _fixture.GetExampleInput();
    input.Name = "ab";

    var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
      $"/categories/{exampleCategory.Id}",
      input
    );

    response.Should().NotBeNull();
    response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status422UnprocessableEntity);
    output.Should().NotBeNull();
    output!.Title.Should().Be("One or more validation errors ocurred");
    output.Type.Should().Be("UnprocessableEntity");
    output.Status.Should().Be((int)StatusCodes.Status422UnprocessableEntity);
    output.Detail.Should().Be("Name should be at least 3 characters long");
  }

  public void Dispose() => _fixture.CleanPersistence();
}
