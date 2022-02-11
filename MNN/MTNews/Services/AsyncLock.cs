using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTNews.Services
{
    class AsyncLock
    {
        #region Private Properties

        /// <summary>
        /// The m semaphore.
        /// </summary>
        private readonly AsyncSemaphore m_semaphore;

        /// <summary>
        /// The m releaser.
        /// </summary>
        private readonly Task<Releaser> m_releaser;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TelstraHealth.Portable.Threading.AsyncLock"/> class.
        /// </summary>
        public AsyncLock()
        {
            m_semaphore = new AsyncSemaphore(1);
            m_releaser = Task.FromResult(new Releaser(this));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Locks the async.
        /// </summary>
        /// <returns>The async.</returns>
        public Task<Releaser> LockAsync()
        {
            var wait = m_semaphore.WaitAsync();
            return wait.IsCompleted ?
                m_releaser :
                wait.ContinueWith((_, state) => new Releaser((AsyncLock)state),
                    this, CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        /// <summary>
        /// Releaser.
        /// </summary>
        public struct Releaser : IDisposable
        {
            private readonly AsyncLock m_toRelease;

            internal Releaser(AsyncLock toRelease) { m_toRelease = toRelease; }

            public void Dispose()
            {
                if (m_toRelease != null)
                    m_toRelease.m_semaphore.Release();
            }
        }

        #endregion
    }

    class AsyncSemaphore
    {
        #region Private Properties

        /// <summary>
        /// The s completed.
        /// </summary>
        private readonly static Task s_completed = Task.FromResult(true);

        /// <summary>
        /// The m waiters.
        /// </summary>
        private readonly Queue<TaskCompletionSource<bool>> m_waiters = new Queue<TaskCompletionSource<bool>>();

        /// <summary>
        /// The m current count.
        /// </summary>
        private int m_currentCount;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TelstraHealth.Portable.Threading.AsyncSemaphore"/> class.
        /// </summary>
        /// <param name="initialCount">Initial count.</param>
        public AsyncSemaphore(int initialCount)
        {
            if (initialCount < 0) return;//throw new ArgumentOutOfRangeException("initialCount");
            m_currentCount = initialCount;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Waits the async.
        /// </summary>
        /// <returns>The async.</returns>
        public Task WaitAsync()
        {
            lock (m_waiters)
            {
                if (m_currentCount > 0)
                {
                    --m_currentCount;
                    return s_completed;
                }
                else
                {
                    var waiter = new TaskCompletionSource<bool>();
                    m_waiters.Enqueue(waiter);
                    return waiter.Task;
                }
            }
        }

        /// <summary>
        /// Release this instance.
        /// </summary>
        public void Release()
        {
            TaskCompletionSource<bool> toRelease = null;

            lock (m_waiters)
            {
                if (m_waiters.Count > 0)
                    toRelease = m_waiters.Dequeue();
                else
                    ++m_currentCount;
            }

            if (toRelease != null)
                toRelease.SetResult(true);
        }

        #endregion
    }
}
