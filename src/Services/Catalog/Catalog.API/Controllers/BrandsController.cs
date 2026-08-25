using Microsoft.AspNetCore.Mvc;
using Catalog.Application.Queries;
using Catalog.Application.Responses.BrandResponses;

namespace Catalog.API.Controllers;

public class BrandsController : BaseMediatedController
{
    [HttpGet]
    [ProducesResponseType(typeof(GetAllBrandsResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAllBrandsResult>> GetAllBrands()
    {
        var brands = await Mediator.Send(new GetAllBrandsQuery());
        return Ok(brands);
    }
}