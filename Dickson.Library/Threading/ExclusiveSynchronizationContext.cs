using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dickson.Library.Threading
{
    class ExclusiveSynchronizationContext : SynchronizationContext
    {
        bool m_Done;
        readonly AutoResetEvent m_WorkItemsWaiting = new AutoResetEvent(false);
        readonly Queue<Tuple<SendOrPostCallback, object>> m_Items = new Queue<Tuple<SendOrPostCallback, object>>();

        public Exception InnerException { get; set; }

        public override void Send(SendOrPostCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public override void Post(SendOrPostCallback callback, object state)
        {
            lock (m_Items)
            {
                m_Items.Enqueue(Tuple.Create(callback, state));
            }
            m_WorkItemsWaiting.Set();
        }

        public void EndMessageLoop()
        {
            Post(_ => m_Done = true, null);
        }

        public void BeginMessageLoop()
        {
            while (!m_Done)
            {
                Tuple<SendOrPostCallback, object> task = null;
                lock (m_Items)
                {
                    if (m_Items.Count > 0)
                    {
                        task = m_Items.Dequeue();
                    }
                }
                if (task != null)
                {
                    task.Item1(task.Item2);
                    if (InnerException != null)
                    {
                        throw new AggregateException("AsyncHelper执行的异步方法引发了一个异常", InnerException);
                    }
                }
                else
                {
                    m_WorkItemsWaiting.WaitOne();
                }
            }
        }

        public override SynchronizationContext CreateCopy()
        {
            return this;
        }
    }
}
