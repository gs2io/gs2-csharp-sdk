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
 *
 * deny overwrite
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
using Gs2.Gs2Log.Model;
using Gs2.Gs2Log.Model.Cache;
using Gs2.Gs2Log.Request;
using Gs2.Gs2Log.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
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
        public string NamespaceName { get; } = null!;
        public string Status { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;
        public long? TotalCount { get; set; } = null!;
        public long? ScanSize { get; set; } = null!;

        public NamespaceDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LogRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Log.Model.AccessLog> AccessLog(
            string service = null,
            string method = null,
            string userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryAccessLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                begin,
                end,
                longTerm,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.AccessLog> AccessLogAsync(
            #else
        public QueryAccessLogIterator AccessLogAsync(
            #endif
            string service = null,
            string method = null,
            string userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryAccessLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                begin,
                end,
                longTerm,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeAccessLog(
            Action<Gs2.Gs2Log.Model.AccessLog[]> callback,
            string service = null,
            string method = null,
            string userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.AccessLog>(
                (null as Gs2.Gs2Log.Model.AccessLog).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeAccessLogWithInitialCallAsync(
            Action<Gs2.Gs2Log.Model.AccessLog[]> callback,
            string service = null,
            string method = null,
            string userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
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
                callback,
                service,
                method,
                userId,
                begin,
                end,
                longTerm
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeAccessLog(
            ulong callbackId,
            string service = null,
            string method = null,
            string userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.AccessLog>(
                (null as Gs2.Gs2Log.Model.AccessLog).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Log.Model.AccessLogCount> CountAccessLog(
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new CountAccessLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                begin,
                end,
                longTerm,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.AccessLogCount> CountAccessLogAsync(
            #else
        public CountAccessLogIterator CountAccessLogAsync(
            #endif
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new CountAccessLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                begin,
                end,
                longTerm,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeCountAccessLog(
            Action<Gs2.Gs2Log.Model.AccessLogCount[]> callback,
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.AccessLogCount>(
                (null as Gs2.Gs2Log.Model.AccessLogCount).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeCountAccessLogWithInitialCallAsync(
            Action<Gs2.Gs2Log.Model.AccessLogCount[]> callback,
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
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
                callback,
                service,
                method,
                userId,
                begin,
                end,
                longTerm
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeCountAccessLog(
            ulong callbackId,
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.AccessLogCount>(
                (null as Gs2.Gs2Log.Model.AccessLogCount).CacheParentKey(
                    this.NamespaceName
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
        public Gs2Iterator<Gs2.Gs2Log.Model.ExecuteStampSheetLog> ExecuteStampSheetLog(
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryExecuteStampSheetLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.ExecuteStampSheetLog> ExecuteStampSheetLogAsync(
            #else
        public QueryExecuteStampSheetLogIterator ExecuteStampSheetLogAsync(
            #endif
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryExecuteStampSheetLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeExecuteStampSheetLog(
            Action<Gs2.Gs2Log.Model.ExecuteStampSheetLog[]> callback,
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.ExecuteStampSheetLog>(
                (null as Gs2.Gs2Log.Model.ExecuteStampSheetLog).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeExecuteStampSheetLogWithInitialCallAsync(
            Action<Gs2.Gs2Log.Model.ExecuteStampSheetLog[]> callback,
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
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
                callback,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeExecuteStampSheetLog(
            ulong callbackId,
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.ExecuteStampSheetLog>(
                (null as Gs2.Gs2Log.Model.ExecuteStampSheetLog).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount> CountExecuteStampSheetLog(
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new CountExecuteStampSheetLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount> CountExecuteStampSheetLogAsync(
            #else
        public CountExecuteStampSheetLogIterator CountExecuteStampSheetLogAsync(
            #endif
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new CountExecuteStampSheetLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeCountExecuteStampSheetLog(
            Action<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount[]> callback,
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount>(
                (null as Gs2.Gs2Log.Model.ExecuteStampSheetLogCount).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeCountExecuteStampSheetLogWithInitialCallAsync(
            Action<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount[]> callback,
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
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
                callback,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeCountExecuteStampSheetLog(
            ulong callbackId,
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.ExecuteStampSheetLogCount>(
                (null as Gs2.Gs2Log.Model.ExecuteStampSheetLogCount).CacheParentKey(
                    this.NamespaceName
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
        public Gs2Iterator<Gs2.Gs2Log.Model.ExecuteStampTaskLog> ExecuteStampTaskLog(
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryExecuteStampTaskLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.ExecuteStampTaskLog> ExecuteStampTaskLogAsync(
            #else
        public QueryExecuteStampTaskLogIterator ExecuteStampTaskLogAsync(
            #endif
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryExecuteStampTaskLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeExecuteStampTaskLog(
            Action<Gs2.Gs2Log.Model.ExecuteStampTaskLog[]> callback,
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.ExecuteStampTaskLog>(
                (null as Gs2.Gs2Log.Model.ExecuteStampTaskLog).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeExecuteStampTaskLogWithInitialCallAsync(
            Action<Gs2.Gs2Log.Model.ExecuteStampTaskLog[]> callback,
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
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
                callback,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeExecuteStampTaskLog(
            ulong callbackId,
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.ExecuteStampTaskLog>(
                (null as Gs2.Gs2Log.Model.ExecuteStampTaskLog).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount> CountExecuteStampTaskLog(
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new CountExecuteStampTaskLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount> CountExecuteStampTaskLogAsync(
            #else
        public CountExecuteStampTaskLogIterator CountExecuteStampTaskLogAsync(
            #endif
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new CountExecuteStampTaskLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeCountExecuteStampTaskLog(
            Action<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount[]> callback,
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount>(
                (null as Gs2.Gs2Log.Model.ExecuteStampTaskLogCount).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeCountExecuteStampTaskLogWithInitialCallAsync(
            Action<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount[]> callback,
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
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
                callback,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeCountExecuteStampTaskLog(
            ulong callbackId,
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.ExecuteStampTaskLogCount>(
                (null as Gs2.Gs2Log.Model.ExecuteStampTaskLogCount).CacheParentKey(
                    this.NamespaceName
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
        public Gs2Iterator<Gs2.Gs2Log.Model.IssueStampSheetLog> IssueStampSheetLog(
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryIssueStampSheetLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.IssueStampSheetLog> IssueStampSheetLogAsync(
            #else
        public QueryIssueStampSheetLogIterator IssueStampSheetLogAsync(
            #endif
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryIssueStampSheetLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeIssueStampSheetLog(
            Action<Gs2.Gs2Log.Model.IssueStampSheetLog[]> callback,
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.IssueStampSheetLog>(
                (null as Gs2.Gs2Log.Model.IssueStampSheetLog).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeIssueStampSheetLogWithInitialCallAsync(
            Action<Gs2.Gs2Log.Model.IssueStampSheetLog[]> callback,
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
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
                callback,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeIssueStampSheetLog(
            ulong callbackId,
            string service = null,
            string method = null,
            string userId = null,
            string action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.IssueStampSheetLog>(
                (null as Gs2.Gs2Log.Model.IssueStampSheetLog).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Log.Model.IssueStampSheetLogCount> CountIssueStampSheetLog(
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new CountIssueStampSheetLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.IssueStampSheetLogCount> CountIssueStampSheetLogAsync(
            #else
        public CountIssueStampSheetLogIterator CountIssueStampSheetLogAsync(
            #endif
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new CountIssueStampSheetLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeCountIssueStampSheetLog(
            Action<Gs2.Gs2Log.Model.IssueStampSheetLogCount[]> callback,
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.IssueStampSheetLogCount>(
                (null as Gs2.Gs2Log.Model.IssueStampSheetLogCount).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeCountIssueStampSheetLogWithInitialCallAsync(
            Action<Gs2.Gs2Log.Model.IssueStampSheetLogCount[]> callback,
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
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
                callback,
                service,
                method,
                userId,
                action,
                begin,
                end,
                longTerm
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeCountIssueStampSheetLog(
            ulong callbackId,
            bool? service = null,
            bool? method = null,
            bool? userId = null,
            bool? action = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.IssueStampSheetLogCount>(
                (null as Gs2.Gs2Log.Model.IssueStampSheetLogCount).CacheParentKey(
                    this.NamespaceName
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
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Log.Model.Insight> Insights(
        )
        {
            return new DescribeInsightsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.Insight> InsightsAsync(
            #else
        public DescribeInsightsIterator InsightsAsync(
            #endif
        )
        {
            return new DescribeInsightsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeInsights(
            Action<Gs2.Gs2Log.Model.Insight[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.Insight>(
                (null as Gs2.Gs2Log.Model.Insight).CacheParentKey(
                    this.NamespaceName
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
                (null as Gs2.Gs2Log.Model.Insight).CacheParentKey(
                    this.NamespaceName
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
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Log.Model.AccessLogWithTelemetry> AccessLogWithTelemetry(
            string? userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryAccessLogWithTelemetryIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                userId,
                begin,
                end,
                longTerm,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.AccessLogWithTelemetry> AccessLogWithTelemetryAsync(
            #else
        public QueryAccessLogWithTelemetryIterator AccessLogWithTelemetryAsync(
            #endif
            string? userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryAccessLogWithTelemetryIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                userId,
                begin,
                end,
                longTerm,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeAccessLogWithTelemetry(
            Action<Gs2.Gs2Log.Model.AccessLogWithTelemetry[]> callback,
            string userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.AccessLogWithTelemetry>(
                (null as Gs2.Gs2Log.Model.AccessLogWithTelemetry).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeAccessLogWithTelemetryWithInitialCallAsync(
            Action<Gs2.Gs2Log.Model.AccessLogWithTelemetry[]> callback,
            string userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            var items = await AccessLogWithTelemetryAsync(
                userId,
                begin,
                end,
                longTerm
            ).ToArrayAsync();
            var callbackId = SubscribeAccessLogWithTelemetry(
                callback,
                userId,
                begin,
                end,
                longTerm
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeAccessLogWithTelemetry(
            ulong callbackId,
            string userId = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.AccessLogWithTelemetry>(
                (null as Gs2.Gs2Log.Model.AccessLogWithTelemetry).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Log.Domain.Model.AccessLogWithTelemetryDomain AccessLogWithTelemetry(
        ) {
            return new Gs2.Gs2Log.Domain.Model.AccessLogWithTelemetryDomain(
                this._gs2,
                this.NamespaceName
            );
        }

        public Gs2.Gs2Log.Domain.Model.UserDomain User(
            string userId
        ) {
            return new Gs2.Gs2Log.Domain.Model.UserDomain(
                this._gs2,
                this.NamespaceName,
                userId
            );
        }

        public UserAccessTokenDomain AccessToken(
            AccessToken accessToken
        ) {
            return new UserAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                accessToken
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
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetNamespaceStatusFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetNamespaceStatusAsync(request)
            );
            var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Log.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Model.Namespace> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.GetNamespaceFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.GetNamespaceAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.UpdateNamespaceFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.UpdateNamespaceAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.NamespaceDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteNamespaceFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
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
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    () => this._client.DeleteNamespaceAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Domain.Model.InsightDomain> CreateInsightFuture(
            CreateInsightRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.InsightDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CreateInsightFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Log.Domain.Model.InsightDomain(
                    this._gs2,
                    this.NamespaceName,
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
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateInsightAsync(request)
            );
            var domain = new Gs2.Gs2Log.Domain.Model.InsightDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }
        #endif

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Model.Namespace> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Model.Namespace> self)
            {
                var (value, find) = (null as Gs2.Gs2Log.Model.Namespace).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Log.Model.Namespace).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    () => this.GetFuture(
                        new GetNamespaceRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
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
            var (value, find) = (null as Gs2.Gs2Log.Model.Namespace).GetCache(
                this._gs2.Cache,
                this.NamespaceName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Log.Model.Namespace).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                () => this.GetAsync(
                    new GetNamespaceRequest()
                )
            );
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
            (null as Gs2.Gs2Log.Model.Namespace).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Log.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Log.Model.Namespace).CacheParentKey(
                ),
                (null as Gs2.Gs2Log.Model.Namespace).CacheKey(
                    this.NamespaceName
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
            #else
                    async Task Impl() {
            #endif
                        try {
                            await ModelAsync();
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
            #if GS2_ENABLE_UNITASK
                    Impl().Forget();
            #else
                    Impl();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Log.Model.Namespace>(
                (null as Gs2.Gs2Log.Model.Namespace).CacheParentKey(
                ),
                (null as Gs2.Gs2Log.Model.Namespace).CacheKey(
                    this.NamespaceName
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

        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Log.Model.InGameLog> InGameLog(
            string userId = null,
            InGameLogTag[] tags = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryInGameLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                userId,
                tags,
                begin,
                end,
                longTerm,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.InGameLog> InGameLogAsync(
            #else
        public QueryInGameLogIterator InGameLogAsync(
            #endif
            string userId = null,
            InGameLogTag[] tags = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null,
            string timeOffsetToken = null
        )
        {
            return new QueryInGameLogIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                userId,
                tags,
                begin,
                end,
                longTerm,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeInGameLog(
            Action<Gs2.Gs2Log.Model.InGameLog[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Log.Model.InGameLog>(
                (null as Gs2.Gs2Log.Model.InGameLog).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeInGameLogWithInitialCallAsync(
            Action<Gs2.Gs2Log.Model.InGameLog[]> callback,
            string userId = null,
            InGameLogTag[] tags = null,
            long? begin = null,
            long? end = null,
            bool? longTerm = null
        )
        {
            var items = await InGameLogAsync(
                userId,
                tags,
                begin,
                end,
                longTerm
            ).ToArrayAsync();
            var callbackId = SubscribeInGameLog(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeInGameLog(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Log.Model.InGameLog>(
                (null as Gs2.Gs2Log.Model.InGameLog).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }
    }
}
