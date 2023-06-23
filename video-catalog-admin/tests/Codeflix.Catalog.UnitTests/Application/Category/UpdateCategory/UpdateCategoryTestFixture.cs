using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection
  : ICollectionFixture<UpdateCategoryTestFixture>
{ }

public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
{
  public string GetInvalidTooLongDescription()
  {
    var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();

    while (tooLongDescriptionForCategory.Length <= 10_000)
      tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";

    return tooLongDescriptionForCategory;
  }

  public UpdateCategoryInput GetValidInput(Guid? id = null)
    => new(
      id ?? Guid.NewGuid(),
      GetValidCategoryName(),
      GetValidCategoryDescription(),
      GetRandomBoolean()
    );
}
