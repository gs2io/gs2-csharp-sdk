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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Identifier.Domain.Iterator;
using Gs2.Gs2Identifier.Domain.Model;
using Gs2.Gs2Identifier.Request;
using Gs2.Gs2Identifier.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Identifier.Model;
#if UNITY_2017_1_OR_NEWER
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Identifier.Domain
{

    public class Gs2Identifier {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2IdentifierRestClient _client;

        private readonly String _parentKey;

        public Gs2Identifier(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2IdentifierRestClient(
                session
            );
            this._parentKey = "identifier";
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> CreateUserFuture(
            CreateUserRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                var future = this._client.CreateUserFuture(
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
                CreateUserResult result = null;
                    result = await this._client.CreateUserAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "identifier",
                            "User"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Identifier.Domain.Model.UserDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.UserDomain> CreateUserAsync(
            CreateUserRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            var future = this._client.CreateUserFuture(
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
            CreateUserResult result = null;
                result = await this._client.CreateUserAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "identifier",
                        "User"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Identifier.Domain.Model.UserDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result?.Item?.Name
                );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.UserDomain> CreateUserAsync(
            CreateUserRequest request
        ) {
            var future = CreateUserFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateUserFuture.")]
        public IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> CreateUser(
            CreateUserRequest request
        ) {
            return CreateUserFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> CreateSecurityPolicyFuture(
            CreateSecurityPolicyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                var future = this._client.CreateSecurityPolicyFuture(
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
                CreateSecurityPolicyResult result = null;
                    result = await this._client.CreateSecurityPolicyAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "identifier",
                            "SecurityPolicy"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> CreateSecurityPolicyAsync(
            CreateSecurityPolicyRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            var future = this._client.CreateSecurityPolicyFuture(
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
            CreateSecurityPolicyResult result = null;
                result = await this._client.CreateSecurityPolicyAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "identifier",
                        "SecurityPolicy"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    result?.Item?.Name
                );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> CreateSecurityPolicyAsync(
            CreateSecurityPolicyRequest request
        ) {
            var future = CreateSecurityPolicyFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateSecurityPolicyFuture.")]
        public IFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> CreateSecurityPolicy(
            CreateSecurityPolicyRequest request
        ) {
            return CreateSecurityPolicyFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Identifier.Model.User> Users(
        )
        {
            return new DescribeUsersIterator(
                this._cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Identifier.Model.User> UsersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Identifier.Model.User> Users(
            #endif
        #else
        public DescribeUsersIterator Users(
        #endif
        )
        {
            return new DescribeUsersIterator(
                this._cache,
                this._client
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

        public ulong SubscribeUsers(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Identifier.Model.User>(
                "identifier:User",
                callback
            );
        }

        public void UnsubscribeUsers(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Identifier.Model.User>(
                "identifier:User",
                callbackId
            );
        }

        public Gs2.Gs2Identifier.Domain.Model.UserDomain User(
            string userName
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.UserDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                userName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Identifier.Model.SecurityPolicy> SecurityPolicies(
        )
        {
            return new DescribeSecurityPoliciesIterator(
                this._cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Identifier.Model.SecurityPolicy> SecurityPoliciesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Identifier.Model.SecurityPolicy> SecurityPolicies(
            #endif
        #else
        public DescribeSecurityPoliciesIterator SecurityPolicies(
        #endif
        )
        {
            return new DescribeSecurityPoliciesIterator(
                this._cache,
                this._client
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

        public ulong SubscribeSecurityPolicies(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                "identifier:SecurityPolicy",
                callback
            );
        }

        public void UnsubscribeSecurityPolicies(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                "identifier:SecurityPolicy",
                callbackId
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Identifier.Model.SecurityPolicy> CommonSecurityPolicies(
        )
        {
            return new DescribeCommonSecurityPoliciesIterator(
                this._cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Identifier.Model.SecurityPolicy> CommonSecurityPoliciesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Identifier.Model.SecurityPolicy> CommonSecurityPolicies(
            #endif
        #else
        public DescribeCommonSecurityPoliciesIterator CommonSecurityPolicies(
        #endif
        )
        {
            return new DescribeCommonSecurityPoliciesIterator(
                this._cache,
                this._client
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

        public ulong SubscribeCommonSecurityPolicies(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                "identifier:SecurityPolicy",
                callback
            );
        }

        public void UnsubscribeCommonSecurityPolicies(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                "identifier:SecurityPolicy",
                callbackId
            );
        }

        public Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain SecurityPolicy(
            string securityPolicyName
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                securityPolicyName
            );
        }

        public static void UpdateCacheFromStampSheet(
                CacheDatabase cache,
                string transactionId,
                string method,
                string request,
                string result
        ) {
        }

        public static void UpdateCacheFromStampTask(
                CacheDatabase cache,
                string taskId,
                string method,
                string request,
                string result
        ) {
        }

        public static void UpdateCacheFromJobResult(
                CacheDatabase cache,
                string method,
                Gs2.Gs2JobQueue.Model.Job job,
                Gs2.Gs2JobQueue.Model.JobResultBody result
        ) {
        }

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
    #if UNITY_2017_1_OR_NEWER
    #endif
        }
    }
}
