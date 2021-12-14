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
using Gs2.Gs2Identifier.Domain.Iterator;
using Gs2.Gs2Identifier.Request;
using Gs2.Gs2Identifier.Result;
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

namespace Gs2.Gs2Identifier.Domain.Model
{

    public partial class UserDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2IdentifierRestClient _client;
        private readonly string _userName;

        private readonly String _parentKey;
        public string ClientSecret { get; set; }
        public string NextPageToken { get; set; }
        public string UserName => _userName;

        public UserDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string userName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2IdentifierRestClient(
                session
            );
            this._userName = userName;
            this._parentKey = "identifier:Gs2.Gs2Identifier.Model.User";
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.UserDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.UserDomain> UpdateAsync(
        #endif
            UpdateUserRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
          IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> self)
          {
        #endif
            request
                .WithUserName(this._userName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateUserFuture(
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
            var result = await this._client.UpdateUserAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                        request.UserName != null ? request.UserName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Identifier.Domain.Model.UserDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Identifier.Model.User> GetAsync(
            #else
        private IFuture<Gs2.Gs2Identifier.Model.User> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Identifier.Model.User> GetAsync(
        #endif
            GetUserRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
          IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.User> self)
          {
        #endif
            request
                .WithUserName(this._userName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetUserFuture(
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
            var result = await this._client.GetUserAsync(
                request
            );
            #endif
                    
            if (result.Item != null) {
                _cache.Put(
                    _parentKey,
                    Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                        request.UserName != null ? request.UserName.ToString() : null
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.User>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.UserDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.UserDomain> DeleteAsync(
        #endif
            DeleteUserRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
          IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> self)
          {
        #endif
            request
                .WithUserName(this._userName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteUserFuture(
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
            DeleteUserResult result = null;
            try {
                result = await this._client.DeleteUserAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            _cache.Delete<Gs2.Gs2Identifier.Model.User>(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                    request.UserName != null ? request.UserName.ToString() : null
                )
            );
            Gs2.Gs2Identifier.Domain.Model.UserDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> CreateIdentifierAsync(
            #else
        public IFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> CreateIdentifier(
            #endif
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> CreateIdentifierAsync(
        #endif
            CreateIdentifierRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
          IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> self)
          {
        #endif
            request
                .WithUserName(this._userName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.CreateIdentifierFuture(
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
            var result = await this._client.CreateIdentifierAsync(
                request
            );
            #endif
            string parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                this._userName != null ? this._userName.ToString() : null,
                "Identifier"
            );
                    
            if (result.Item != null) {
                _cache.Put(
                    parentKey,
                    Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                        request.UserName != null ? request.UserName.ToString() : null,
                        result.Item?.ClientId?.ToString()
                    ),
                    result.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Identifier.Domain.Model.IdentifierDomain domain = new Gs2.Gs2Identifier.Domain.Model.IdentifierDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                result?.Item?.UserName,
                result?.Item?.ClientId
            );
            domain.ClientSecret = result?.ClientSecret;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Identifier.Model.Identifier> Identifiers(
            #else
        public Gs2Iterator<Gs2.Gs2Identifier.Model.Identifier> Identifiers(
            #endif
        #else
        public DescribeIdentifiersIterator Identifiers(
        #endif
        )
        {
            return new DescribeIdentifiersIterator(
                this._cache,
                this._client,
                this._userName
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

        public Gs2.Gs2Identifier.Domain.Model.IdentifierDomain Identifier(
            string clientId
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.IdentifierDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._userName,
                clientId
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Identifier.Model.Password> Passwords(
            #else
        public Gs2Iterator<Gs2.Gs2Identifier.Model.Password> Passwords(
            #endif
        #else
        public DescribePasswordsIterator Passwords(
        #endif
        )
        {
            return new DescribePasswordsIterator(
                this._cache,
                this._client,
                this._userName
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

        public Gs2.Gs2Identifier.Domain.Model.PasswordDomain Password(
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.PasswordDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._userName
            );
        }

        public Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain AttachSecurityPolicy(
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.AttachSecurityPolicyDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._userName
            );
        }

        public static string CreateCacheParentKey(
            string userName,
            string childType
        )
        {
            return string.Join(
                ":",
                "identifier",
                userName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string userName
        )
        {
            return string.Join(
                ":",
                userName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Model.User> Model() {
            #else
        public IFuture<Gs2.Gs2Identifier.Model.User> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Identifier.Model.User> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.User> self)
            {
        #endif
            Gs2.Gs2Identifier.Model.User value = _cache.Get<Gs2.Gs2Identifier.Model.User>(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                    this.UserName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetUserRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException)
                        {
                            _cache.Delete<Gs2.Gs2Identifier.Model.User>(
                            _parentKey,
                            Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                                this.UserName?.ToString()
                            )
                        );
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException) {
                    _cache.Delete<Gs2.Gs2Identifier.Model.User>(
                        _parentKey,
                        Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                            this.UserName?.ToString()
                        )
                    );
                }
        #endif
                value = _cache.Get<Gs2.Gs2Identifier.Model.User>(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
                    this.UserName?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.User>(Impl);
        #endif
        }

    }
}
