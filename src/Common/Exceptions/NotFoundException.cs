using FluentValidation.Results;

namespace Common.Exceptions;

public class NotFoundException(IEnumerable<ValidationFailure> errors) : Exception
{
    public IEnumerable<ValidationFailure> Errors { get; } = errors;
}