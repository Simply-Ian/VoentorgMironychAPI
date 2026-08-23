using Catalog.Application.Responses.BrandResponses;
using MediatR;

namespace Catalog.Application.Queries;

public record GetAllBrandsQuery : IRequest<GetAllBrandsResult>;