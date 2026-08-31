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
    [ProducesResponseType(typeof(SearchItemsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointDescription("Поиск разновидности товара по ее Guid")]
    public async Task<ActionResult<SearchItemsResult>> GetItemById([FromRoute, Required] Guid id)
    {
        var result = await Mediator.Send(new GetItemByIdQuery(id));
        return result.Items.Any() ? Ok(result) : NotFound();
    }

    [HttpGet("search-by-title")]
    [ProducesResponseType(typeof(SearchItemsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointDescription("Поиск разновидности товара по ее названию. В текущей реализации проверяет, содержится ли " +
                         "переданная подстрока в названии товара, регистр символов не учитывается.\n" +
                         "Если строка поиска короче 3 символов, возвращает 400 с описанием ошибки")]
    public async Task<ActionResult<SearchItemsResult>> GetItemsByTitle([FromQuery, Required, MinLength(3)] string title)
    {
        var result = await Mediator.Send(new GetItemsByTitleQuery(title));
        return result.Items.Any() ? Ok(result) : NotFound() ;
    }

    [HttpGet("search-by-brand")]
    [ProducesResponseType(typeof(SearchItemsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointDescription("Поиск товаров по названию бренда")]
    public async Task<ActionResult<SearchItemsResult>> GetItemsByBrandTitle(
        [FromQuery, Required, MinLength(3)] string title)
    {
        var result = await Mediator.Send(new GetItemsByBrandTitleQuery(title));
        return result.Items.Any() ? Ok(result) : NotFound() ;
    }
}