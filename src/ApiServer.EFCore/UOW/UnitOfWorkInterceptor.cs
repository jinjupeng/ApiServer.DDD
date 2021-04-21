using Castle.Core.Internal;
using Castle.DynamicProxy;
using System.Reflection;
using System.Threading.Tasks;

namespace ApiServer.EFCore.UOW
{
    public class UnitOfWorkInterceptor : IInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Intercept(IInvocation invocation)
        {
            MethodInfo method;
            try
            {
                method = invocation.MethodInvocationTarget;
            }
            catch
            {
                method = invocation.GetConcreteMethod();
            }

            var unitOfWorkAttr = method.GetAttribute<UnitOfWorkAttribute>();
            if (unitOfWorkAttr == null || unitOfWorkAttr.IsDisabled)
            {
                //No need to a uow
                invocation.Proceed();
                return;
            }

            //No current uow, run a new one
            PerformUow(invocation);
        }

        private void PerformUow(IInvocation invocation)
        {
            if (invocation.Method.IsAsync())
            {
                PerformAsyncUow(invocation);
            }
            else
            {
                PerformSyncUow(invocation);
            }
        }

        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="invocation"></param>
        private void PerformSyncUow(IInvocation invocation)
        {
            using (var uow = _unitOfWork.Begin())
            {
                invocation.Proceed();
                _unitOfWork.Complete(uow);
            }
        }

        /// <summary>
        /// 异步
        /// </summary>
        /// <param name="invocation"></param>
        private void PerformAsyncUow(IInvocation invocation)
        {
            var uow = _unitOfWork.Begin();

            try
            {
                invocation.Proceed();
            }
            catch
            {
                uow.Dispose();
                throw;
            }

            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                    (Task)invocation.ReturnValue,
                    async () => await _admUnitOfWork.CompleteAsync(uow),
                    exception => uow.Dispose()
                );
            }
            else //Task<TResult>
            {
                invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                    invocation.Method.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                    async () => await _admUnitOfWork.CompleteAsync(uow),
                    exception => uow.Dispose()
                );
            }
        }
    }
}
