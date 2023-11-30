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
using Gs2.Gs2Dictionary.Domain.Iterator;
using Gs2.Gs2Dictionary.Request;
using Gs2.Gs2Dictionary.Result;
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

namespace Gs2.Gs2Dictionary.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DictionaryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DictionaryRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2Dictionary.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
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

        public ulong SubscribeEntries(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Dictionary.Model.Entry>(
                Gs2.Gs2Dictionary.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Entry"
                ),
                callback
            );
        }

        public void UnsubscribeEntries(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Dictionary.Model.Entry>(
                Gs2.Gs2Dictionary.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Entry"
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

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "dictionary",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string userId
        )
        {
            return string.Join(
                ":",
                userId ?? "null"
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.AddEntriesByUserIdFuture(
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
                        var parentKey = Gs2.Gs2Dictionary.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Entry"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Dictionary.Domain.Model.EntryDomain.CreateCacheKey(
                                item.Name.ToString()
                            );
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                }
                var domain = new Gs2.Gs2Dictionary.Domain.Model.EntryDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Dictionary.Domain.Model.EntryDomain(
                        this._gs2,
                        request.NamespaceName,
                        result.Items[i]?.UserId,
                        result.Items[i]?.Name
                    );
                    var parentKey = Gs2.Gs2Dictionary.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Entry"
                    );
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            AddEntriesByUserIdResult result = null;
                result = await this._client.AddEntriesByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Dictionary.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Entry"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Dictionary.Domain.Model.EntryDomain.CreateCacheKey(
                            item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
            }
                var domain = new Gs2.Gs2Dictionary.Domain.Model.EntryDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Dictionary.Domain.Model.EntryDomain(
                        this._gs2,
                        request.NamespaceName,
                        result.Items[i]?.UserId,
                        result.Items[i]?.Name
                    );
                    var parentKey = Gs2.Gs2Dictionary.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Entry"
                    );
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to AddEntriesFuture.")]
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]> AddEntries(
            AddEntriesByUserIdRequest request
        ) {
            return AddEntriesFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.UserDomain> ResetFuture(
            ResetByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.UserDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.ResetByUserIdFuture(
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
                    
                    var parentKey = CreateCacheParentKey(
                        requestModel.NamespaceName.ToString(),
                        requestModel.UserId.ToString(),
                        "Entry"
                    );
                    foreach (Gs2.Gs2Dictionary.Model.Entry item in _gs2.Cache.List<Gs2.Gs2Dictionary.Model.Entry>(
                        parentKey
                    )) {
                        _gs2.Cache.Delete<Gs2.Gs2Dictionary.Model.Entry>(
                                parentKey,
                                Gs2.Gs2Dictionary.Domain.Model.EntryDomain.CreateCacheKey(
                                    item?.Name?.ToString()
                                )
                            );
                    }
                }
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            ResetByUserIdResult result = null;
                result = await this._client.ResetByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                var parentKey = CreateCacheParentKey(
                    requestModel.NamespaceName.ToString(),
                    requestModel.UserId.ToString(),
                    "Entry"
                );
                foreach (Gs2.Gs2Dictionary.Model.Entry item in _gs2.Cache.List<Gs2.Gs2Dictionary.Model.Entry>(
                    parentKey
                )) {
                    _gs2.Cache.Delete<Gs2.Gs2Dictionary.Model.Entry>(
                            parentKey,
                            Gs2.Gs2Dictionary.Domain.Model.EntryDomain.CreateCacheKey(
                                item?.Name?.ToString()
                            )
                        );
                }
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to ResetFuture.")]
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.UserDomain> Reset(
            ResetByUserIdRequest request
        ) {
            return ResetFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.UserDomain> VerifyEntryFuture(
            VerifyEntryByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.UserDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.VerifyEntryByUserIdFuture(
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            VerifyEntryByUserIdResult result = null;
                result = await this._client.VerifyEntryByUserIdAsync(
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
        [Obsolete("The name has been changed to VerifyEntryFuture.")]
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.UserDomain> VerifyEntry(
            VerifyEntryByUserIdRequest request
        ) {
            return VerifyEntryFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]> DeleteEntriesFuture(
            DeleteEntriesByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = this._client.DeleteEntriesByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
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
                        var parentKey = Gs2.Gs2Dictionary.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Entry"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Dictionary.Domain.Model.EntryDomain.CreateCacheKey(
                                item.Name.ToString()
                            );
                            _gs2.Cache.Delete<Gs2.Gs2Dictionary.Model.Entry>(
                                parentKey,
                                key
                            );
                        }
                    }
                }
                var domain = new Gs2.Gs2Dictionary.Domain.Model.EntryDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Dictionary.Domain.Model.EntryDomain(
                        this._gs2,
                        request.NamespaceName,
                        result.Items[i]?.UserId,
                        result.Items[i]?.Name
                    );
                }
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            DeleteEntriesByUserIdResult result = null;
            try {
                result = await this._client.DeleteEntriesByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
            }

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Dictionary.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Entry"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Dictionary.Domain.Model.EntryDomain.CreateCacheKey(
                            item.Name.ToString()
                        );
                        _gs2.Cache.Delete<Gs2.Gs2Dictionary.Model.Entry>(
                            parentKey,
                            key
                        );
                    }
                }
            }
                var domain = new Gs2.Gs2Dictionary.Domain.Model.EntryDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Dictionary.Domain.Model.EntryDomain(
                        this._gs2,
                        request.NamespaceName,
                        result.Items[i]?.UserId,
                        result.Items[i]?.Name
                    );
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteEntriesFuture.")]
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryDomain[]> DeleteEntries(
            DeleteEntriesByUserIdRequest request
        ) {
            return DeleteEntriesFuture(request);
        }
        #endif

    }

    public partial class UserDomain {

    }
}
