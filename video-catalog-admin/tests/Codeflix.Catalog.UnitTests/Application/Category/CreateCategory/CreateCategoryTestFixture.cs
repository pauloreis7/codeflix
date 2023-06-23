using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace Codeflix.Catalog.UnitTests.Application.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection
  : ICollectionFixture<CreateCategoryTestFixture>
{ }

public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
{
  public CreateCategoryInput GetInput() =>
    new(
      GetValidCategoryName(),
      GetValidCategoryDescription(),
      GetRandomBoolean()
    );
}
