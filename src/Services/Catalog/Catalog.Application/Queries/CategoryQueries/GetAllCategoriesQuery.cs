using Catalog.Application.Responses.CategoryResponses;
using MediatR;

namespace Catalog.Application.Queries;

public record GetAllCategoriesQuery : IRequest<GetAllCategoriesResult>;