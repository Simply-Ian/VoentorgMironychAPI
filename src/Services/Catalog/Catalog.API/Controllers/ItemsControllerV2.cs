using Asp.Versioning;
using Catalog.Application.Queries.ItemQueries;
using Catalog.Application.Responses.ItemResponses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Catalog.API.Controllers;

[ApiVersion(2)]
[ApiExplorerSettings(GroupName = "v2")]
[Route("api/v{version:apiVersion}/items")]
[SwaggerTag("Контроллер v2 поддерживает пагинацию, в отличие от v1")]
// Почему в Swagger отображается как ItemsControllerV, без цифры "2"?
public class ItemsControllerV2 : BaseMediatedController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, 
        "Страница каталога заданного размера", 
        typeof(GetItemsWithPaginationResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Возвращается, если номер страницы меньше 0 или больше количества страниц, либо размер страницы меньше 1")]
    public async Task<ActionResult<GetItemsWithPaginationResult>> GetItemsWithPagination
    (
        [FromQuery, SwaggerParameter("0-based индекс страницы", Required = false)]
        int pageIndex,
        [FromQuery, SwaggerParameter("Размер страницы, элем.", Required = true)]
        int pageSize
    ) => Ok(await Mediator.Send(new GetItemsWithPaginationQuery(pageIndex, pageSize)));
}