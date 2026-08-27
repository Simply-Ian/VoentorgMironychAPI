using Catalog.Application.Responses.ItemResponses;
using MediatR;

namespace Catalog.Application.Queries.ItemQueries;

public record GetItemsByTitleQuery(string ProvidedTitle) : IRequest<SearchItemsResult>;