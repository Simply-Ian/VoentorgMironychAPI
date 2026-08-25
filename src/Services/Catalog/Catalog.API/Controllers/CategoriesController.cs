using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

public class CategoriesController : BaseMediatedController
{
    [HttpGet]
    [ProducesResponseType(typeof(GetAllCategoriesResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAllCategoriesResult>> GetAllCategories()
    {
        return Ok(await Mediator.Send(new GetAllCategoriesQuery()));
    }
}