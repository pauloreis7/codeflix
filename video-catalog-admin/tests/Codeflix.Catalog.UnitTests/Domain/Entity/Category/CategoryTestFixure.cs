using DomainEntity = Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTestFixure
{
  public DomainEntity.Category GetValidCategory() => new("Category Name", "Category Description");
}

[CollectionDefinition(nameof(CategoryTestFixure))]
public class CategoryTestFixureCollection : ICollectionFixture<CategoryTestFixure>
{ }
