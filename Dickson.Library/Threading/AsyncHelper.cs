using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dickson.Library.Threading
{
    public static class AsyncHelper
    {/// <summary>
     /// 同步执行异步方法，仅在需要保持上下文的场景中使用此方法，保持上下文会话费额外的开销。
     /// </summary>
     /// <param name="task">异步操作。</param>
        public static void RunSync(this Func<Task> task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            var origin = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(async _ =>
            {
                try
                {
                    await task();
                }
                catch (Exception ex)
                {
                    synch.InnerException = ex;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(origin);
        }

        /// <summary>
        /// 同步执行异步方法，仅在需要保持上下文的场景中使用此方法，保持上下文会话费额外的开销。
        /// </summary>
        /// <param name="task">异步操作。</param>
        public static TResult RunSync<TResult>(this Func<Task<TResult>> task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            var origin = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            TResult result = default(TResult);
            synch.Post(async _ =>
            {
                try
                {
                    result = await task();
                }
                catch (Exception ex)
                {
                    synch.InnerException = ex;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(origin);
            return result;
        }
    }
}
