using System;
using System.Threading.Tasks;

namespace Core.Util
{
    public class AsyncLock : IDisposable
    {
        private readonly System.Threading.SemaphoreSlim _semaphore = new(1, 1);

        public async Task<IDisposable> LockAsync()
        {
            await this._semaphore.WaitAsync();
            return this;
        }
        
        public void Dispose() {
            this._semaphore.Release();
        }
    }
}
