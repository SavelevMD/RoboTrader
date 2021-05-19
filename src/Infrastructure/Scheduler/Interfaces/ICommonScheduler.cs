using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.Interfaces
{
    public interface ICommonScheduler
    {
        IDisposable Enqueue(Expression<Action> methodCall, TimeSpan delay);
        IDisposable Enqueue<T, TResult>(Expression<Func<T, CancellationToken, Task<TResult>>> methodCall);
        IDisposable Enqueue<T, TResult>(Expression<Func<T, CancellationToken, Task<TResult>>> methodCall, TimeSpan delay);
        IDisposable Enqueue<T>(Expression<Action<T>> methodCall);
        IDisposable Enqueue<T>(Expression<Action<T>> methodCall, TimeSpan delay);
        IDisposable Enqueue<T>(Expression<Func<T, CancellationToken, Task>> methodCall);
        IDisposable Enqueue<T>(Expression<Func<T, CancellationToken, Task>> methodCall, TimeSpan delay);
        IDisposable Schedule<T, TResult>(Expression<Func<T, CancellationToken, Task<TResult>>> methodCall, TimeSpan period);
        IDisposable Schedule<T>(Expression<Action<T>> methodCall, TimeSpan period);
        IDisposable Schedule<T>(Expression<Func<T, CancellationToken, Task>> methodCall, TimeSpan period);
    }
}
