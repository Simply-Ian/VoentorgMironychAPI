using System.ComponentModel.DataAnnotations;
using Catalog.Application.Commands;
using Catalog.Application.Contracts;
using Catalog.Application.Queries.ItemQueries;
using Catalog.Application.Responses.GenericResponses;
using Catalog.Application.Responses.ItemResponses;
using Catalog.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
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
    
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)] // См. middleware GracefulExceptionsHandling
    [EndpointDescription(
        "Создает элемент каталога. Все указанные в запросе ID бренда и категорий должны принадлежать " +
        "существующим брендам/категориям, иначе вернет 400. Также проверяет Url картинки на валидность " +
        "и цену - на положительность")]
    public async Task<ActionResult<string>> CreateItem([FromBody, Required] CreateItemContract item)
    {
        // Исключения обрабатываются отдельным Middleware
        var result = await Mediator.Send(new CreateEntityCommand<CreateItemContract, Item>(item));
        Item createdItem = result.CreatedEntity;
        return CreatedAtAction(nameof(GetItemById), new {id = createdItem.Id}, item);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<ValidationFailure>),
        StatusCodes.Status400BadRequest)] // См. middleware GracefulExceptionsHandling
    [EndpointDescription(
        $"Обновляет элемент каталога. Помимо валидации, аналогичной {nameof(CreateItem)}, проверяет, " +
        $"существует ли элемент с указанным Id.")]
    public async Task<ActionResult> UpdateItem([FromRoute, Required] Guid id,
        [FromBody, Required] CreateItemContract item)
    {
        await Mediator.Send(new UpdateEntityCommand<CreateItemContract, Item>(id, item));
        return Ok();
    }
    
    // Статус-коды по REST.
    /// <returns>Результат содержит единственное булево поле: true, если удаление увенчалось успехом,
    /// false - если не удалось из-за внутренней ошибки</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteItem([FromRoute, Required] Guid id)
    {
        DeleteEntityByIdResult result = await Mediator.Send(new DeleteEntityCommand<Item>(id));
        if (result.IsSuccess) return NoContent();
        if (result.Reason is FluentValidation.ValidationException) return NotFound();
        return StatusCode(500, result.IsSuccess);
    }
    
}