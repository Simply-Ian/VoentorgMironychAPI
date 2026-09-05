namespace Catalog.Application.Responses.GenericResponses;

/* Лично я бы предпочел сделать как в случае с обновлением: не возвращать ничего, а в случае неудачи
выбрасывать исключения. */
public record DeleteEntityByIdResult(bool IsSuccess, Exception? Reason = null);