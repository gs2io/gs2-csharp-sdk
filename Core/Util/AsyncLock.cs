using System;
using System.Threading.Tasks;
using Gs2.Core.Util;

namespace Core.Util
{
    public class AsyncLock : IDisposable
    {
        private readonly System.Threading.SemaphoreSlim _semaphore = new(1, 1);

        public async Task<IDisposable> LockAsync()
        {
            await TaskUtilities.WaitAsync(this._semaphore);
            return this;
        }
        
        public void Dispose() {
            this._semaphore.Release();
        }
    }
}
