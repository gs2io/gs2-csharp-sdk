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
#pragma warning disable CS0414 // Field is assigned but its value is never used

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Distributor.Domain.Iterator;
using Gs2.Gs2Distributor.Domain.Model;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Distributor.Model;
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

namespace Gs2.Gs2Distributor.Domain
{

    public class Gs2Distributor {

        private static readonly List<AutoRunStampSheetNotification> _completedStampSheets = new List<AutoRunStampSheetNotification>();
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DistributorRestClient _client;

        private readonly String _parentKey;

        public Gs2Distributor(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DistributorRestClient(
                gs2.RestSession
            );
            this._parentKey = "distributor";
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> self)
            {
                var future = this._client.CreateNamespaceFuture(
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
                            "distributor",
                            "Namespace"
                        );
                        var key = Gs2.Gs2Distributor.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Distributor.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public async Task<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #endif
            CreateNamespaceRequest request
        ) {
            CreateNamespaceResult result = null;
                result = await this._client.CreateNamespaceAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "distributor",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Distributor.Domain.Model.NamespaceDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Distributor.Domain.Model.NamespaceDomain(
                    this._gs2,
                    result?.Item?.Name
                );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateNamespaceFuture.")]
        public IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> CreateNamespace(
            CreateNamespaceRequest request
        ) {
            return CreateNamespaceFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Distributor.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2.Cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Distributor.Model.Namespace> NamespacesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Distributor.Model.Namespace> Namespaces(
            #endif
        #else
        public DescribeNamespacesIterator NamespacesAsync(
        #endif
        )
        {
            return new DescribeNamespacesIterator(
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

        public ulong SubscribeNamespaces(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Distributor.Model.Namespace>(
                "distributor:Namespace",
                callback
            );
        }

        public void UnsubscribeNamespaces(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Distributor.Model.Namespace>(
                "distributor:Namespace",
                callbackId
            );
        }

        public Gs2.Gs2Distributor.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Distributor.Domain.Model.NamespaceDomain(
                this._gs2,
                namespaceName
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
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class AutoRunStampSheetNotificationEvent : UnityEvent<AutoRunStampSheetNotification>
        {

        }

        [SerializeField]
        private AutoRunStampSheetNotificationEvent onAutoRunStampSheetNotification = new AutoRunStampSheetNotificationEvent();

        public event UnityAction<AutoRunStampSheetNotification> OnAutoRunStampSheetNotification
        {
            add => onAutoRunStampSheetNotification.AddListener(value);
            remove => onAutoRunStampSheetNotification.RemoveListener(value);
        }
    #endif

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
    #if UNITY_2017_1_OR_NEWER
            switch (action) {
                case "AutoRunStampSheetNotification": {
                    lock (_completedStampSheets)
                    {
                        var notification = AutoRunStampSheetNotification.FromJson(JsonMapper.ToObject(payload));
                        _completedStampSheets.Add(notification);
                        onAutoRunStampSheetNotification.Invoke(notification);
                    }
                    break;
                }
            }
    #endif
        }

    #if UNITY_2017_1_OR_NEWER
        public Gs2Future DispatchFuture(
            AccessToken accessToken
        )
        {
            AutoRunStampSheetNotification[] copiedCompletedStampSheets;

            IEnumerator Impl(Gs2Future self)
            {
                lock (_completedStampSheets)
                {
                    if (_completedStampSheets.Count == 0)
                    {
                        yield break;
                    }

                    copiedCompletedStampSheets = new AutoRunStampSheetNotification[_completedStampSheets.Count];
                    _completedStampSheets.Where(v => v.UserId == accessToken.UserId).ToList().CopyTo(copiedCompletedStampSheets);
                    foreach (var copiedCompletedStampSheet in copiedCompletedStampSheets) {
                        _completedStampSheets.Remove(copiedCompletedStampSheet);
                    }
                }

                foreach (var completedStampSheet in copiedCompletedStampSheets) {
                    if (completedStampSheet == null) continue;
                    {
                        var future = _gs2.Distributor.Namespace(
                            completedStampSheet.NamespaceName
                        ).AccessToken(
                            accessToken
                        ).StampSheetResult(
                            completedStampSheet.TransactionId
                        ).ModelNoCacheFuture();
                        yield return future;
                        if (future.Error != null) {
                            if (future.Error is Gs2.Core.Exception.NotFoundException) {
                            }
                            else {
                                self.OnError(future.Error);
                            }
                            yield break;
                        }
                    }
                    {
                        var autoRun = new AutoTransactionAccessTokenDomain(
                            _gs2,
                            accessToken,
                            completedStampSheet.TransactionId
                        );
                        var future = autoRun.WaitFuture();
                        yield return future;
                        if (future.Error != null) {
                            if (future.Error is Gs2.Core.Exception.NotFoundException) {
                            }
                            else {
                                self.OnError(future.Error);
                            }
                            yield break;
                        }
                    }
                }
            }

            return new Gs2InlineFuture(Impl);
        }
        
        public Gs2Future DispatchByUserIdFuture(
            string userId
        )
        {
            AutoRunStampSheetNotification[] copiedCompletedStampSheets;

            IEnumerator Impl(Gs2Future self)
            {
                lock (_completedStampSheets)
                {
                    if (_completedStampSheets.Count == 0)
                    {
                        yield break;
                    }

                    copiedCompletedStampSheets = new AutoRunStampSheetNotification[_completedStampSheets.Count];
                    _completedStampSheets.Where(v => v.UserId == userId).ToList().CopyTo(copiedCompletedStampSheets);
                    foreach (var copiedCompletedStampSheet in copiedCompletedStampSheets) {
                        _completedStampSheets.Remove(copiedCompletedStampSheet);
                    }
                }

                foreach (var completedStampSheet in copiedCompletedStampSheets) {
                    if (completedStampSheet == null) continue;
                    {
                        var future = _gs2.Distributor.Namespace(
                            completedStampSheet.NamespaceName
                        ).User(
                            userId
                        ).StampSheetResult(
                            completedStampSheet.TransactionId
                        ).ModelNoCacheFuture();
                        yield return future;
                        if (future.Error != null) {
                            if (future.Error is Gs2.Core.Exception.NotFoundException) {
                            }
                            else {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                    }
                    {
                        var autoRun = new AutoTransactionDomain(
                            _gs2,
                            userId,
                            completedStampSheet.TransactionId
                        );
                        var future = autoRun.WaitFuture();
                        yield return future;
                        if (future.Error != null) {
                            if (future.Error is Gs2.Core.Exception.NotFoundException) {
                            }
                            else {
                                self.OnError(future.Error);
                            }
                            yield break;
                        }
                    }
                }
            }

            return new Gs2InlineFuture(Impl);
        }
    #endif

    #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
        #if UNITY_2017_1_OR_NEWER
        public async UniTask DispatchAsync(
        #else
        public async Task DispatchAsync(
        #endif
            AccessToken accessToken
        )
        {
            AutoRunStampSheetNotification[] copiedCompletedStampSheets;

            lock (_completedStampSheets)
            {
                if (_completedStampSheets.Count == 0)
                {
                    return;
                }

                copiedCompletedStampSheets = new AutoRunStampSheetNotification[_completedStampSheets.Count];
                _completedStampSheets.Where(v => v.UserId == accessToken.UserId).ToList().CopyTo(copiedCompletedStampSheets);
                foreach (var copiedCompletedStampSheet in copiedCompletedStampSheets) {
                    _completedStampSheets.Remove(copiedCompletedStampSheet);
                }
            }

            foreach (var completedStampSheet in copiedCompletedStampSheets) {
                if (completedStampSheet == null) continue;
                var autoRun = new AutoTransactionAccessTokenDomain(
                    _gs2,
                    accessToken,
                    completedStampSheet.TransactionId
                );
                try
                {
                    for (var i = 0; i < 3; i++) {
                        var item = await _gs2.Distributor.Namespace(
                            completedStampSheet.NamespaceName
                        ).AccessToken(
                            accessToken
                        ).StampSheetResult(
                            completedStampSheet.TransactionId
                        ).ModelNoCacheAsync();
                        if (item != null) break;
                    }
                    await autoRun.WaitAsync();
                }
                catch (NotFoundException)
                {
                }
            }
        }
        
        #if UNITY_2017_1_OR_NEWER
        public async UniTask DispatchByUserIdAsync(
        #else
        public async Task DispatchByUserIdAsync(
        #endif
            string userId
        )
        {
            AutoRunStampSheetNotification[] copiedCompletedStampSheets;

            lock (_completedStampSheets)
            {
                if (_completedStampSheets.Count == 0)
                {
                    return;
                }

                copiedCompletedStampSheets = new AutoRunStampSheetNotification[_completedStampSheets.Count];
                _completedStampSheets.Where(v => v.UserId == userId).ToList().CopyTo(copiedCompletedStampSheets);
                foreach (var copiedCompletedStampSheet in copiedCompletedStampSheets) {
                    _completedStampSheets.Remove(copiedCompletedStampSheet);
                }
            }

            foreach (var completedStampSheet in copiedCompletedStampSheets) {
                if (completedStampSheet == null) continue;
                var autoRun = new AutoTransactionDomain(
                    _gs2,
                    userId,
                    completedStampSheet.TransactionId
                );
                try
                {
                    await _gs2.Distributor.Namespace(
                        completedStampSheet.NamespaceName
                    ).User(
                        userId
                    ).StampSheetResult(
                        completedStampSheet.TransactionId
                    ).ModelNoCacheAsync();
                    await autoRun.WaitAsync();
                }
                catch (NotFoundException)
                {
                }
            }
        }
    #endif
    }
}
