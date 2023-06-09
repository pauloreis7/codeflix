using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.EndToEndTests.Api.Category.Common;
using Xunit;

namespace Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTestFixtureCollection
  : ICollectionFixture<CreateCategoryApiTestFixture>
{ }

public class CreateCategoryApiTestFixture : CategoryBaseFixture
{
  public CreateCategoryInput GetExampleInput()
    => new(
      GetValidCategoryName(),
      GetValidCategoryDescription(),
      GetRandomBoolean()
    );
}
