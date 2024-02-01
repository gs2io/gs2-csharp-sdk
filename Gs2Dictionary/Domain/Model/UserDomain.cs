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
using Gs2.Gs2Dictionary.Domain.Iterator;
using Gs2.Gs2Dictionary.Model.Cache;
using Gs2.Gs2Dictionary.Request;
using Gs2.Gs2Dictionary.Result;
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

namespace Gs2.Gs2Dictionary.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DictionaryRestClient _client;
        public string NamespaceName { get; }
        public string UserId { get; }
        public string NextPageToken { get; set; }

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DictionaryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Dictionary.Model.Entry> Entries(
        )
        {
            return new DescribeEntriesByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Dictionary.Model.Entry> EntriesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Dictionary.Model.Entry> Entries(
            #endif
        #else
        public DescribeEntriesByUserIdIterator EntriesAsync(
        #endif
        )
        {
            return new DescribeEntriesByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.UserId
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

        public ulong SubscribeEntries(
            Action<Gs2.Gs2Dictionary.Model.Entry[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Dictionary.Model.Entry>(
                (null as Gs2.Gs2Dictionary.Model.Entry).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeEntriesWithInitialCallAsync(
            Action<Gs2.Gs2Dictionary.Model.Entry[]> callback
        )
        {
            var items = await EntriesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeEntries(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeEntries(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Dictionary.Model.Entry>(
                (null as Gs2.Gs2Dictionary.Model.Entry).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Dictionary.Domain.Model.EntryDomain Entry(
            string entryName
        ) {
            return new Gs2.Gs2Dictionary.Domain.Model.EntryDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                entryName
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]> AddEntriesFuture(
            AddEntriesByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]> self)
            {
                request = request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.AddEntriesByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = result?.Items?.Select(v => new Gs2.Gs2Dictionary.Domain.Model.EntryDomain(
                    this._gs2,
                    this.NamespaceName,
                    v?.UserId,
                    v?.Name
                )).ToArray() ?? Array.Empty<Gs2.Gs2Dictionary.Domain.Model.EntryDomain>();
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]> AddEntriesAsync(
            #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]> AddEntriesAsync(
            #endif
            AddEntriesByUserIdRequest request
        ) {
            request = request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.AddEntriesByUserIdAsync(request)
            );
            var domain = result?.Items?.Select(v => new Gs2.Gs2Dictionary.Domain.Model.EntryDomain(
                this._gs2,
                this.NamespaceName,
                v?.UserId,
                v?.Name
            )).ToArray() ?? Array.Empty<Gs2.Gs2Dictionary.Domain.Model.EntryDomain>();
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.UserDomain> ResetFuture(
            ResetByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.UserDomain> self)
            {
                request = request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.ResetByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Domain.Model.UserDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.UserDomain> ResetAsync(
            #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.UserDomain> ResetAsync(
            #endif
            ResetByUserIdRequest request
        ) {
            request = request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.ResetByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.UserDomain> VerifyEntryFuture(
            VerifyEntryByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.UserDomain> self)
            {
                request = request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.VerifyEntryByUserIdFuture(request)
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
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Domain.Model.UserDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.UserDomain> VerifyEntryAsync(
            #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.UserDomain> VerifyEntryAsync(
            #endif
            VerifyEntryByUserIdRequest request
        ) {
            request = request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.VerifyEntryByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]> DeleteEntriesFuture(
            DeleteEntriesByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]> self)
            {
                request = request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteEntriesByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = result?.Items?.Select(v => new Gs2.Gs2Dictionary.Domain.Model.EntryDomain(
                    this._gs2,
                    this.NamespaceName,
                    v?.UserId,
                    v?.Name
                )).ToArray() ?? Array.Empty<Gs2.Gs2Dictionary.Domain.Model.EntryDomain>();
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]> DeleteEntriesAsync(
            #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]> DeleteEntriesAsync(
            #endif
            DeleteEntriesByUserIdRequest request
        ) {
            Gs2.Gs2Dictionary.Model.Entry[] items = null;
            try {
                request = request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteEntriesByUserIdAsync(request)
                );
                items = result?.Items;
            }
            catch (NotFoundException e) {
                
            }
            var domain = items?.Select(v => new Gs2.Gs2Dictionary.Domain.Model.EntryDomain(
                this._gs2,
                this.NamespaceName,
                v?.UserId,
                v?.Name
            )).ToArray() ?? Array.Empty<Gs2.Gs2Dictionary.Domain.Model.EntryDomain>();
            return domain;
        }
        #endif

    }

    public partial class UserDomain {

    }
}
