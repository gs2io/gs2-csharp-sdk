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
#pragma warning disable CS0169, CS0168

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
using Cysharp.Threading.Tasks.Linq;
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

        public ulong SubscribeAccessLog(
            string service,
            string method,
            string userId,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.AccessLog[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.AccessLog>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "AccessLog"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeAccessLogWithInitialCallAsync(
            string service,
            string method,
            string userId,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.AccessLog[]> callback
        )
        {
            var items = await AccessLogAsync(
                service,
                method,
                userId,
                begin,
                end,
                longTerm
            ).ToArrayAsync();
            var callbackId = SubscribeAccessLog(
                service,
                method,
                userId,
                begin,
                end,
                longTerm,
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeAccessLog(
            string service,
            string method,
            string userId,
            long? begin,
            long? end,
            bool? longTerm,
            ulong callbackId
        )
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

        public ulong SubscribeCountAccessLog(
            bool? service,
            bool? method,
            bool? userId,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.AccessLogCount[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.AccessLogCount>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "AccessLogCount"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeCountAccessLogWithInitialCallAsync(
            bool? service,
            bool? method,
            bool? userId,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.AccessLogCount[]> callback
        )
        {
            var items = await CountAccessLogAsync(
                service,
                method,
                userId,
                begin,
                end,
                longTerm
            ).ToArrayAsync();
            var callbackId = SubscribeCountAccessLog(
                service,
                method,
                userId,
                begin,
                end,
                longTerm,
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeCountAccessLog(
            bool? service,
            bool? method,
            bool? userId,
            long? begin,
            long? end,
            bool? longTerm,
            ulong callbackId
        )
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

        public ulong SubscribeExecuteStampSheetLog(
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.ExecuteStampSheetLog[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.ExecuteStampSheetLog>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "ExecuteStampSheetLog"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeExecuteStampSheetLogWithInitialCallAsync(
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.ExecuteStampSheetLog[]> callback
        )
        {
            var items = await ExecuteStampSheetLogAsync(
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            ).ToArrayAsync();
            var callbackId = SubscribeExecuteStampSheetLog(
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeExecuteStampSheetLog(
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm,
            ulong callbackId
        )
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

        public ulong SubscribeCountExecuteStampSheetLog(
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "ExecuteStampSheetLogCount"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeCountExecuteStampSheetLogWithInitialCallAsync(
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount[]> callback
        )
        {
            var items = await CountExecuteStampSheetLogAsync(
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            ).ToArrayAsync();
            var callbackId = SubscribeCountExecuteStampSheetLog(
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeCountExecuteStampSheetLog(
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm,
            ulong callbackId
        )
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

        public ulong SubscribeExecuteStampTaskLog(
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.ExecuteStampTaskLog[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.ExecuteStampTaskLog>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "ExecuteStampTaskLog"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeExecuteStampTaskLogWithInitialCallAsync(
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.ExecuteStampTaskLog[]> callback
        )
        {
            var items = await ExecuteStampTaskLogAsync(
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            ).ToArrayAsync();
            var callbackId = SubscribeExecuteStampTaskLog(
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeExecuteStampTaskLog(
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm,
            ulong callbackId
        )
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

        public ulong SubscribeCountExecuteStampTaskLog(
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "ExecuteStampTaskLogCount"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeCountExecuteStampTaskLogWithInitialCallAsync(
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount[]> callback
        )
        {
            var items = await CountExecuteStampTaskLogAsync(
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            ).ToArrayAsync();
            var callbackId = SubscribeCountExecuteStampTaskLog(
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeCountExecuteStampTaskLog(
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm,
            ulong callbackId
        )
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

        public ulong SubscribeIssueStampSheetLog(
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.IssueStampSheetLog[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.IssueStampSheetLog>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "IssueStampSheetLog"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeIssueStampSheetLogWithInitialCallAsync(
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.IssueStampSheetLog[]> callback
        )
        {
            var items = await IssueStampSheetLogAsync(
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            ).ToArrayAsync();
            var callbackId = SubscribeIssueStampSheetLog(
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeIssueStampSheetLog(
            string service,
            string method,
            string userId,
            string action,
            long? begin,
            long? end,
            bool? longTerm,
            ulong callbackId
        )
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

        public ulong SubscribeCountIssueStampSheetLog(
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.IssueStampSheetLogCount[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.IssueStampSheetLogCount>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "IssueStampSheetLogCount"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeCountIssueStampSheetLogWithInitialCallAsync(
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm,
            Action<Gs2.Gs2Log.Model.IssueStampSheetLogCount[]> callback
        )
        {
            var items = await CountIssueStampSheetLogAsync(
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            ).ToArrayAsync();
            var callbackId = SubscribeCountIssueStampSheetLog(
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeCountIssueStampSheetLog(
            bool? service,
            bool? method,
            bool? userId,
            bool? action,
            long? begin,
            long? end,
            bool? longTerm,
            ulong callbackId
        )
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

        public ulong SubscribeInsights(
            Action<Gs2.Gs2Log.Model.Insight[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.Insight>(
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    "Insight"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeInsightsWithInitialCallAsync(
            Action<Gs2.Gs2Log.Model.Insight[]> callback
        )
        {
            var items = await InsightsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeInsights(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeInsights(
            ulong callbackId
        )
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

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "namespace")
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.Status = domain.Status = result?.Status;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Log.Domain.Model.NamespaceDomain> GetStatusAsync(
            #else
        public async Task<Gs2.Gs2Log.Domain.Model.NamespaceDomain> GetStatusAsync(
            #endif
            GetNamespaceStatusRequest request
        ) {
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

                if (e.Errors.Length == 0 || e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "namespace")
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

                var requestModel = request;
                var resultModel = result;
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
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Log.Model.Namespace> GetAsync(
            #else
        private async Task<Gs2.Gs2Log.Model.Namespace> GetAsync(
            #endif
            GetNamespaceRequest request
        ) {
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

                if (e.Errors.Length == 0 || e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
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

                var requestModel = request;
                var resultModel = result;
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
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Log.Domain.Model.NamespaceDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Log.Domain.Model.NamespaceDomain> UpdateAsync(
            #endif
            UpdateNamespaceRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            UpdateNamespaceResult result = null;
                result = await this._client.UpdateNamespaceAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
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

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "namespace")
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

                var requestModel = request;
                var resultModel = result;
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
                        _gs2.Cache.Delete<Gs2.Gs2Log.Model.Namespace>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Log.Domain.Model.NamespaceDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Log.Domain.Model.NamespaceDomain> DeleteAsync(
            #endif
            DeleteNamespaceRequest request
        ) {
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

                if (e.Errors.Length == 0 || e.Errors[0].Component != "namespace")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Delete<Gs2.Gs2Log.Model.Namespace>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Log.Domain.Model.NamespaceDomain> PutLogAsync(
            #else
        public async Task<Gs2.Gs2Log.Domain.Model.NamespaceDomain> PutLogAsync(
            #endif
            PutLogRequest request
        ) {
            PutLogResult result = null;
                result = await this._client.PutLogAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Insight"
                        );
                        var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Log.Domain.Model.InsightDomain> CreateInsightAsync(
            #else
        public async Task<Gs2.Gs2Log.Domain.Model.InsightDomain> CreateInsightAsync(
            #endif
            CreateInsightRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName);
            CreateInsightResult result = null;
                result = await this._client.CreateInsightAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Insight"
                    );
                    var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
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

                            if (e.errors.Length == 0 || e.errors[0].component != "namespace")
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
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Log.Model.Namespace> ModelAsync()
            #else
        public async Task<Gs2.Gs2Log.Model.Namespace> ModelAsync()
            #endif
        {
            var parentKey = string.Join(
                ":",
                "log",
                "Namespace"
            );
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Log.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName?.ToString()
                )).LockAsync())
            {
        # endif
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

                        if (e.errors.Length == 0 || e.errors[0].component != "namespace")
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
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
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


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Log.Model.Namespace>(
                _parentKey,
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Log.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheKey(
                    this.NamespaceName.ToString()
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    ModelAsync().Forget();
            #else
                    ModelAsync();
            #endif
        #endif
                }
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

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Log.Model.Namespace> callback)
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Log.Model.Namespace> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Log.Model.Namespace> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

    }
}
