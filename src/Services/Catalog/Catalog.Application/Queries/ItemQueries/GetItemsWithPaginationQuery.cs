using Catalog.Application.Responses.ItemResponses;
using MediatR;

namespace Catalog.Application.Queries.ItemQueries;

public record GetItemsWithPaginationQuery(int PageIndex, int PageSize) : IRequest<GetItemsWithPaginationResult>;