using System;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace CommonValidator
{
    public sealed class CommonValidator : ICommonValidator
    {
        private readonly IServiceProvider _provider;

        public CommonValidator(IServiceProvider provider) => _provider = provider;

        public TValue ValidateWithContextAndThrow<TValue>(ValidationContext<TValue> context)
        {
            var validationResult = _provider.GetRequiredService<IValidator<TValue>>()
                .Validate(context);

            return !validationResult.IsValid ? throw new ValidationException(validationResult.Errors) : context.InstanceToValidate;
        }

        public TValue ValidateAndThrow<TValue>(TValue value)
        {
            _provider.GetRequiredService<IValidator<TValue>>()
                .ValidateAndThrow(value);

            return value;
        }

        public async Task<TValue> ValidateAndThrowAsync<TValue>(TValue value, CancellationToken cancellationToken = default)
        {
            await _provider.GetRequiredService<IValidator<TValue>>()
                .ValidateAndThrowAsync(value, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return value;
        }
    }
}
