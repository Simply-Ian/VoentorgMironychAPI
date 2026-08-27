using System.ComponentModel.DataAnnotations;
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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetItemByIdResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointDescription("Поиск разновидности товара по ее Guid")]
    public async Task<ActionResult<GetItemByIdResult>> GetItemById([FromRoute, Required] Guid id)
    {
        var result = await Mediator.Send(new GetItemByIdQuery(id));
        return result.Item is null ? NotFound() : Ok(result);
    }
}