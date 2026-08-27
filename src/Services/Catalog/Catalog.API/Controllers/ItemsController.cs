using Catalog.Application.Queries.ItemQueries;
using Catalog.Application.Responses.ItemResponses;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

public class ItemsController : BaseMediatedController
{
    [HttpGet]
    [ProducesResponseType(typeof(GetAllItemsResult), StatusCodes.Status200OK)]
    [EndpointDescription("Возвращает список всех разновидностей товаров")]
    public async Task<ActionResult<GetAllItemsResult>> GetAllItems()
        => Ok(await Mediator.Send(new GetAllItemsQuery()));
}