using Catalog.Application.Responses.ItemResponses;
using MediatR;

namespace Catalog.Application.Queries.ItemQueries;

public record GetItemByIdQuery (Guid Id) : IRequest<SearchItemsResult>;