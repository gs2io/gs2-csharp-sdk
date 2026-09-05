using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Util;

namespace Gs2.Core.Domain
{
    internal sealed class ExpiringHandledResultSet
    {
        private readonly object _syncRoot = new object();
        private readonly Dictionary<string, long> _handled = new Dictionary<string, long>();
        private readonly Func<long> _currentTime;
        private readonly long _retentionMilliseconds;

        internal ExpiringHandledResultSet(TimeSpan retention) : this(
            () => UnixTime.ToUnixTime(DateTime.Now),
            (long)retention.TotalMilliseconds
        )
        {
        }

        internal ExpiringHandledResultSet(Func<long> currentTime, long retentionMilliseconds)
        {
            this._currentTime = currentTime;
            this._retentionMilliseconds = retentionMilliseconds;
        }

        internal bool TryHandle(string key)
        {
            lock (this._syncRoot)
            {
                var now = this._currentTime.Invoke();
                foreach (var expiredKey in this._handled
                             .Where(pair => pair.Value < now)
                             .Select(pair => pair.Key)
                             .ToArray())
                {
                    this._handled.Remove(expiredKey);
                }

                if (this._handled.ContainsKey(key))
                {
                    return false;
                }

                this._handled.Add(key, now + this._retentionMilliseconds);
                return true;
            }
        }
    }
}
