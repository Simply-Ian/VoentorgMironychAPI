using FluentValidation.Results;

namespace Common.Exceptions;

public class NotFoundException : Exception
{
    public IEnumerable<ValidationFailure> Errors { get; } = [];
    public NotFoundException(IEnumerable<ValidationFailure> errors)
    {
        Errors = errors;
    }

    public NotFoundException(string message) : base(message) {}
}