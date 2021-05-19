using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Scheduler.Interfaces;

namespace Scheduler.RxImplementation
{
    public sealed class CommonScheduler : ICommonScheduler
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<CommonScheduler> _logger;

        private const string _errorMessage = "Фоновая задача `DefaultRxScheduler` завершилась с ошибкой";

        public CommonScheduler(IServiceProvider provider, ILogger<CommonScheduler> logger)
        {
            _logger = logger;
            _provider = provider;
        }

        public IDisposable Enqueue<T>(Expression<Action<T>> methodCall)
        {
            ThrowIfArgumentIsWrong(methodCall);

            var method = methodCall.Compile();

            return TaskPoolScheduler.Default.Schedule(_provider.GetRequiredService<T>(), action);

            IDisposable action(IScheduler scheduler, T instance)
            {
                try
                {
                    method.Invoke(instance);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, _errorMessage);
                }

                return Disposable.Empty;
            };
        }

        public IDisposable Enqueue(Expression<Action> methodCall, TimeSpan delay)
        {
            ThrowIfArgumentsAreWrong(methodCall, delay);

            var method = methodCall.Compile();

            return TaskPoolScheduler.Default.Schedule(delay, () =>
            {
                try
                {
                    method.Invoke();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, _errorMessage);
                }
            });
        }

        public IDisposable Enqueue<T>(Expression<Action<T>> methodCall, TimeSpan delay)
        {
            ThrowIfArgumentsAreWrong(methodCall, delay);

            var method = methodCall.Compile();

            return TaskPoolScheduler.Default.Schedule(_provider.GetRequiredService<T>(), delay, action);

            IDisposable action(IScheduler scheduler, T instance)
            {
                try
                {
                    method.Invoke(instance);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, _errorMessage);
                }

                return Disposable.Empty;
            };
        }

        public IDisposable Enqueue<T>(Expression<Func<T, CancellationToken, Task>> methodCall)
        {
            ThrowIfArgumentIsWrong(methodCall);

            var method = methodCall.Compile();

            return TaskPoolScheduler.Default.ScheduleAsync(_provider.GetRequiredService<T>(), action);

            async Task<IDisposable> action(IScheduler scheduler, T instance, CancellationToken cancellationToken)
            {
                try
                {
                    await method.Invoke(instance, cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, _errorMessage);
                }

                return Disposable.Empty;
            };
        }

        public IDisposable Enqueue<T>(Expression<Func<T, CancellationToken, Task>> methodCall, TimeSpan delay)
        {
            ThrowIfArgumentsAreWrong(methodCall, delay);

            var method = methodCall.Compile();

            return TaskPoolScheduler.Default.ScheduleAsync(_provider.GetRequiredService<T>(), delay, action);

            async Task<IDisposable> action(IScheduler scheduler, T instance, CancellationToken cancellationToken)
            {
                try
                {
                    await method.Invoke(instance, cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, _errorMessage);
                }

                return Disposable.Empty;
            };
        }

        public IDisposable Enqueue<T, TResult>(Expression<Func<T, CancellationToken, Task<TResult>>> methodCall)
        {
            ThrowIfArgumentIsWrong(methodCall);

            var method = methodCall.Compile();

            return TaskPoolScheduler.Default.ScheduleAsync(_provider.GetRequiredService<T>(), action);

            async Task<IDisposable> action(IScheduler scheduler, T instance, CancellationToken cancellationToken)
            {
                try
                {
                    await method.Invoke(instance, cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, _errorMessage);
                }

                return Disposable.Empty;
            };
        }

        public IDisposable Enqueue<T, TResult>(Expression<Func<T, CancellationToken, Task<TResult>>> methodCall, TimeSpan delay)
        {
            ThrowIfArgumentsAreWrong(methodCall, delay);

            var method = methodCall.Compile();

            return TaskPoolScheduler.Default.ScheduleAsync(_provider.GetRequiredService<T>(), delay, action);

            async Task<IDisposable> action(IScheduler scheduler, T instance, CancellationToken cancellationToken)
            {
                try
                {
                    await method.Invoke(instance, cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, _errorMessage);
                }

                return Disposable.Empty;
            };
        }

        public IDisposable Schedule<T>(Expression<Action<T>> methodCall, TimeSpan period)
        {
            ThrowIfArgumentsAreWrong(methodCall, period);

            var method = methodCall.Compile();

            return TaskPoolScheduler.Default.SchedulePeriodic(_provider, period, provider =>
            {
                try
                {
                    method.Invoke(provider.GetRequiredService<T>());
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, _errorMessage);
                }
            });
        }

        public IDisposable Schedule<T>(Expression<Func<T, CancellationToken, Task>> methodCall, TimeSpan period)
        {
            ThrowIfArgumentsAreWrong(methodCall, period);

            var method = methodCall.Compile();
            var cancellationTokenSource = new CancellationTokenSource();

            var scheduled = TaskPoolScheduler.Default.SchedulePeriodic(_provider, period, async provider =>
            {
                try
                {
                    await method.Invoke(provider.GetRequiredService<T>(), cancellationTokenSource.Token);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, _errorMessage);
                }
            });

            return new CompositeDisposable { scheduled, Disposable.Create(cancellationTokenSource, cancellationToken =>
            {
                cancellationToken.Cancel();
                cancellationToken.Dispose();
            })};
        }

        public IDisposable Schedule<T, TResult>(Expression<Func<T, CancellationToken, Task<TResult>>> methodCall, TimeSpan period)
        {
            ThrowIfArgumentsAreWrong(methodCall, period);

            var method = methodCall.Compile();
            var cancellationTokenSource = new CancellationTokenSource();

            var scheduled = TaskPoolScheduler.Default.SchedulePeriodic(_provider, period, async provider =>
            {
                try
                {
                    await method.Invoke(provider.GetRequiredService<T>(), cancellationTokenSource.Token);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, _errorMessage);
                }
            });

            return new CompositeDisposable { scheduled, Disposable.Create(cancellationTokenSource, cancellationToken =>
            {
                cancellationToken.Cancel();
                cancellationToken.Dispose();
            })};
        }

        #region Private Methods

        private void ThrowIfArgumentIsWrong<TArgument>(TArgument argument)
        {
            if (EqualityComparer<TArgument>.Default.Equals(argument, default))
            {
                throw new ArgumentNullException("Выражение `Expression` не может быть пустым.");
            }
        }

        private void ThrowIfArgumentsAreWrong<TArgument>(TArgument argument, TimeSpan delay)
        {
            if (delay <= TimeSpan.Zero)
            {
                throw new ArgumentNullException(nameof(delay), $"Аргумент `{nameof(delay)}` не может быть равным {TimeSpan.Zero} или быть отрицательным.");
            }

            if (EqualityComparer<TArgument>.Default.Equals(argument, default))
            {
                throw new ArgumentNullException("Выражение `Expression` не может быть пустым.");
            }
        }

        #endregion
    }
}
