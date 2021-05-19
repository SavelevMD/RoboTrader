using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

namespace CommonValidator
{
    public interface ICommonValidator
    {
        TValue ValidateWithContextAndThrow<TValue>(ValidationContext<TValue> context);
        TValue ValidateAndThrow<TValue>(TValue value);

        Task<TValue> ValidateAndThrowAsync<TValue>(TValue value, CancellationToken cancellationToken = default);
    }
}