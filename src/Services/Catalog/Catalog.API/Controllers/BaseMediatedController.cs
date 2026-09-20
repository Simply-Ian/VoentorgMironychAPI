using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseMediatedController(IMediator mediator) : ControllerBase
{
    protected IMediator Mediator => mediator;
}