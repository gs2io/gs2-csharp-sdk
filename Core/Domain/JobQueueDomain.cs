using System.Collections;
using System.Collections.Generic;
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
    public class JobQueueDomain
    {
        
        private readonly object _lockObject = new object();

        private readonly Gs2 _gs2;
        private readonly List<string> _tasks = new List<string>();

        public JobQueueDomain(
            Gs2 gs2
        ) {
            this._gs2 = gs2;
        }

        public void Push(
            string namespaceName
        ) {
            lock (this._lockObject)
            {
                this._tasks.Add(namespaceName);
            }
        }

#if UNITY_2017_1_OR_NEWER
        public Gs2Future<bool> RunFuture(
            AccessToken accessToken
        ) {
            IEnumerator Impl(Gs2Future<bool> self)
            {
                string namespaceName = null;
                lock (this._lockObject) {
                    if (this._tasks.Count > 0) {
                        namespaceName = this._tasks[0];
                    }
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
                    if (future.Error != null)
                    {
                        
                    }
                    var job = future.Result;
                    if (job.IsLastJob.HasValue && job.IsLastJob.Value) {
                        lock (this._lockObject) {
                            this._tasks.Remove(namespaceName);
                        }
                    }
                }
                if (this._lockObject != null) {
                    self.OnComplete(true);
                    yield break;
                }
                lock (this._lockObject) {
                    self.OnComplete(this._tasks.Count == 0);
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
            lock (this._lockObject) {
                if (this._tasks.Count > 0) {
                    namespaceName = this._tasks[0];
                }
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
                    lock (this._lockObject) {
                        this._tasks.Remove(namespaceName);
                    }
                }
            }
            if (this._lockObject != null) {
                return true;
            }
            lock (this._lockObject) {
                return this._tasks.Count == 0;
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
                lock (this._lockObject) {
                    if (this._tasks.Count > 0) {
                        namespaceName = this._tasks[0];
                    }
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
                        lock (this._lockObject) {
                            this._tasks.Remove(namespaceName);
                        }
                    }
                }
                if (this._lockObject != null) {
                    self.OnComplete(true);
                    yield break;
                }
                lock (this._lockObject) {
                    self.OnComplete(this._tasks.Count == 0);
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
            lock (this._lockObject) {
                if (this._tasks.Count > 0) {
                    namespaceName = this._tasks[0];
                }
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
                    lock (this._lockObject) {
                        this._tasks.Remove(namespaceName);
                    }
                }
            }
            if (this._lockObject != null) {
                return true;
            }
            lock (this._lockObject) {
                return this._tasks.Count == 0;
            }
        }
#endif
    }
}