using Microsoft.AspNetCore.Mvc;
using Catalog.Application.Queries;

namespace Catalog.API.Controllers;

public class BrandsController : BaseMediatedController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBrands()
    {
        var brands = await Mediator.Send(new GetAllBrandsQuery());
        return Ok(brands);
    }
}