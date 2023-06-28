using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection
  : ICollectionFixture<UpdateCategoryTestFixture>
{ }

public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
{
  public UpdateCategoryInput GetValidInput(Guid? id = null)
   => new(
     id ?? Guid.NewGuid(),
     GetValidCategoryName(),
     GetValidCategoryDescription(),
     GetRandomBoolean()
   );

  public UpdateCategoryInput GetInvalidInputShortName()
  {
    var invalidInputShortName = GetValidInput();
    invalidInputShortName.Name = invalidInputShortName.Name[..2];
    return invalidInputShortName;
  }
}
