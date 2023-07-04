using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codeflix.Catalog.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
  private readonly IMediator _mediator;

  public CategoriesController(IMediator mediator) => _mediator = mediator;

  [HttpPost]
  [ProducesResponseType(typeof(CategoryModelOutput), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
  public async Task<IActionResult> Create(
    [FromBody] CreateCategoryInput input,
    CancellationToken cancellationToken
  )
  {
    var output = await _mediator.Send(input, cancellationToken);

    return CreatedAtAction(
      nameof(Create),
      new { output.Id },
      output
    );
  }

  [HttpPut("{id:guid}")]
  [ProducesResponseType(typeof(CategoryModelOutput), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
  public async Task<IActionResult> Update(
    [FromBody] UpdateCategoryInput input,
    CancellationToken cancellationToken
  )
  {
    var output = await _mediator.Send(input, cancellationToken);

    return Ok(output);
  }

  [HttpGet]
  [ProducesResponseType(typeof(CategoryModelOutput), StatusCodes.Status200OK)]
  public async Task<IActionResult> List(
    CancellationToken cancellationToken,
    [FromQuery] int? page = null,
    [FromQuery] int? perPage = null,
    [FromQuery] string? search = null,
    [FromQuery] string? sort = null,
    [FromQuery] SearchOrder? dir = null
  )
  {
    var input = new ListCategoriesInput(
      page: page ?? 1,
      perPage: perPage ?? 15,
      search: search ?? "",
      sort: sort ?? "",
      dir: dir ?? SearchOrder.Asc
    );

    var output = await _mediator.Send(input, cancellationToken);

    return Ok(output);
  }

  [HttpGet("{id:guid}")]
  [ProducesResponseType(typeof(CategoryModelOutput), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetById(
    [FromRoute] Guid id,
    CancellationToken cancellationToken
  )
  {
    var output = await _mediator.Send(new GetCategoryInput(id), cancellationToken);

    return Ok(output);
  }

  [HttpDelete("{id:guid}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Delete(
    [FromRoute] Guid id,
    CancellationToken cancellationToken
  )
  {
    await _mediator.Send(new DeleteCategoryInput(id), cancellationToken);

    return NoContent();
  }
}
