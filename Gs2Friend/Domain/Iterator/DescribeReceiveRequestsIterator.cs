
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
// ReSharper disable InconsistentNaming

#pragma warning disable CS0414 // Field is assigned but its value is never used
#pragma warning disable 1998

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Friend.Model; /* diff +++ */
using Gs2.Util.LitJson;
using Gs2.Gs2Friend.Model.Cache;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Events;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Friend.Domain.Iterator
{

    public class DescribeReceiveRequestsIterator :
    #if UNITY_2017_1_OR_NEWER
/* diff --- start
        Gs2Iterator<Gs2.Gs2Friend.Model.FriendRequest>,
 diff --- end */
        Gs2Iterator<Gs2.Gs2Friend.Model.ReceiveFriendRequest>, /* diff +++ */
    #endif
    #if GS2_ENABLE_UNITASK
/* diff --- start
        IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.FriendRequest>
 diff --- end */
        IUniTaskAsyncEnumerable<Gs2.Gs2Friend.Model.ReceiveFriendRequest> /* diff +++ */
    #else
/* diff --- start
        IAsyncEnumerable<Gs2.Gs2Friend.Model.FriendRequest>
 diff --- end */
        IAsyncEnumerable<Gs2.Gs2Friend.Model.ReceiveFriendRequest> /* diff +++ */
    #endif
    {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FriendRestClient _client;
        public string NamespaceName { get; }
        public AccessToken AccessToken { get; }
        public string UserId => AccessToken?.UserId;
/* diff --- start
        public bool? WithProfile { get; }
 diff --- end */
        private string _pageToken;
        private bool _isCacheChecked;
        private bool _last;
/* diff --- start
        private Gs2.Gs2Friend.Model.FriendRequest[] _result;
 diff --- end */
        private Gs2.Gs2Friend.Model.ReceiveFriendRequest[] _result; /* diff +++ */

        public static int? fetchSize;

        public DescribeReceiveRequestsIterator(
            Gs2.Core.Domain.Gs2 gs2,
            Gs2FriendRestClient client,
            string namespaceName,
/* diff --- start
            AccessToken accessToken,
            bool? withProfile = null
 diff --- end */
            AccessToken accessToken /* diff +++ */
        ) {
            this._gs2 = gs2;
            this._client = client;
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
/* diff --- start
            this.WithProfile = withProfile;
 diff --- end */
            this._pageToken = null;
            this._last = false;
/* diff --- start
            this._result = new Gs2.Gs2Friend.Model.FriendRequest[]{};
 diff --- end */
            this._result = new Gs2.Gs2Friend.Model.ReceiveFriendRequest[]{}; /* diff +++ */
        }

        #if GS2_ENABLE_UNITASK
        private async UniTask _load() {
        #else
        private async Task _load() {
        #endif
            var isCacheChecked = this._isCacheChecked;
            this._isCacheChecked = true;
            if (!isCacheChecked && this._gs2.Cache.TryGetList
/* diff --- start
                    <Gs2.Gs2Friend.Model.FriendRequest>
 diff --- end */
                    <Gs2.Gs2Friend.Model.ReceiveFriendRequest> /* diff +++ */
            (
                    (null as Gs2.Gs2Friend.Model.ReceiveFriendRequest).CacheParentKey(
                        NamespaceName,
                        AccessToken?.UserId,
/* diff --- start
                        this.AccessToken?.TimeOffset
 diff --- end */
                        AccessToken?.TimeOffset /* diff +++ */
                    ),
                    out var list
            )) {
                this._result = list
                    .ToArray();
                this._pageToken = null;
                this._last = true;
            } else {

                var request = new Gs2.Gs2Friend.Request.DescribeReceiveRequestsRequest()
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken != null ? this.AccessToken.Token : null)
/* diff --- start
                    .WithWithProfile(this.WithProfile)
 diff --- end */
                    .WithPageToken(this._pageToken)
                    .WithLimit(fetchSize);
                var r = await this._client.DescribeReceiveRequestsAsync(
                    request
                );
/* diff --- start
                this._result = r.Items
 diff --- end */
/* diff +++ start */
                this._result = r.Items.Select(v => new ReceiveFriendRequest {
                        UserId = v.UserId,
                        TargetUserId = v.TargetUserId,
                    })
/* diff +++ end */
                    .ToArray();
                this._pageToken = r.NextPageToken;
                this._last = this._pageToken == null;
                r.PutCache(
                    this._gs2.Cache,
                    UserId,
/* diff --- start
                    this.AccessToken?.TimeOffset,
 diff --- end */
                    AccessToken?.TimeOffset, /* diff +++ */
                    request
                );

                if (this._last) {
/* diff --- start
                    this._gs2.Cache.SetListCached<Gs2.Gs2Friend.Model.FriendRequest>(
 diff --- end */
                    this._gs2.Cache.SetListCached<Gs2.Gs2Friend.Model.ReceiveFriendRequest>( /* diff +++ */
                        (null as Gs2.Gs2Friend.Model.ReceiveFriendRequest).CacheParentKey(
                            NamespaceName,
                            AccessToken?.UserId,
/* diff --- start
                            this.AccessToken?.TimeOffset
 diff --- end */
                            AccessToken?.TimeOffset /* diff +++ */
                        )
                    );
                }
            }
        }

        private bool _hasNext()
        {
            return this._result.Length != 0 || !this._last;
        }

        #if UNITY_2017_1_OR_NEWER
        public override bool HasNext()
        {
            if (Error != null) return false;
            return _hasNext();
        }

        protected override System.Collections.IEnumerator Next(
/* diff --- start
            Action<AsyncResult<Gs2.Gs2Friend.Model.FriendRequest>> callback
 diff --- end */
            Action<AsyncResult<Gs2.Gs2Friend.Model.ReceiveFriendRequest>> callback /* diff +++ */
        )
        {
            if (this._result.Length == 0 && !this._last) {
                var future = this._load().ToGs2Future();
                yield return future;
                if (future.Error != null)
                {
                    Current = null;
                    Error = future.Error;
/* diff --- start
                    callback.Invoke(new AsyncResult<Gs2.Gs2Friend.Model.FriendRequest>(
 diff --- end */
                    callback.Invoke(new AsyncResult<Gs2.Gs2Friend.Model.ReceiveFriendRequest>( /* diff +++ */
                        Current,
                        Error
                    ));
                    yield break;
                }
            }
            if (this._result.Length == 0) {
                Current = null;
/* diff --- start
                callback.Invoke(new AsyncResult<Gs2.Gs2Friend.Model.FriendRequest>(
 diff --- end */
                callback.Invoke(new AsyncResult<Gs2.Gs2Friend.Model.ReceiveFriendRequest>( /* diff +++ */
                    Current,
                    Error
                ));
                yield break;
            }
            var ret = this._result[0];
            this._result = this._result.ToList().GetRange(1, this._result.Length - 1).ToArray();
            if (this._result.Length == 0 && !this._last) {
                var future = this._load().ToGs2Future();
                yield return future;
                if (future.Error != null)
                {
                    Current = null;
                    Error = future.Error;
/* diff --- start
                    callback.Invoke(new AsyncResult<Gs2.Gs2Friend.Model.FriendRequest>(
 diff --- end */
                    callback.Invoke(new AsyncResult<Gs2.Gs2Friend.Model.ReceiveFriendRequest>( /* diff +++ */
                        Current,
                        Error
                    ));
                    yield break;
                }
            }
            Current = ret;
/* diff --- start
            callback.Invoke(new AsyncResult<Gs2.Gs2Friend.Model.FriendRequest>(
 diff --- end */
            callback.Invoke(new AsyncResult<Gs2.Gs2Friend.Model.ReceiveFriendRequest>( /* diff +++ */
                Current,
                Error
            ));
        }
        #endif

        #if GS2_ENABLE_UNITASK
/* diff --- start
        public IUniTaskAsyncEnumerator<Gs2.Gs2Friend.Model.FriendRequest> GetAsyncEnumerator(
 diff --- end */
        public IUniTaskAsyncEnumerator<Gs2.Gs2Friend.Model.ReceiveFriendRequest> GetAsyncEnumerator( /* diff +++ */
            CancellationToken cancellationToken = new CancellationToken()
/* diff --- start
        ) => UniTaskAsyncEnumerable.Create<Gs2.Gs2Friend.Model.FriendRequest>(async (writer, token) =>
 diff --- end */
        ) => UniTaskAsyncEnumerable.Create<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(async (writer, token) => /* diff +++ */
        #else
/* diff --- start
        public async IAsyncEnumerator<Gs2.Gs2Friend.Model.FriendRequest> GetAsyncEnumerator(
 diff --- end */
        public async IAsyncEnumerator<Gs2.Gs2Friend.Model.ReceiveFriendRequest> GetAsyncEnumerator( /* diff +++ */
            CancellationToken cancellationToken = new CancellationToken()
        )
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Friend.Model.FriendRequest>(
                    (null as Gs2.Gs2Friend.Model.FriendRequest).CacheParentKey(
                        NamespaceName,
                        AccessToken?.UserId,
                        this.AccessToken?.TimeOffset
                   ),
                   "ListFriendRequest"
               ).LockAsync()) {
 diff --- end */
/* diff +++ start */
                using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Friend.Model.ReceiveFriendRequest>(
                        (null as Gs2.Gs2Friend.Model.ReceiveFriendRequest).CacheParentKey(
                            NamespaceName,
                            AccessToken?.UserId,
                            AccessToken?.TimeOffset
                       ),
                       "ListReceiveFriendRequest"
                   ).LockAsync()) {
/* diff +++ end */
                while(this._hasNext()) {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (this._result.Length == 0 && !this._last) {
                        await this._load();
                    }
                    if (this._result.Length == 0) {
                        break;
                    }
                    var ret = this._result[0];
                    this._result = this._result.ToList().GetRange(1, this._result.Length - 1).ToArray();
                    if (this._result.Length == 0 && !this._last) {
                        await this._load();
                    }
            #if GS2_ENABLE_UNITASK
                    await writer.YieldAsync(ret);
            #else
                    yield return ret;
            #endif
                }
            }
        }
        #if GS2_ENABLE_UNITASK
        ).GetAsyncEnumerator();
        #endif
    }
}
