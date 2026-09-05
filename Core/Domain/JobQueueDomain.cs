using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Gs2.Core.Exception;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2JobQueue.Request;
#if UNITY_2017_1_OR_NEWER 
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    [Obsolete("Use of the auto-execute feature is strongly recommended")]
    public class JobQueueDomain
    {
        private static readonly TimeSpan RunRetryTimeout = TimeSpan.FromSeconds(15);
        private readonly SemaphoreSlim _semaphore  = new SemaphoreSlim(1, 1);

        private readonly Gs2 _gs2;
        private readonly object _tasksLock = new object();
        private sealed class PendingTask : IEquatable<PendingTask>
        {
            internal readonly string NamespaceName;
            internal readonly string UserId;
            internal readonly bool IsWildcard;

            internal PendingTask(string namespaceName, string userId, bool isWildcard)
            {
                NamespaceName = namespaceName;
                UserId = userId;
                IsWildcard = isWildcard;
            }

            public bool Equals(PendingTask other)
            {
                return other != null &&
                       IsWildcard == other.IsWildcard &&
                       string.Equals(NamespaceName, other.NamespaceName, StringComparison.Ordinal) &&
                       string.Equals(UserId, other.UserId, StringComparison.Ordinal);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as PendingTask);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = NamespaceName.GetHashCode();
                    hashCode = (hashCode * 397) ^ (UserId?.GetHashCode() ?? 0);
                    hashCode = (hashCode * 397) ^ IsWildcard.GetHashCode();
                    return hashCode;
                }
            }

            internal bool Matches(string userId)
            {
                return IsWildcard || string.Equals(UserId, userId, StringComparison.Ordinal);
            }
        }

        private readonly HashSet<PendingTask> _tasks = new HashSet<PendingTask>();
        internal int PendingTaskCount
        {
            get
            {
                lock (_tasksLock)
                {
                    return _tasks.Count;
                }
            }
        }

        public JobQueueDomain(
            Gs2 gs2
        ) {
            this._gs2 = gs2;
        }

        public void Push(
            string namespaceName
        ) {
            ValidateNamespaceName(namespaceName);
            PushPendingTask(new PendingTask(namespaceName, null, true));
        }

        internal void PushForUser(
            string namespaceName,
            string userId
        ) {
            ValidateNamespaceName(namespaceName);
            ValidateUserId(userId);
            PushPendingTask(new PendingTask(namespaceName, userId, false));
        }

        private static void ValidateNamespaceName(string namespaceName)
        {
            if (string.IsNullOrEmpty(namespaceName))
            {
                throw new ArgumentException(
                    "Namespace name must not be null or empty.",
                    nameof(namespaceName)
                );
            }
        }

        private static void ValidateUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(
                    "User ID must not be null or empty.",
                    nameof(userId)
                );
            }
        }

        private void PushPendingTask(PendingTask task)
        {
            lock (_tasksLock)
            {
                this._tasks.Add(task);
            }
        }

        internal string TakeNextTask()
        {
            lock (_tasksLock)
            {
                var task = this._tasks.FirstOrDefault();
                if (task != null)
                {
                    this._tasks.Remove(task);
                }
                return task?.NamespaceName;
            }
        }

        internal string TakeNextTaskForUser(string userId)
        {
            return TakeNextPendingTaskForUser(userId)?.NamespaceName;
        }

        private PendingTask TakeNextPendingTaskForUser(string userId)
        {
            ValidateUserId(userId);
            lock (_tasksLock)
            {
                var task = this._tasks.FirstOrDefault(candidate =>
                    !candidate.IsWildcard &&
                    string.Equals(candidate.UserId, userId, StringComparison.Ordinal)
                ) ?? this._tasks.FirstOrDefault(candidate => candidate.IsWildcard);
                if (task != null)
                {
                    this._tasks.Remove(task);
                }
                return task;
            }
        }

        private bool IsEmptyForUser(string userId)
        {
            ValidateUserId(userId);
            lock (_tasksLock)
            {
                return !this._tasks.Any(task => task.Matches(userId));
            }
        }

#if UNITY_2017_1_OR_NEWER
        public Gs2Future<bool> RunFuture(
            AccessToken accessToken
        ) => RunAsync(accessToken).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public async UniTask<bool> RunAsync(
#else
        public async Task<bool> RunAsync(
#endif
            AccessToken accessToken
        ) {
            PendingTask pendingTask = null;
            var requeue = false;
            await TaskUtilities.WaitAsync(this._semaphore);
            try {
                pendingTask = TakeNextPendingTaskForUser(accessToken?.UserId);
                requeue = pendingTask != null;
                if (pendingTask != null) {
                    var timer = Stopwatch.StartNew();
                    RETRY:
                    try {
                        var job = await this._gs2.JobQueue.Namespace(
                            pendingTask.NamespaceName
                        ).AccessToken(
                            accessToken
                        ).RunAsync(
                            new RunRequest()
                        );
                        if (job.IsLastJob.HasValue && job.IsLastJob.Value) {
                            requeue = false;
                        }
                    }
                    catch (Gs2Exception e) {
                        if (!e.RecommendAutoRetry || timer.Elapsed > RunRetryTimeout) {
                            throw;
                        }
                        await TaskUtilities.DelayAsync(Gs2Constant.RetryWait);
                        goto RETRY;
                    }
                }
                if (requeue)
                {
                    PushPendingTask(pendingTask);
                    requeue = false;
                }
                return IsEmptyForUser(accessToken?.UserId);
            }
            finally {
                if (requeue)
                {
                    PushPendingTask(pendingTask);
                }
                this._semaphore.Release();
            }
        }

#if UNITY_2017_1_OR_NEWER
        public Gs2Future<bool> RunByUserIdFuture(
            string userId
        ) => RunByUserIdAsync(userId).ToGs2Future();
#endif
        
#if GS2_ENABLE_UNITASK
        public async UniTask<bool> RunByUserIdAsync(
#else
        public async Task<bool> RunByUserIdAsync(
#endif
            string userId
        ) {
            PendingTask pendingTask = null;
            var requeue = false;
            await TaskUtilities.WaitAsync(this._semaphore);
            try {
                pendingTask = TakeNextPendingTaskForUser(userId);
                requeue = pendingTask != null;
                if (pendingTask != null) {
                    var timer = Stopwatch.StartNew();
                    RETRY:
                    try {
                        var job = await this._gs2.JobQueue.Namespace(
                            pendingTask.NamespaceName
                        ).User(
                            userId
                        ).RunAsync(
                            new RunByUserIdRequest()
                        );
                        if (job.IsLastJob.HasValue && job.IsLastJob.Value) {
                            requeue = false;
                        }
                    }
                    catch (Gs2Exception e) {
                        if (!e.RecommendAutoRetry || timer.Elapsed > RunRetryTimeout) {
                            throw;
                        }
                        await TaskUtilities.DelayAsync(Gs2Constant.RetryWait);
                        goto RETRY;
                    }
                }
                if (requeue)
                {
                    PushPendingTask(pendingTask);
                    requeue = false;
                }
                return IsEmptyForUser(userId);
            }
            finally {
                if (requeue)
                {
                    PushPendingTask(pendingTask);
                }
                this._semaphore.Release();
            }
        }
    }
}
