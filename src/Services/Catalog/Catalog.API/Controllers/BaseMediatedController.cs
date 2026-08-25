using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseMediatedController : ControllerBase
{
    protected IMediator Mediator => HttpContext.RequestServices.GetService<IMediator>() 
                                    ?? throw new Exception("MediatR is not registered");
}