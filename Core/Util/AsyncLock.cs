using System;
using System.Threading.Tasks;

namespace Core.Util
{
    public class AsyncLock
    {
        private readonly System.Threading.SemaphoreSlim _semaphore 
            = new System.Threading.SemaphoreSlim(1, 1);
        private readonly Task<IDisposable> _releaser;

        public AsyncLock()
        {
            this._releaser = Task.FromResult((IDisposable)new Releaser(this));
        }

        public Task<IDisposable> LockAsync()
        {
            var wait = this._semaphore.WaitAsync();
            return wait.IsCompleted ?
                this._releaser :
                wait.ContinueWith(
                    (_, state) => (IDisposable)state,
                    this._releaser.Result, 
                    System.Threading.CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, 
                    TaskScheduler.Default
                );
        }
        private sealed class Releaser : IDisposable
        {
            private readonly AsyncLock _toRelease;
            internal Releaser(AsyncLock toRelease) { this._toRelease = toRelease; }
            public void Dispose() { this._toRelease._semaphore.Release(); }
        }
    }
}
