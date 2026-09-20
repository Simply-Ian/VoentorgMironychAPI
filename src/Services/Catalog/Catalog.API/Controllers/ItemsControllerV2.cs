using Asp.Versioning;
using Catalog.Application.Queries.ItemQueries;
using Catalog.Application.Responses.ItemResponses;
using Catalog.Domain.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Catalog.API.Controllers;

[ApiVersion(2)]
[ApiExplorerSettings(GroupName = "v2")]
[Route("api/v{version:apiVersion}/items")]
[SwaggerTag("Контроллер v2 поддерживает пагинацию, в отличие от v1")]
public class ItemsControllerV2(IMediator mediator) : BaseMediatedController(mediator)
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, 
        "Страница каталога заданного размера", 
        typeof(GetItemsWithPaginationResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Возвращается, если номер страницы меньше 0 или больше количества страниц, либо размер страницы меньше 1")]
    public async Task<ActionResult<GetItemsWithPaginationResult>> GetItemsWithPagination
    (
        [FromQuery]
        QueryArgs queryArgs
    ) => Ok(await Mediator.Send(new GetItemsWithPaginationQuery(queryArgs)));
}