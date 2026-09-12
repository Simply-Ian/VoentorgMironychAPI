using Catalog.Application.Responses.ItemResponses;
using Catalog.Domain.Specifications;
using MediatR;

namespace Catalog.Application.Queries.ItemQueries;

public record GetItemsWithPaginationQuery(QueryArgs Args) : IRequest<GetItemsWithPaginationResult>;