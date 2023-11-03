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
using UnityEngine;
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
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LogRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string Status { get; set; }
        public string NextPageToken { get; set; }
        public long? TotalCount { get; set; }
        public long? ScanSize { get; set; }
        public string NamespaceName => _namespaceName;

        public NamespaceDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LogRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._parentKey = "log:Namespace";
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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
        public QueryAccessLogIterator AccessLogAsync(
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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

        public ulong SubscribeAccessLog(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.AccessLog>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "AccessLog"
                ),
                callback
            );
        }

        public void UnsubscribeAccessLog(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.AccessLog>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "AccessLog"
                ),
                callbackId
            );
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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
        public CountAccessLogIterator CountAccessLogAsync(
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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

        public ulong SubscribeCountAccessLog(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.AccessLogCount>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "AccessLogCount"
                ),
                callback
            );
        }

        public void UnsubscribeCountAccessLog(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.AccessLogCount>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "AccessLogCount"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Log.Domain.Model.AccessLogDomain AccessLog(
        ) {
            return new Gs2.Gs2Log.Domain.Model.AccessLogDomain(
                this._gs2,
                this.NamespaceName
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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
        public QueryExecuteStampSheetLogIterator ExecuteStampSheetLogAsync(
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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

        public ulong SubscribeExecuteStampSheetLog(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.ExecuteStampSheetLog>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "ExecuteStampSheetLog"
                ),
                callback
            );
        }

        public void UnsubscribeExecuteStampSheetLog(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.ExecuteStampSheetLog>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "ExecuteStampSheetLog"
                ),
                callbackId
            );
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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
        public CountExecuteStampSheetLogIterator CountExecuteStampSheetLogAsync(
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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

        public ulong SubscribeCountExecuteStampSheetLog(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "ExecuteStampSheetLogCount"
                ),
                callback
            );
        }

        public void UnsubscribeCountExecuteStampSheetLog(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "ExecuteStampSheetLogCount"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Log.Domain.Model.ExecuteStampSheetLogDomain ExecuteStampSheetLog(
        ) {
            return new Gs2.Gs2Log.Domain.Model.ExecuteStampSheetLogDomain(
                this._gs2,
                this.NamespaceName
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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
        public QueryExecuteStampTaskLogIterator ExecuteStampTaskLogAsync(
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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

        public ulong SubscribeExecuteStampTaskLog(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.ExecuteStampTaskLog>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "ExecuteStampTaskLog"
                ),
                callback
            );
        }

        public void UnsubscribeExecuteStampTaskLog(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.ExecuteStampTaskLog>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "ExecuteStampTaskLog"
                ),
                callbackId
            );
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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
        public CountExecuteStampTaskLogIterator CountExecuteStampTaskLogAsync(
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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

        public ulong SubscribeCountExecuteStampTaskLog(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "ExecuteStampTaskLogCount"
                ),
                callback
            );
        }

        public void UnsubscribeCountExecuteStampTaskLog(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "ExecuteStampTaskLogCount"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Log.Domain.Model.ExecuteStampTaskLogDomain ExecuteStampTaskLog(
        ) {
            return new Gs2.Gs2Log.Domain.Model.ExecuteStampTaskLogDomain(
                this._gs2,
                this.NamespaceName
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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
        public QueryIssueStampSheetLogIterator IssueStampSheetLogAsync(
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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

        public ulong SubscribeIssueStampSheetLog(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.IssueStampSheetLog>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "IssueStampSheetLog"
                ),
                callback
            );
        }

        public void UnsubscribeIssueStampSheetLog(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.IssueStampSheetLog>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "IssueStampSheetLog"
                ),
                callbackId
            );
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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
        public CountIssueStampSheetLogIterator CountIssueStampSheetLogAsync(
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
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
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

        public ulong SubscribeCountIssueStampSheetLog(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.IssueStampSheetLogCount>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "IssueStampSheetLogCount"
                ),
                callback
            );
        }

        public void UnsubscribeCountIssueStampSheetLog(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.IssueStampSheetLogCount>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "IssueStampSheetLogCount"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Log.Domain.Model.IssueStampSheetLogDomain IssueStampSheetLog(
        ) {
            return new Gs2.Gs2Log.Domain.Model.IssueStampSheetLogDomain(
                this._gs2,
                this.NamespaceName
            );
        }

        public Gs2.Gs2Log.Domain.Model.LogDomain Log(
        ) {
            return new Gs2.Gs2Log.Domain.Model.LogDomain(
                this._gs2,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Log.Model.Insight> Insights(
        )
        {
            return new DescribeInsightsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.Insight> InsightsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Log.Model.Insight> Insights(
            #endif
        #else
        public DescribeInsightsIterator InsightsAsync(
        #endif
        )
        {
            return new DescribeInsightsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName
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

        public ulong SubscribeInsights(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.Insight>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Insight"
                ),
                callback
            );
        }

        public void UnsubscribeInsights(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.Insight>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Insight"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Log.Domain.Model.InsightDomain Insight(
            string insightName
        ) {
            return new Gs2.Gs2Log.Domain.Model.InsightDomain(
                this._gs2,
                this.NamespaceName,
                insightName
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

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetNamespaceStatusFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "namespace")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName);
                GetNamespaceStatusResult result = null;
                try {
                    result = await this._client.GetNamespaceStatusAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                        );
                    this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "namespace")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Status = domain.Status = result?.Status;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.NamespaceDomain> GetStatusAsync(
            GetNamespaceStatusRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.GetNamespaceStatusFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                    );
                    this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "namespace")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName);
            GetNamespaceStatusResult result = null;
            try {
                result = await this._client.GetNamespaceStatusAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.NamespaceDomain> GetStatusAsync(
            GetNamespaceStatusRequest request
        ) {
            var future = GetStatusFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to GetStatusFuture.")]
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> GetStatus(
            GetNamespaceStatusRequest request
        ) {
            return GetStatusFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Log.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Log.Model.Namespace> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetNamespaceFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "namespace")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName);
                GetNamespaceResult result = null;
                try {
                    result = await this._client.GetNamespaceAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                        );
                    this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "namespace")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "log",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Model.Namespace>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Log.Model.Namespace> GetAsync(
            GetNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.GetNamespaceFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                    );
                    this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "namespace")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName);
            GetNamespaceResult result = null;
            try {
                result = await this._client.GetNamespaceAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "log",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
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
                request
                    .WithNamespaceName(this.NamespaceName);
                UpdateNamespaceResult result = null;
                    result = await this._client.UpdateNamespaceAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "log",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.NamespaceDomain> UpdateAsync(
            UpdateNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
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
            request
                .WithNamespaceName(this.NamespaceName);
            UpdateNamespaceResult result = null;
                result = await this._client.UpdateNamespaceAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "log",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.NamespaceDomain> UpdateAsync(
            UpdateNamespaceRequest request
        ) {
            var future = UpdateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> Update(
            UpdateNamespaceRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.DeleteNamespaceFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                            request.NamespaceName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "namespace")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName);
                DeleteNamespaceResult result = null;
                try {
                    result = await this._client.DeleteNamespaceAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                        );
                    this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "namespace")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "log",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Log.Model.Namespace>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.NamespaceDomain> DeleteAsync(
            DeleteNamespaceRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.DeleteNamespaceFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                        request.NamespaceName.ToString()
                    );
                    this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "namespace")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName);
            DeleteNamespaceResult result = null;
            try {
                result = await this._client.DeleteNamespaceAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                    request.NamespaceName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "log",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Log.Model.Namespace>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.NamespaceDomain> DeleteAsync(
            DeleteNamespaceRequest request
        ) {
            var future = DeleteFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> Delete(
            DeleteNamespaceRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> PutLogFuture(
            PutLogRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
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
                PutLogResult result = null;
                    result = await this._client.PutLogAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.NamespaceDomain> PutLogAsync(
            PutLogRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
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
            PutLogResult result = null;
                result = await this._client.PutLogAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.NamespaceDomain> PutLogAsync(
            PutLogRequest request
        ) {
            var future = PutLogFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to PutLogFuture.")]
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> PutLog(
            PutLogRequest request
        ) {
            return PutLogFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Domain.Model.InsightDomain> CreateInsightFuture(
            CreateInsightRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.InsightDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.CreateInsightFuture(
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
                request
                    .WithNamespaceName(this.NamespaceName);
                CreateInsightResult result = null;
                    result = await this._client.CreateInsightAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Insight"
                        );
                        var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Log.Domain.Model.InsightDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.InsightDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.InsightDomain> CreateInsightAsync(
            CreateInsightRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.CreateInsightFuture(
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
            request
                .WithNamespaceName(this.NamespaceName);
            CreateInsightResult result = null;
                result = await this._client.CreateInsightAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Insight"
                    );
                    var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Log.Domain.Model.InsightDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.InsightDomain> CreateInsightAsync(
            CreateInsightRequest request
        ) {
            var future = CreateInsightFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateInsightFuture.")]
        public IFuture<Gs2.Gs2Log.Domain.Model.InsightDomain> CreateInsight(
            CreateInsightRequest request
        ) {
            return CreateInsightFuture(request);
        }
        #endif

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Model.Namespace> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Model.Namespace> self)
            {
                var parentKey = string.Join(
                    ":",
                    "log",
                    "Namespace"
                );
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Log.Model.Namespace>(
                    parentKey,
                    Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                        this.NamespaceName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetNamespaceRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                                    this.NamespaceName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "namespace")
                            {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Log.Model.Namespace>(
                        parentKey,
                        Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                            this.NamespaceName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Model.Namespace>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Log.Model.Namespace> ModelAsync()
        {
            var parentKey = string.Join(
                ":",
                "log",
                "Namespace"
            );
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Log.Model.Namespace>(
                    parentKey,
                    Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                        this.NamespaceName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetNamespaceRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                                    this.NamespaceName?.ToString()
                                );
                    this._gs2.Cache.Put<Gs2.Gs2Log.Model.Namespace>(
                        parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "namespace")
                    {
                        throw;
                    }
                }
                (value, _) = _gs2.Cache.Get<Gs2.Gs2Log.Model.Namespace>(
                        parentKey,
                        Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                            this.NamespaceName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Model.Namespace> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Log.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Log.Model.Namespace> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Log.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Log.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Log.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                ),
                callbackId
            );
        }

    }
}
