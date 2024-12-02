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
using Gs2.Gs2Distributor.Model.Cache;
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
using Cysharp.Threading.Tasks.Linq;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Distributor.Domain
{

    public class Gs2Distributor {

        private static readonly List<AutoRunStampSheetNotification> _completedStampSheets = new List<AutoRunStampSheetNotification>();
        private static readonly List<AutoRunTransactionNotification> _completedTransactions = new List<AutoRunTransactionNotification>();
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2DistributorRestClient _client;

        public Gs2Distributor(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2DistributorRestClient(
                gs2.RestSession
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> CreateNamespaceFuture(
            CreateNamespaceRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Distributor.Domain.Model.NamespaceDomain> self)
            {
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    null,
                    () => this._client.CreateNamespaceFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                () => this._client.CreateNamespaceAsync(request)
            );
            var domain = new Gs2.Gs2Distributor.Domain.Model.NamespaceDomain(
                this._gs2,
                result?.Item?.Name
            );
            return domain;
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Distributor.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2,
                this._client
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Distributor.Model.Namespace> NamespacesAsync(
            #else
        public DescribeNamespacesIterator NamespacesAsync(
            #endif
        )
        {
            return new DescribeNamespacesIterator(
                this._gs2,
                this._client
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeNamespaces(
            Action<Gs2.Gs2Distributor.Model.Namespace[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Distributor.Model.Namespace>(
                (null as Gs2.Gs2Distributor.Model.Namespace).CacheParentKey(
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeNamespacesWithInitialCallAsync(
            Action<Gs2.Gs2Distributor.Model.Namespace[]> callback
        )
        {
            var items = await NamespacesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeNamespaces(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeNamespaces(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Distributor.Model.Namespace>(
                (null as Gs2.Gs2Distributor.Model.Namespace).CacheParentKey(
                ),
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

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, IfExpressionByUserIdRequest, IfExpressionByUserIdResult> IfExpressionByUserIdComplete = new UnityEvent<string, IfExpressionByUserIdRequest, IfExpressionByUserIdResult>();
    #else
        public static Action<string, IfExpressionByUserIdRequest, IfExpressionByUserIdResult> IfExpressionByUserIdComplete;
    #endif

        public void UpdateCacheFromStampTask(
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "IfExpressionByUserId": {
                        var requestModel = IfExpressionByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = IfExpressionByUserIdResult.FromJson(JsonMapper.ToObject(result));

                        IfExpressionByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
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
    #if UNITY_2017_1_OR_NEWER
        [Serializable]
        private class AutoRunTransactionNotificationEvent : UnityEvent<AutoRunTransactionNotification>
        {

        }

        [SerializeField]
        private AutoRunTransactionNotificationEvent onAutoRunTransactionNotification = new AutoRunTransactionNotificationEvent();

        public event UnityAction<AutoRunTransactionNotification> OnAutoRunTransactionNotification
        {
            add => onAutoRunTransactionNotification.AddListener(value);
            remove => onAutoRunTransactionNotification.RemoveListener(value);
        }
    #endif

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
            switch (action) {
                case "AutoRunStampSheetNotification": {
                    lock (_completedStampSheets)
                    {
                        var notification = AutoRunStampSheetNotification.FromJson(JsonMapper.ToObject(payload));
                        _gs2.Cache.Delete<Gs2.Gs2Distributor.Model.StampSheetResult>(
                            (null as Gs2.Gs2Distributor.Model.StampSheetResult).CacheParentKey(
                                notification.NamespaceName,
                                notification.UserId
                            ),
                            (null as Gs2.Gs2Distributor.Model.StampSheetResult).CacheKey(
                                notification.TransactionId
                            )
                        );
                        _completedStampSheets.Add(notification);
    #if UNITY_2017_1_OR_NEWER
                        onAutoRunStampSheetNotification.Invoke(notification);
    #endif
                    }
                    break;
                }
                case "AutoRunTransactionNotification": {
                    lock (_completedTransactions)
                    {
                        var notification = AutoRunTransactionNotification.FromJson(JsonMapper.ToObject(payload));
                        _gs2.Cache.Delete<Gs2.Gs2Distributor.Model.TransactionResult>(
                            (null as Gs2.Gs2Distributor.Model.TransactionResult).CacheParentKey(
                                notification.NamespaceName,
                                notification.UserId
                            ),
                            (null as Gs2.Gs2Distributor.Model.TransactionResult).CacheKey(
                                notification.TransactionId
                            )
                        );
                        _completedTransactions.Add(notification);
    #if UNITY_2017_1_OR_NEWER
                        onAutoRunTransactionNotification.Invoke(notification);
    #endif
                    }
                    break;
                }
            }
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
                        for (var i = 0; i < 3; i++) {
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
                            if (future.Result != null) break;
                        }
                    }
                    {
                        var autoRun = new AutoStampSheetAccessTokenDomain(
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
                
                AutoRunTransactionNotification[] copiedCompletedTransactions;

                lock (_completedTransactions)
                {
                    if (_completedTransactions.Count == 0)
                    {
                        yield break;
                    }

                    copiedCompletedTransactions = new AutoRunTransactionNotification[_completedTransactions.Count];
                    _completedTransactions.Where(v => v.UserId == accessToken.UserId).ToList().CopyTo(copiedCompletedTransactions);
                    foreach (var copiedCompletedTransaction in copiedCompletedTransactions) {
                        _completedTransactions.Remove(copiedCompletedTransaction);
                    }
                }

                foreach (var completedTransaction in copiedCompletedTransactions) {
                    if (completedTransaction == null) continue;
                    {
                        for (var i = 0; i < 3; i++) {
                            var future = _gs2.Distributor.Namespace(
                                completedTransaction.NamespaceName
                            ).AccessToken(
                                accessToken
                            ).TransactionResult(
                                completedTransaction.TransactionId
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
                            if (future.Result != null) break;
                        }
                    }
                    {
                        var autoRun = new AutoTransactionAccessTokenDomain(
                            _gs2,
                            accessToken,
                            completedTransaction.TransactionId
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
            IEnumerator Impl(Gs2Future self)
            {
                AutoRunStampSheetNotification[] copiedCompletedStampSheets;

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
                        var autoRun = new AutoStampSheetDomain(
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
                
                AutoRunTransactionNotification[] copiedCompletedTransactions;

                lock (_completedTransactions)
                {
                    if (_completedTransactions.Count == 0)
                    {
                        yield break;
                    }

                    copiedCompletedTransactions = new AutoRunTransactionNotification[_completedTransactions.Count];
                    _completedTransactions.Where(v => v.UserId == userId).ToList().CopyTo(copiedCompletedTransactions);
                    foreach (var copiedCompletedTransaction in copiedCompletedTransactions) {
                        _completedTransactions.Remove(copiedCompletedTransaction);
                    }
                }

                foreach (var completedTransaction in copiedCompletedTransactions) {
                    if (completedTransaction == null) continue;
                    {
                        var future = _gs2.Distributor.Namespace(
                            completedTransaction.NamespaceName
                        ).User(
                            userId
                        ).TransactionResult(
                            completedTransaction.TransactionId
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
                            completedTransaction.TransactionId
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
                var autoRun = new AutoStampSheetAccessTokenDomain(
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
            
            AutoRunTransactionNotification[] copiedCompletedTransactions;

            lock (_completedTransactions)
            {
                if (_completedTransactions.Count == 0)
                {
                    return;
                }

                copiedCompletedTransactions = new AutoRunTransactionNotification[_completedTransactions.Count];
                _completedTransactions.Where(v => v.UserId == accessToken.UserId).ToList().CopyTo(copiedCompletedTransactions);
                foreach (var copiedCompletedTransaction in copiedCompletedTransactions) {
                    _completedTransactions.Remove(copiedCompletedTransaction);
                }
            }

            foreach (var completedTransaction in copiedCompletedTransactions) {
                if (completedTransaction == null) continue;
                var autoRun = new AutoTransactionAccessTokenDomain(
                    _gs2,
                    accessToken,
                    completedTransaction.TransactionId
                );
                try
                {
                    for (var i = 0; i < 3; i++) {
                        var item = await _gs2.Distributor.Namespace(
                            completedTransaction.NamespaceName
                        ).AccessToken(
                            accessToken
                        ).TransactionResult(
                            completedTransaction.TransactionId
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
                var autoRun = new AutoStampSheetDomain(
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
            
            AutoRunTransactionNotification[] copiedCompletedTransactions;

            lock (_completedTransactions)
            {
                if (_completedTransactions.Count == 0)
                {
                    return;
                }

                copiedCompletedTransactions = new AutoRunTransactionNotification[_completedTransactions.Count];
                _completedTransactions.Where(v => v.UserId == userId).ToList().CopyTo(copiedCompletedTransactions);
                foreach (var copiedCompletedTransaction in copiedCompletedTransactions) {
                    _completedTransactions.Remove(copiedCompletedTransaction);
                }
            }

            foreach (var completedTransaction in copiedCompletedTransactions) {
                if (completedTransaction == null) continue;
                var autoRun = new AutoTransactionDomain(
                    _gs2,
                    userId,
                    completedTransaction.TransactionId
                );
                try
                {
                    await _gs2.Distributor.Namespace(
                        completedTransaction.NamespaceName
                    ).User(
                        userId
                    ).TransactionResult(
                        completedTransaction.TransactionId
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
