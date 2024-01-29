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
#pragma warning disable CS0414 // Field is assigned but its value is never used

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
using Cysharp.Threading.Tasks.Linq;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Identifier.Domain
{

    public class Gs2Identifier {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2IdentifierRestClient _client;

        private readonly String _parentKey;

        public Gs2Identifier(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2IdentifierRestClient(
                gs2.RestSession
            );
            this._parentKey = "identifier";
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> CreateUserFuture(
            CreateUserRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain> self)
            {
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

                var requestModel = request;
                var resultModel = result;
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
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Identifier.Domain.Model.UserDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.UserDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.UserDomain> CreateUserAsync(
            #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.UserDomain> CreateUserAsync(
            #endif
            CreateUserRequest request
        ) {
            CreateUserResult result = null;
                result = await this._client.CreateUserAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Identifier.Domain.Model.UserDomain(
                    this._gs2,
                    result?.Item?.Name
                );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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

                var requestModel = request;
                var resultModel = result;
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
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> CreateSecurityPolicyAsync(
            #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain> CreateSecurityPolicyAsync(
            #endif
            CreateSecurityPolicyRequest request
        ) {
            CreateSecurityPolicyResult result = null;
                result = await this._client.CreateSecurityPolicyAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                    this._gs2,
                    result?.Item?.Name
                );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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
                this._gs2.Cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Identifier.Model.User> UsersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Identifier.Model.User> Users(
            #endif
        #else
        public DescribeUsersIterator UsersAsync(
        #endif
        )
        {
            return new DescribeUsersIterator(
                this._gs2.Cache,
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

        public ulong SubscribeUsers(
            Action<Gs2.Gs2Identifier.Model.User[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Identifier.Model.User>(
                "identifier:User",
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeUsersWithInitialCallAsync(
            Action<Gs2.Gs2Identifier.Model.User[]> callback
        )
        {
            var items = await UsersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeUsers(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeUsers(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Identifier.Model.User>(
                "identifier:User",
                callbackId
            );
        }

        public Gs2.Gs2Identifier.Domain.Model.UserDomain User(
            string userName
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.UserDomain(
                this._gs2,
                userName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Identifier.Model.SecurityPolicy> SecurityPolicies(
        )
        {
            return new DescribeSecurityPoliciesIterator(
                this._gs2.Cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Identifier.Model.SecurityPolicy> SecurityPoliciesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Identifier.Model.SecurityPolicy> SecurityPolicies(
            #endif
        #else
        public DescribeSecurityPoliciesIterator SecurityPoliciesAsync(
        #endif
        )
        {
            return new DescribeSecurityPoliciesIterator(
                this._gs2.Cache,
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

        public ulong SubscribeSecurityPolicies(
            Action<Gs2.Gs2Identifier.Model.SecurityPolicy[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                "identifier:SecurityPolicy",
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSecurityPoliciesWithInitialCallAsync(
            Action<Gs2.Gs2Identifier.Model.SecurityPolicy[]> callback
        )
        {
            var items = await SecurityPoliciesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeSecurityPolicies(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSecurityPolicies(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Identifier.Model.SecurityPolicy>(
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
                this._gs2.Cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Identifier.Model.SecurityPolicy> CommonSecurityPoliciesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Identifier.Model.SecurityPolicy> CommonSecurityPolicies(
            #endif
        #else
        public DescribeCommonSecurityPoliciesIterator CommonSecurityPoliciesAsync(
        #endif
        )
        {
            return new DescribeCommonSecurityPoliciesIterator(
                this._gs2.Cache,
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

        public ulong SubscribeCommonSecurityPolicies(
            Action<Gs2.Gs2Identifier.Model.SecurityPolicy[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                "identifier:SecurityPolicy",
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeCommonSecurityPoliciesWithInitialCallAsync(
            Action<Gs2.Gs2Identifier.Model.SecurityPolicy[]> callback
        )
        {
            var items = await CommonSecurityPoliciesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeCommonSecurityPolicies(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeCommonSecurityPolicies(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Identifier.Model.SecurityPolicy>(
                "identifier:SecurityPolicy",
                callbackId
            );
        }

        public Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain SecurityPolicy(
            string securityPolicyName
        ) {
            return new Gs2.Gs2Identifier.Domain.Model.SecurityPolicyDomain(
                this._gs2,
                securityPolicyName
            );
        }

        public void UpdateCacheFromStampSheet(
                string transactionId,
                string method,
                string request,
                string result
        ) {
        }

        public void UpdateCacheFromStampTask(
                string taskId,
                string method,
                string request,
                string result
        ) {
        }

        public void UpdateCacheFromJobResult(
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
