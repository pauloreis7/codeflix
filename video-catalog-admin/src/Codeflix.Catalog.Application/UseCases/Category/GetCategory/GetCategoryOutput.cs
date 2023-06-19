using DomainEntity = Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryOutput
{
  public GetCategoryOutput(
    Guid id,
    string name,
    string description,
    bool isActive,
    DateTime createdAt
  )
  {
    Id = id;
    Name = name;
    Description = description;
    IsActive = isActive;
    CreatedAt = createdAt;
  }
  public Guid Id { get; set; }

  public string Name { get; set; }

  public string Description { get; set; }

  public bool IsActive { get; set; }

  public DateTime CreatedAt { get; set; }

  public static GetCategoryOutput FromCategory(DomainEntity.Category category)
  {
    return new GetCategoryOutput(
      category.Id,
      category.Name,
      category.Description,
      category.IsActive,
      category.CreatedAt
    );
  }
}