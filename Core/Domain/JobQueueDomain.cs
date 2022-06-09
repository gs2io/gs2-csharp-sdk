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
        
        private object lockObject = new object();

        private Gs2 gs2;
        private List<string> tasks = new List<string>();

        public JobQueueDomain(
            Gs2 gs2
        ) {
            this.gs2 = gs2;
        }

        public void Push(
            string namespaceName
        ) {
            lock (lockObject)
            {
                tasks.Add(namespaceName);
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
            lock (lockObject) {
                if (tasks.Count > 0) {
                    namespaceName = tasks[0];
                }
            }
            if (namespaceName != null) {
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                var future = gs2.JobQueue.Namespace(
                    namespaceName
                ).AccessToken(
                    accessToken
                ).Run(
                    new RunRequest()
                );
                yield return future;
                if (future.Error != null)
                {
                    
                }
                JobAccessTokenDomain job = future.Result;
#else
                JobAccessTokenDomain job = await gs2.JobQueue.Namespace(
                    namespaceName
                ).AccessToken(
                    accessToken
                ).RunAsync(
                    new RunRequest()
                );
#endif 
                if (job.IsLastJob.HasValue && job.IsLastJob.Value) {
                    lock (lockObject) {
                        tasks.Remove(namespaceName);
                    }
                }
            }
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(tasks.Count == 0);
#else
            return tasks.Count == 0;
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
            lock (lockObject) {
                if (tasks.Count > 0) {
                    namespaceName = tasks[0];
                }
            }
            if (namespaceName != null) {
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                var future = gs2.JobQueue.Namespace(
                    namespaceName
                ).User(
                    userId
                ).Run(
                    new RunByUserIdRequest()
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                JobDomain job = future.Result;
#else
                JobDomain job = await gs2.JobQueue.Namespace(
                    namespaceName
                ).User(
                    userId
                ).RunAsync(
                    new RunByUserIdRequest()
                );
#endif 
                if (job.IsLastJob.HasValue && job.IsLastJob.Value) {
                    lock (lockObject) {
                        tasks.Remove(namespaceName);
                    }
                }
            }
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(tasks.Count == 0);
#else
            return tasks.Count == 0;
#endif 
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }

            return new Gs2InlineFuture<bool>(Impl);
#endif
        }
    }
}