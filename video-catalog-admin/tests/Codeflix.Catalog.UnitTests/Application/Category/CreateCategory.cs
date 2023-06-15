using Moq;
using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Domain.Repository;
using DomainEntity = Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FluentAssertions;

namespace Codeflix.Catalog.UnitTests.Application.Category;

public class CreateCategoryTest
{
  [Fact(DisplayName = nameof(CreateCategory))]
  [Trait("Application", "CreateCategory - Use Cases")]
  public async void CreateCategory()
  {
    var repositoryMock = new Mock<ICategoryRepository>();
    var unitOfWorkMock = new Mock<IUnitOfWork>();
    var useCase = new CreateCategory(
      repositoryMock.Object,
      unitOfWorkMock.Object
    );

    var input = new CreateCategoryInput(
      "Category Name",
      "Category Description",
      true
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

    output.Should().NotBeNull();
    output.Name.Should().Be("Category Name");
    output.Description.Should().Be("Category Description");
    output.IsActive.Should().Be(true);
    output.Id.Should().NotBeEmpty();
    output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
  }
}
