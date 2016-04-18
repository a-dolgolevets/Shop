using System;
using Shop.Domain.Base;

namespace Shop.ServicesFacade.Helpers
{
    public class OperationResult
    {
        public OperationResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public OperationResult(Exception exception)
            : this(false)
        {
            Exception = exception;
        }

        public bool IsSuccess { get; private set; }

        public Exception Exception { get; set; }
    }

    public class OperationResult<T> : OperationResult where T : IBaseEntity
    {
        public OperationResult(bool isSuccess, T entity)
            : base(isSuccess)
        {
            Entity = entity;
        }

        public OperationResult(Exception exception, T entity)
            : base(exception)
        {
            Entity = entity;
        }

        public T Entity { get; private set; }
    }
}