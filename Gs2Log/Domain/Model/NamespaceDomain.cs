/*
 * Copyright 2016 Game Server Services, Inc. or its affiliates. All Rights
 * Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable ArrangeThisQualifier
// ReSharper disable NotAccessedField.Local

#pragma warning disable 1998

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Log.Domain.Iterator;
using Gs2.Gs2Log.Request;
using Gs2.Gs2Log.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Log.Domain.Model
{

    public partial class NamespaceDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2LogRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string Status { get; set; }
        public string NextPageToken { get; set; }
        public long? TotalCount { get; set; }
        public long? ScanSize { get; set; }
        public string NamespaceName => _namespaceName;

        public NamespaceDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2LogRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._parentKey = "log:Gs2.Gs2Log.Model.Namespace";
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.NamespaceDomain> GetStatusAsync(
            #else
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> GetStatus(
            #endif
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.NamespaceDomain> GetStatusAsync(
        #endif
            GetNamespaceStatusRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetNamespaceStatusFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            var result = await this._client.GetNamespaceStatusAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            Gs2.Gs2Log.Domain.Model.NamespaceDomain domain = this;
            this.Status = domain.Status = result?.Status;
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Log.Model.Namespace> GetAsync(
            #else
        private IFuture<Gs2.Gs2Log.Model.Namespace> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Log.Model.Namespace> GetAsync(
        #endif
            GetNamespaceRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Model.Namespace> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetNamespaceFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            var result = await this._client.GetNamespaceAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Model.Namespace>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.NamespaceDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.NamespaceDomain> UpdateAsync(
        #endif
            UpdateNamespaceRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateNamespaceFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            var result = await this._client.UpdateNamespaceAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            Gs2.Gs2Log.Domain.Model.NamespaceDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.NamespaceDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.NamespaceDomain> DeleteAsync(
        #endif
            DeleteNamespaceRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteNamespaceFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            DeleteNamespaceResult result = null;
            try {
                result = await this._client.DeleteNamespaceAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            Gs2.Gs2Log.Domain.Model.NamespaceDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.NamespaceDomain> PutLogAsync(
            #else
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> PutLog(
            #endif
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.NamespaceDomain> PutLogAsync(
        #endif
            PutLogRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> self)
            {
        #endif
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.PutLogFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            var result = await this._client.PutLogAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            Gs2.Gs2Log.Domain.Model.NamespaceDomain domain = this;
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain>(Impl);
        #endif
        }

        public Gs2.Gs2Log.Domain.Model.LogDomain Log(
        ) {
            return new Gs2.Gs2Log.Domain.Model.LogDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Log.Model.AccessLog> AccessLog(
            string service,
            string method,
            string userId,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new QueryAccessLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                begin,
                end,
                longTerm
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.AccessLog> AccessLogAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Log.Model.AccessLog> AccessLog(
            #endif
        #else
        public QueryAccessLogIterator AccessLog(
        #endif
            string service,
            string method,
            string userId,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new QueryAccessLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                begin,
                end,
                longTerm
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Log.Model.AccessLogCount> CountAccessLog(
            bool? service,
            bool? method,
            bool? userId,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new CountAccessLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                begin,
                end,
                longTerm
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.AccessLogCount> CountAccessLogAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Log.Model.AccessLogCount> CountAccessLog(
            #endif
        #else
        public CountAccessLogIterator CountAccessLog(
        #endif
            bool? service,
            bool? method,
            bool? userId,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new CountAccessLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                begin,
                end,
                longTerm
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public Gs2.Gs2Log.Domain.Model.AccessLogDomain AccessLog(
        ) {
            return new Gs2.Gs2Log.Domain.Model.AccessLogDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName
            );
        }

        public Gs2.Gs2Log.Domain.Model.AccessLogCountDomain AccessLogCount(
        ) {
            return new Gs2.Gs2Log.Domain.Model.AccessLogCountDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Log.Model.ExecuteStampSheetLog> ExecuteStampSheetLog(
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new QueryExecuteStampSheetLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.ExecuteStampSheetLog> ExecuteStampSheetLogAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Log.Model.ExecuteStampSheetLog> ExecuteStampSheetLog(
            #endif
        #else
        public QueryExecuteStampSheetLogIterator ExecuteStampSheetLog(
        #endif
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new QueryExecuteStampSheetLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount> CountExecuteStampSheetLog(
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new CountExecuteStampSheetLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount> CountExecuteStampSheetLogAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount> CountExecuteStampSheetLog(
            #endif
        #else
        public CountExecuteStampSheetLogIterator CountExecuteStampSheetLog(
        #endif
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new CountExecuteStampSheetLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public Gs2.Gs2Log.Domain.Model.ExecuteStampSheetLogDomain ExecuteStampSheetLog(
        ) {
            return new Gs2.Gs2Log.Domain.Model.ExecuteStampSheetLogDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName
            );
        }

        public Gs2.Gs2Log.Domain.Model.ExecuteStampSheetLogCountDomain ExecuteStampSheetLogCount(
        ) {
            return new Gs2.Gs2Log.Domain.Model.ExecuteStampSheetLogCountDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Log.Model.ExecuteStampTaskLog> ExecuteStampTaskLog(
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new QueryExecuteStampTaskLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.ExecuteStampTaskLog> ExecuteStampTaskLogAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Log.Model.ExecuteStampTaskLog> ExecuteStampTaskLog(
            #endif
        #else
        public QueryExecuteStampTaskLogIterator ExecuteStampTaskLog(
        #endif
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new QueryExecuteStampTaskLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount> CountExecuteStampTaskLog(
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new CountExecuteStampTaskLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount> CountExecuteStampTaskLogAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount> CountExecuteStampTaskLog(
            #endif
        #else
        public CountExecuteStampTaskLogIterator CountExecuteStampTaskLog(
        #endif
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new CountExecuteStampTaskLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public Gs2.Gs2Log.Domain.Model.ExecuteStampTaskLogDomain ExecuteStampTaskLog(
        ) {
            return new Gs2.Gs2Log.Domain.Model.ExecuteStampTaskLogDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName
            );
        }

        public Gs2.Gs2Log.Domain.Model.ExecuteStampTaskLogCountDomain ExecuteStampTaskLogCount(
        ) {
            return new Gs2.Gs2Log.Domain.Model.ExecuteStampTaskLogCountDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Log.Model.IssueStampSheetLog> IssueStampSheetLog(
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new QueryIssueStampSheetLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.IssueStampSheetLog> IssueStampSheetLogAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Log.Model.IssueStampSheetLog> IssueStampSheetLog(
            #endif
        #else
        public QueryIssueStampSheetLogIterator IssueStampSheetLog(
        #endif
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new QueryIssueStampSheetLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Log.Model.IssueStampSheetLogCount> CountIssueStampSheetLog(
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new CountIssueStampSheetLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.IssueStampSheetLogCount> CountIssueStampSheetLogAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Log.Model.IssueStampSheetLogCount> CountIssueStampSheetLog(
            #endif
        #else
        public CountIssueStampSheetLogIterator CountIssueStampSheetLog(
        #endif
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm
        )
        {
            return new CountIssueStampSheetLogIterator(
                this._cache,
                this._client,
                this._namespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public Gs2.Gs2Log.Domain.Model.IssueStampSheetLogDomain IssueStampSheetLog(
        ) {
            return new Gs2.Gs2Log.Domain.Model.IssueStampSheetLogDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName
            );
        }

        public Gs2.Gs2Log.Domain.Model.IssueStampSheetLogCountDomain IssueStampSheetLogCount(
        ) {
            return new Gs2.Gs2Log.Domain.Model.IssueStampSheetLogCountDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "log",
                namespaceName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string namespaceName
        )
        {
            return string.Join(
                ":",
                namespaceName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Model.Namespace> Model() {
            #else
        public IFuture<Gs2.Gs2Log.Model.Namespace> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Log.Model.Namespace> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Model.Namespace> self)
            {
        #endif
            Gs2.Gs2Log.Model.Namespace value = _cache.Get<Gs2.Gs2Log.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetNamespaceRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "namespace")
                            {
                                _cache.Delete<Gs2.Gs2Log.Model.Namespace>(
                                    _parentKey,
                                    Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                                        this.NamespaceName?.ToString()
                                    )
                                );
                            }
                            else
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    if (e.errors[0].component == "namespace")
                    {
                    _cache.Delete<Gs2.Gs2Log.Model.Namespace>(
                            _parentKey,
                            Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                                this.NamespaceName?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Log.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName?.ToString()
                )
            );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Model.Namespace>(Impl);
        #endif
        }

    }
}
