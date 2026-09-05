using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gs2.Core.Util;

namespace Core.Util
{
    public class AsyncLock : IDisposable
    {
        private sealed class Releaser : IDisposable
        {
            private System.Threading.SemaphoreSlim _semaphore;
            private Action _onRelease;
            private Action _onDisposed;

            internal Releaser(
                System.Threading.SemaphoreSlim semaphore,
                Action onRelease,
                Action onDisposed
            )
            {
                _semaphore = semaphore;
                _onRelease = onRelease;
                _onDisposed = onDisposed;
            }

            public void Dispose()
            {
                var semaphore = System.Threading.Interlocked.Exchange(ref _semaphore, null);
                if (semaphore == null)
                {
                    return;
                }
                var onRelease = System.Threading.Interlocked.Exchange(ref _onRelease, null);
                var onDisposed = System.Threading.Interlocked.Exchange(ref _onDisposed, null);
                try
                {
                    onDisposed?.Invoke();
                }
                finally
                {
                    try
                    {
                        semaphore.Release();
                    }
                    finally
                    {
                        onRelease?.Invoke();
                    }
                }
            }
        }

        private sealed class PendingLease
        {
            internal bool DisposeRequested;
        }

        private readonly System.Threading.SemaphoreSlim _semaphore;
        private Tuple<System.Threading.SemaphoreSlim, Action> _initialLease;
        private readonly Func<Tuple<System.Threading.SemaphoreSlim, Action>> _acquireLease;
        private readonly Action _afterOwnerTargetSnapshot;
        private readonly object _stateLock = new object();
        private readonly List<PendingLease> _pendingLeases = new List<PendingLease>();
        private Releaser _ownedLease;

        public AsyncLock()
        {
            _semaphore = new System.Threading.SemaphoreSlim(1, 1);
        }

        internal AsyncLock(
            System.Threading.SemaphoreSlim semaphore,
            Action onRelease,
            Func<Tuple<System.Threading.SemaphoreSlim, Action>> acquireLease,
            Action afterOwnerTargetSnapshot = null
        )
        {
            _initialLease = Tuple.Create(semaphore, onRelease);
            _acquireLease = acquireLease;
            _afterOwnerTargetSnapshot = afterOwnerTargetSnapshot;
        }

        public async Task<IDisposable> LockAsync()
        {
            if (_acquireLease == null)
            {
                await TaskUtilities.WaitAsync(this._semaphore);
                Releaser releaser = null;
                releaser = new Releaser(
                    this._semaphore,
                    null,
                    () => ClearOwnedLease(releaser)
                );
                System.Threading.Interlocked.Exchange(ref _ownedLease, releaser);
                return releaser;
            }

            Tuple<System.Threading.SemaphoreSlim, Action> lease;
            PendingLease pendingLease;
            lock (_stateLock)
            {
                lease = System.Threading.Interlocked.Exchange(ref _initialLease, null) ?? _acquireLease.Invoke();
                pendingLease = new PendingLease();
                _pendingLeases.Add(pendingLease);
            }
            try
            {
                await TaskUtilities.WaitAsync(lease.Item1);
            }
            catch
            {
                lock (_stateLock)
                {
                    _pendingLeases.Remove(pendingLease);
                }
                lease.Item2?.Invoke();
                throw;
            }
            Releaser ownedLease = null;
            var disposeRequested = false;
            lock (_stateLock)
            {
                _pendingLeases.Remove(pendingLease);
                disposeRequested = pendingLease.DisposeRequested;
                ownedLease = new Releaser(
                    lease.Item1,
                    lease.Item2,
                    () => ClearOwnedLease(ownedLease)
                );
                if (!disposeRequested)
                {
                    _ownedLease = ownedLease;
                }
            }
            if (disposeRequested)
            {
                ownedLease.Dispose();
                throw new ObjectDisposedException(nameof(AsyncLock));
            }
            return ownedLease;
        }

        private void ClearOwnedLease(Releaser releaser)
        {
            lock (_stateLock)
            {
                if (ReferenceEquals(_ownedLease, releaser))
                {
                    _ownedLease = null;
                }
            }
        }

        ~AsyncLock()
        {
            ReleaseUnusedInitialLease();
        }

        private void ReleaseUnusedInitialLease()
        {
            var lease = System.Threading.Interlocked.Exchange(ref _initialLease, null);
            lease?.Item2?.Invoke();
        }

        public void Dispose()
        {
            if (_acquireLease == null)
            {
                var defaultOwnedLease = System.Threading.Interlocked.Exchange(ref _ownedLease, null);
                if (defaultOwnedLease != null)
                {
                    defaultOwnedLease.Dispose();
                }
                return;
            }

            var ownedLease = System.Threading.Volatile.Read(ref _ownedLease);
            _afterOwnerTargetSnapshot?.Invoke();
            lock (_stateLock)
            {
                if (ownedLease != null)
                {
                    if (ReferenceEquals(_ownedLease, ownedLease))
                    {
                        _ownedLease = null;
                    }
                    else
                    {
                        ownedLease = null;
                    }
                }
                else
                {
                    ownedLease = _ownedLease;
                    if (ownedLease != null)
                    {
                        _ownedLease = null;
                    }
                    else
                    {
                        foreach (var pendingLease in _pendingLeases)
                        {
                            if (pendingLease.DisposeRequested)
                            {
                                continue;
                            }
                            pendingLease.DisposeRequested = true;
                            break;
                        }
                    }
                }
            }
            if (ownedLease != null)
            {
                ownedLease.Dispose();
            }
            ReleaseUnusedInitialLease();
            GC.SuppressFinalize(this);
        }
    }
}
