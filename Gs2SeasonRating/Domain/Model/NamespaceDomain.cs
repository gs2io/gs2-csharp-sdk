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
using Gs2.Gs2SeasonRating.Domain.Iterator;
using Gs2.Gs2SeasonRating.Model.Cache;
using Gs2.Gs2SeasonRating.Request;
using Gs2.Gs2SeasonRating.Result;
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

namespace Gs2.Gs2SeasonRating.Domain.Model
{

    public partial class NamespaceDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2SeasonRatingRestClient _client;
        public string NamespaceName { get; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string UploadToken { get; set; }
        public string UploadUrl { get; set; }
        public string NextPageToken { get; set; }

        public NamespaceDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2SeasonRatingRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }

        public Gs2.Gs2SeasonRating.Domain.Model.CurrentSeasonModelMasterDomain CurrentSeasonModelMaster(
        ) {
            return new Gs2.Gs2SeasonRating.Domain.Model.CurrentSeasonModelMasterDomain(
                this._gs2,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2SeasonRating.Model.SeasonModel> SeasonModels(
        )
        {
            return new DescribeSeasonModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2SeasonRating.Model.SeasonModel> SeasonModelsAsync(
            #else
        public DescribeSeasonModelsIterator SeasonModelsAsync(
            #endif
        )
        {
            return new DescribeSeasonModelsIterator(
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

        public ulong SubscribeSeasonModels(
            Action<Gs2.Gs2SeasonRating.Model.SeasonModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2SeasonRating.Model.SeasonModel>(
                (null as Gs2.Gs2SeasonRating.Model.SeasonModel).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSeasonModelsWithInitialCallAsync(
            Action<Gs2.Gs2SeasonRating.Model.SeasonModel[]> callback
        )
        {
            var items = await SeasonModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeSeasonModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSeasonModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2SeasonRating.Model.SeasonModel>(
                (null as Gs2.Gs2SeasonRating.Model.SeasonModel).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        public Gs2.Gs2SeasonRating.Domain.Model.SeasonModelDomain SeasonModel(
            string seasonName
        ) {
            return new Gs2.Gs2SeasonRating.Domain.Model.SeasonModelDomain(
                this._gs2,
                this.NamespaceName,
                seasonName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> SeasonModelMasters(
        )
        {
            return new DescribeSeasonModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2SeasonRating.Model.SeasonModelMaster> SeasonModelMastersAsync(
            #else
        public DescribeSeasonModelMastersIterator SeasonModelMastersAsync(
            #endif
        )
        {
            return new DescribeSeasonModelMastersIterator(
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

        public ulong SubscribeSeasonModelMasters(
            Action<Gs2.Gs2SeasonRating.Model.SeasonModelMaster[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2SeasonRating.Model.SeasonModelMaster>(
                (null as Gs2.Gs2SeasonRating.Model.SeasonModelMaster).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSeasonModelMastersWithInitialCallAsync(
            Action<Gs2.Gs2SeasonRating.Model.SeasonModelMaster[]> callback
        )
        {
            var items = await SeasonModelMastersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeSeasonModelMasters(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSeasonModelMasters(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2SeasonRating.Model.SeasonModelMaster>(
                (null as Gs2.Gs2SeasonRating.Model.SeasonModelMaster).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        public Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain SeasonModelMaster(
            string seasonName
        ) {
            return new Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                seasonName
            );
        }

        public Gs2.Gs2SeasonRating.Domain.Model.UserDomain User(
            string userId
        ) {
            return new Gs2.Gs2SeasonRating.Domain.Model.UserDomain(
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
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2SeasonRating.Model.MatchSession> MatchSessions(
        )
        {
            return new DescribeMatchSessionsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2SeasonRating.Model.MatchSession> MatchSessionsAsync(
            #else
        public DescribeMatchSessionsIterator MatchSessionsAsync(
            #endif
        )
        {
            return new DescribeMatchSessionsIterator(
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

        public ulong SubscribeMatchSessions(
            Action<Gs2.Gs2SeasonRating.Model.MatchSession[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2SeasonRating.Model.MatchSession>(
                (null as Gs2.Gs2SeasonRating.Model.MatchSession).CacheParentKey(
                    this.NamespaceName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeMatchSessionsWithInitialCallAsync(
            Action<Gs2.Gs2SeasonRating.Model.MatchSession[]> callback
        )
        {
            var items = await MatchSessionsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeMatchSessions(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeMatchSessions(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2SeasonRating.Model.MatchSession>(
                (null as Gs2.Gs2SeasonRating.Model.MatchSession).CacheParentKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        public Gs2.Gs2SeasonRating.Domain.Model.MatchSessionDomain MatchSession(
            string sessionName
        ) {
            return new Gs2.Gs2SeasonRating.Domain.Model.MatchSessionDomain(
                this._gs2,
                this.NamespaceName,
                sessionName
            );
        }

        public Gs2.Gs2SeasonRating.Domain.Model.VoteDomain Vote(
            string seasonName,
            string sessionName
        ) {
            return new Gs2.Gs2SeasonRating.Domain.Model.VoteDomain(
                this._gs2,
                this.NamespaceName,
                seasonName,
                sessionName
            );
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
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
            return new Gs2InlineFuture<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> GetStatusAsync(
            #else
        public async Task<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> GetStatusAsync(
            #endif
            GetNamespaceStatusRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
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
        private IFuture<Gs2.Gs2SeasonRating.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SeasonRating.Model.Namespace> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
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
            return new Gs2InlineFuture<Gs2.Gs2SeasonRating.Model.Namespace>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2SeasonRating.Model.Namespace> GetAsync(
            #else
        private async Task<Gs2.Gs2SeasonRating.Model.Namespace> GetAsync(
            #endif
            GetNamespaceRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
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
        public IFuture<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
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
            return new Gs2InlineFuture<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> UpdateAsync(
            #endif
            UpdateNamespaceRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
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
        public IFuture<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
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
            return new Gs2InlineFuture<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2SeasonRating.Domain.Model.NamespaceDomain> DeleteAsync(
            #endif
            DeleteNamespaceRequest request
        ) {
            try {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
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
        public IFuture<Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain> CreateSeasonModelMasterFuture(
            CreateSeasonModelMasterRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CreateSeasonModelMasterFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain> CreateSeasonModelMasterAsync(
            #else
        public async Task<Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain> CreateSeasonModelMasterAsync(
            #endif
            CreateSeasonModelMasterRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateSeasonModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2SeasonRating.Domain.Model.SeasonModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SeasonRating.Domain.Model.MatchSessionDomain> CreateMatchSessionFuture(
            CreateMatchSessionRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SeasonRating.Domain.Model.MatchSessionDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CreateMatchSessionFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2SeasonRating.Domain.Model.MatchSessionDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2SeasonRating.Domain.Model.MatchSessionDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SeasonRating.Domain.Model.MatchSessionDomain> CreateMatchSessionAsync(
            #else
        public async Task<Gs2.Gs2SeasonRating.Domain.Model.MatchSessionDomain> CreateMatchSessionAsync(
            #endif
            CreateMatchSessionRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateMatchSessionAsync(request)
            );
            var domain = new Gs2.Gs2SeasonRating.Domain.Model.MatchSessionDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SeasonRating.Domain.Model.BallotDomain> VoteFuture(
            VoteRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SeasonRating.Domain.Model.BallotDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.VoteFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2SeasonRating.Domain.Model.BallotDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.SeasonName,
                    result?.Item?.SessionName,
                    result?.Item?.NumberOfPlayer,
                    request.KeyId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2SeasonRating.Domain.Model.BallotDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SeasonRating.Domain.Model.BallotDomain> VoteAsync(
            #else
        public async Task<Gs2.Gs2SeasonRating.Domain.Model.BallotDomain> VoteAsync(
            #endif
            VoteRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.VoteAsync(request)
            );
            var domain = new Gs2.Gs2SeasonRating.Domain.Model.BallotDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.SeasonName,
                result?.Item?.SessionName,
                result?.Item?.NumberOfPlayer,
                request.KeyId
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SeasonRating.Domain.Model.BallotDomain> VoteMultipleFuture(
            VoteMultipleRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2SeasonRating.Domain.Model.BallotDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.VoteMultipleFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2SeasonRating.Domain.Model.BallotDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.SeasonName,
                    result?.Item?.SessionName,
                    result?.Item?.NumberOfPlayer,
                    request.KeyId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2SeasonRating.Domain.Model.BallotDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SeasonRating.Domain.Model.BallotDomain> VoteMultipleAsync(
            #else
        public async Task<Gs2.Gs2SeasonRating.Domain.Model.BallotDomain> VoteMultipleAsync(
            #endif
            VoteMultipleRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.VoteMultipleAsync(request)
            );
            var domain = new Gs2.Gs2SeasonRating.Domain.Model.BallotDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.SeasonName,
                result?.Item?.SessionName,
                result?.Item?.NumberOfPlayer,
                request.KeyId
            );

            return domain;
        }
        #endif

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SeasonRating.Model.Namespace> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2SeasonRating.Model.Namespace> self)
            {
                var (value, find) = (null as Gs2.Gs2SeasonRating.Model.Namespace).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2SeasonRating.Model.Namespace).FetchFuture(
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
            return new Gs2InlineFuture<Gs2.Gs2SeasonRating.Model.Namespace>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2SeasonRating.Model.Namespace> ModelAsync()
            #else
        public async Task<Gs2.Gs2SeasonRating.Model.Namespace> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2SeasonRating.Model.Namespace).GetCache(
                this._gs2.Cache,
                this.NamespaceName
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2SeasonRating.Model.Namespace).FetchAsync(
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
        public async UniTask<Gs2.Gs2SeasonRating.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2SeasonRating.Model.Namespace> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2SeasonRating.Model.Namespace> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2SeasonRating.Model.Namespace).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2SeasonRating.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2SeasonRating.Model.Namespace).CacheParentKey(
                ),
                (null as Gs2.Gs2SeasonRating.Model.Namespace).CacheKey(
                    this.NamespaceName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2SeasonRating.Model.Namespace>(
                (null as Gs2.Gs2SeasonRating.Model.Namespace).CacheParentKey(
                ),
                (null as Gs2.Gs2SeasonRating.Model.Namespace).CacheKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2SeasonRating.Model.Namespace> callback)
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
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2SeasonRating.Model.Namespace> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2SeasonRating.Model.Namespace> callback)
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
