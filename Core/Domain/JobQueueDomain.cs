using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2JobQueue.Domain.Model;
using Gs2.Gs2JobQueue.Request;

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

#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
        public Gs2Future<bool> Run(
#else
        public async Task<bool> Run(
#endif
            AccessToken accessToken
        ) {
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(Gs2Future<bool> self)
            {
#endif
            string namespaceName = null;
            lock (this._lockObject) {
                if (this._tasks.Count > 0) {
                    namespaceName = this._tasks[0];
                }
            }
            if (namespaceName != null) {
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
#else
                var job = await this._gs2.JobQueue.Namespace(
                    namespaceName
                ).AccessToken(
                    accessToken
                ).RunAsync(
                    new RunRequest()
                );
#endif 
                if (job.IsLastJob.HasValue && job.IsLastJob.Value) {
                    lock (this._lockObject) {
                        this._tasks.Remove(namespaceName);
                    }
                }
            }
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                if (this._lockObject != null) {
                    lock (this._lockObject) {
                        self.OnComplete(this._tasks.Count == 0);
                    }
                }
#else
            lock (this._lockObject) {
                return this._tasks.Count == 0;
            }
#endif 
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }

            return new Gs2InlineFuture<bool>(Impl);
#endif
        }

#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
        public Gs2Future<bool> RunByUserId(
#else
        public async Task<bool> RunByUserId(
#endif 
            string userId
        ) {
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(Gs2Future<bool> self)
            {
#endif
            string namespaceName = null;
            lock (this._lockObject) {
                if (this._tasks.Count > 0) {
                    namespaceName = this._tasks[0];
                }
            }
            if (namespaceName != null) {
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
#else
                var job = await this._gs2.JobQueue.Namespace(
                    namespaceName
                ).User(
                    userId
                ).RunAsync(
                    new RunByUserIdRequest()
                );
#endif 
                if (job.IsLastJob.HasValue && job.IsLastJob.Value) {
                    lock (this._lockObject) {
                        this._tasks.Remove(namespaceName);
                    }
                }
            }
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            lock (this._lockObject) {
                self.OnComplete(this._tasks.Count == 0);
            }
#else
            lock (this._lockObject) {
                return this._tasks.Count == 0;
            }
#endif 
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }

            return new Gs2InlineFuture<bool>(Impl);
#endif
        }
    }
}