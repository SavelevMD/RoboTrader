using System;

namespace Models.Results
{
    public class OperationResult
    {
        public Exception Exception { get; set; }

        public string ErrorMessage { get; private set; }
        public string SuccessMessage { get; private set; }

        public bool IsSuccess { get; private set; }

        private OperationResult() { }

        public static OperationResult Success()
        {
            return new OperationResult { IsSuccess = true };
        }
        public static OperationResult Success(string message)
        {
            return new OperationResult
            {
                IsSuccess = true,
                SuccessMessage = message
            };
        }

        public static OperationResult Failure(string message)
        {
            return new OperationResult { ErrorMessage = message };
        }

        public static OperationResult Failure(string message, Exception exception)
        {
            return new OperationResult
            {
                ErrorMessage = message,
                Exception = exception
            };
        }
    }

    public class OperationResult<T>
    {
        public Exception Exception { get; set; }

        public string ErrorMessage { get; private set; }
        public string SuccessMessage { get; private set; }
        public T Result { get; private set; }

        public bool IsSuccess { get; private set; }

        private OperationResult(T result) { Result = result; }

        public static OperationResult<T> Success(T result)
        {
            return new OperationResult<T>(result)
            {
                IsSuccess = true,
                Result = result
            };
        }

        public static OperationResult<T> Success(T result, string message = default)
        {
            return new OperationResult<T>(result)
            {
                IsSuccess = true,
                SuccessMessage = message,
                Result = result
            };
        }

        public static OperationResult<T> Failure(string message)
        {
            return new OperationResult<T>(default)
            {
                ErrorMessage = message
            };
        }

        public static OperationResult<T> Failure(string message, Exception exception)
        {
            return new OperationResult<T>(default)
            {
                ErrorMessage = message,
                Exception = exception
            };
        }
    }
}
