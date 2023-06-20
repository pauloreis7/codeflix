using MediatR;
using Codeflix.Catalog.Application.UseCases.Category.Common;

namespace Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
public interface IUpdateCategory
  : IRequestHandler<UpdateCategoryInput, CategoryModelOutput>
{ }
