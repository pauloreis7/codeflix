using Codeflix.Catalog.Domain.Repository;
using Moq;
using DomainEntity = Codeflix.Catalog.Domain.Entity;
using UseCases = Codeflix.Catalog.Application.UseCases.CreateCategory;

namespace Codeflix.Catalog.UnitTests.Application.Category;

public class CreateCategoryTest
{
  [Fact(DisplayName = nameof(CreateCategory))]
  [Trait("Application", "CreateCategory - Use Cases")]
  public async void CreateCategory()
  {
    var repositoryMock = new Mock<ICategoryRepository>();
    var unitOfWorkMock = new Mock<IUnityOfWork>();
    var useCase = new UseCases.CreateCategory(
      repositoryMock.Object,
      unitOfWorkMock.Object
    );

    var input = new CreateCategoryInput(
      "Category name",
      "Category description",
      false
    );

    var output = await useCase.Handle(input, CancellationToken.None);

    repositoryMock.Verify(
      repository => repository.Insert(
        It.IsAny<DomainEntity.Category>(),
        It.IsAny<CancellationToken>()
      ),
      Times.Once
    );
    unitOfWorkMock.Verify(
      uow => uow.Commit(It.IsAny<CancellationToken>()),
      Times.Once
    );
    output.ShouldNotBeNull();
    output.Name.Should().Be("Category Name");
    output.Description.Should().Be("Category Description");
    output.IsActive.Should().Be(true);
    (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
    (output.CreatedAt != null && output.CreatedAt != default(DateTime)).Should().BeTrue();
  }
}