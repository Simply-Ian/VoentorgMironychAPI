using Catalog.Application.Queries;
using Catalog.Application.Responses.CategoryResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

public class CategoriesController(IMediator mediator) : BaseMediatedController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(GetAllCategoriesResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAllCategoriesResult>> GetAllCategories() 
        =>Ok(await Mediator.Send(new GetAllCategoriesQuery()));
}