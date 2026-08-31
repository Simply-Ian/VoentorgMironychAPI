using Catalog.Application.Responses.ItemResponses;
using MediatR;

namespace Catalog.Application.Queries.ItemQueries;

public record GetItemsByBrandTitleQuery (string BrandTitle): IRequest<SearchItemsResult>;