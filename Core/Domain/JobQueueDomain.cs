using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2JobQueue.Request;
#if UNITY_2017_1_OR_NEWER 
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    [Obsolete("Use of the auto-execute feature is strongly recommended")]
    public class JobQueueDomain
    {
        private readonly SemaphoreSlim _semaphore  = new SemaphoreSlim(1, 1);

        private readonly Gs2 _gs2;
        private readonly HashSet<string> _tasks = new HashSet<string>();

        public JobQueueDomain(
            Gs2 gs2
        ) {
            this._gs2 = gs2;
        }

        public void Push(
            string namespaceName
        ) {
            this._tasks.Add(namespaceName);
        }

#if UNITY_2017_1_OR_NEWER
        public Gs2Future<bool> RunFuture(
            AccessToken accessToken
        ) {
            IEnumerator Impl(Gs2Future<bool> self)
            {
                string namespaceName = null;
                while (!this._semaphore.Wait(0)) {
                    yield return null;
                }
                try {
                    if (this._tasks.Count > 0) {
                        namespaceName = this._tasks.First();
                    }
                    if (namespaceName != null) {
                        var future = this._gs2.JobQueue.Namespace(
                            namespaceName
                        ).AccessToken(
                            accessToken
                        ).RunFuture(
                            new RunRequest()
                        );
                        yield return future;
                        if (future.Error != null) {

                        }
                        var job = future.Result;
                        if (job.IsLastJob.HasValue && job.IsLastJob.Value) {
                            this._tasks.Remove(namespaceName);
                        }
                    }
                    self.OnComplete(this._tasks.Count == 0);
                }
                finally {
                    this._semaphore.Release();
                }
            }

            return new Gs2InlineFuture<bool>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if GS2_ENABLE_UNITASK
        public async UniTask<bool> RunAsync(
    #else
        public async Task<bool> RunAsync(
    #endif
            AccessToken accessToken
        ) {
            string namespaceName = null;
            while (!await this._semaphore.WaitAsync(0)) {
    #if GS2_ENABLE_UNITASK
                await UniTask.Yield();
    #else
                await Task.Yield();
    #endif
            }
            try {
                if (this._tasks.Count > 0) {
                    namespaceName = this._tasks.First();
                }
                if (namespaceName != null) {
                    var job = await this._gs2.JobQueue.Namespace(
                        namespaceName
                    ).AccessToken(
                        accessToken
                    ).RunAsync(
                        new RunRequest()
                    );
                    if (job.IsLastJob.HasValue && job.IsLastJob.Value) {
                        this._tasks.Remove(namespaceName);
                    }
                }
                return this._tasks.Count == 0;
            }
            finally {
                this._semaphore.Release();
            }
        }
#endif

#if UNITY_2017_1_OR_NEWER
        public Gs2Future<bool> RunByUserIdFuture(
            string userId
        ) {
            IEnumerator Impl(Gs2Future<bool> self)
            {
                string namespaceName = null;
                while (!this._semaphore.Wait(0)) {
                    yield return null;
                }
                try {
                    if (this._tasks.Count > 0) {
                        namespaceName = this._tasks.First();
                    }
                    if (namespaceName != null) {
                        var future = this._gs2.JobQueue.Namespace(
                            namespaceName
                        ).User(
                            userId
                        ).RunFuture(
                            new RunByUserIdRequest()
                        );
                        yield return future;
                        if (future.Error != null)
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                        var job = future.Result;
                        if (job.IsLastJob.HasValue && job.IsLastJob.Value) {
                            this._tasks.Remove(namespaceName);
                        }
                    }
                    self.OnComplete(this._tasks.Count == 0);
                }
                finally {
                    this._semaphore.Release();
                }
            }

            return new Gs2InlineFuture<bool>(Impl);
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if GS2_ENABLE_UNITASK
        public async UniTask<bool> RunByUserIdAsync(
    #else
        public async Task<bool> RunByUserIdAsync(
    #endif
            string userId
        ) {
            string namespaceName = null;
            while (!await this._semaphore.WaitAsync(0)) {
    #if GS2_ENABLE_UNITASK
                await UniTask.Yield();
    #else
                await Task.Yield();
    #endif
            }
            try {
                if (this._tasks.Count > 0) {
                    namespaceName = this._tasks.First();
                }
                if (namespaceName != null) {
                    var job = await this._gs2.JobQueue.Namespace(
                        namespaceName
                    ).User(
                        userId
                    ).RunAsync(
                        new RunByUserIdRequest()
                    );
                    if (job.IsLastJob.HasValue && job.IsLastJob.Value) {
                        this._tasks.Remove(namespaceName);
                    }
                }
                return this._tasks.Count == 0;
            }
            finally {
                this._semaphore.Release();
            }
        }
#endif
    }
}