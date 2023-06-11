namespace Codeflix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
  [Fact(DisplayName = nameof(Instantiate))]
  [Trait("Domain", "Category - Aggregates")]
  public void Instantiate()
  {
    var validData = new
    {
      Name = "Category name",
      Description = "Category description"
    };

    var category = new Category(validData.Name, validData.Description);

    Assert.NotNull(category);
    Assert.Equal(validData.Name, category.Name);
    Assert.Equal(validData.Description, category.Description);
  }
}
