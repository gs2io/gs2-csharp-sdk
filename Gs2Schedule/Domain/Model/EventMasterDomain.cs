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
using Gs2.Gs2Schedule.Domain.Iterator;
using Gs2.Gs2Schedule.Request;
using Gs2.Gs2Schedule.Result;
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

namespace Gs2.Gs2Schedule.Domain.Model
{

    public partial class EventMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ScheduleRestClient _client;
        private readonly string _namespaceName;
        private readonly string _eventName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string EventName => _eventName;

        public EventMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string eventName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ScheduleRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._eventName = eventName;
            this._parentKey = Gs2.Gs2Schedule.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "EventMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string eventName,
            string childType
        )
        {
            return string.Join(
                ":",
                "schedule",
                namespaceName ?? "null",
                eventName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string eventName
        )
        {
            return string.Join(
                ":",
                eventName ?? "null"
            );
        }

    }

    public partial class EventMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Schedule.Model.EventMaster> GetFuture(
            GetEventMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Schedule.Model.EventMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithEventName(this.EventName);
                var future = this._client.GetEventMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                            request.EventName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Schedule.Model.EventMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "eventMaster")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithEventName(this.EventName);
                GetEventMasterResult result = null;
                try {
                    result = await this._client.GetEventMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                        request.EventName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Schedule.Model.EventMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "eventMaster")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Schedule.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "EventMaster"
                        );
                        var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
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
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Schedule.Model.EventMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Schedule.Model.EventMaster> GetAsync(
            GetEventMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithEventName(this.EventName);
            var future = this._client.GetEventMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                        request.EventName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Schedule.Model.EventMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "eventMaster")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithEventName(this.EventName);
            GetEventMasterResult result = null;
            try {
                result = await this._client.GetEventMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                    request.EventName.ToString()
                    );
                _cache.Put<Gs2.Gs2Schedule.Model.EventMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "eventMaster")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Schedule.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "EventMaster"
                    );
                    var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
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
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Domain.Model.EventMasterDomain> UpdateFuture(
            UpdateEventMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Schedule.Domain.Model.EventMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithEventName(this.EventName);
                var future = this._client.UpdateEventMasterFuture(
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
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithEventName(this.EventName);
                UpdateEventMasterResult result = null;
                    result = await this._client.UpdateEventMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Schedule.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "EventMaster"
                        );
                        var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
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
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Schedule.Domain.Model.EventMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Schedule.Domain.Model.EventMasterDomain> UpdateAsync(
            UpdateEventMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithEventName(this.EventName);
            var future = this._client.UpdateEventMasterFuture(
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
            request
                .WithNamespaceName(this.NamespaceName)
                .WithEventName(this.EventName);
            UpdateEventMasterResult result = null;
                result = await this._client.UpdateEventMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Schedule.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "EventMaster"
                    );
                    var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
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
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Schedule.Domain.Model.EventMasterDomain> UpdateAsync(
            UpdateEventMasterRequest request
        ) {
            var future = UpdateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Schedule.Domain.Model.EventMasterDomain> Update(
            UpdateEventMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Domain.Model.EventMasterDomain> DeleteFuture(
            DeleteEventMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Schedule.Domain.Model.EventMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithEventName(this.EventName);
                var future = this._client.DeleteEventMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                            request.EventName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Schedule.Model.EventMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "eventMaster")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithEventName(this.EventName);
                DeleteEventMasterResult result = null;
                try {
                    result = await this._client.DeleteEventMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                        request.EventName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Schedule.Model.EventMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "eventMaster")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Schedule.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "EventMaster"
                        );
                        var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Schedule.Model.EventMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Schedule.Domain.Model.EventMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Schedule.Domain.Model.EventMasterDomain> DeleteAsync(
            DeleteEventMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithEventName(this.EventName);
            var future = this._client.DeleteEventMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                        request.EventName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Schedule.Model.EventMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "eventMaster")
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithEventName(this.EventName);
            DeleteEventMasterResult result = null;
            try {
                result = await this._client.DeleteEventMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                    request.EventName.ToString()
                    );
                _cache.Put<Gs2.Gs2Schedule.Model.EventMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "eventMaster")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Schedule.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "EventMaster"
                    );
                    var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Schedule.Model.EventMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Schedule.Domain.Model.EventMasterDomain> DeleteAsync(
            DeleteEventMasterRequest request
        ) {
            var future = DeleteFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Schedule.Domain.Model.EventMasterDomain> Delete(
            DeleteEventMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class EventMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Schedule.Model.EventMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Schedule.Model.EventMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Schedule.Model.EventMaster>(
                    _parentKey,
                    Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                        this.EventName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetEventMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                                    this.EventName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Schedule.Model.EventMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "eventMaster")
                            {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    (value, _) = _cache.Get<Gs2.Gs2Schedule.Model.EventMaster>(
                        _parentKey,
                        Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                            this.EventName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Schedule.Model.EventMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Schedule.Model.EventMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Schedule.Model.EventMaster>(
                    _parentKey,
                    Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                        this.EventName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetEventMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                                    this.EventName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Schedule.Model.EventMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "eventMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Schedule.Model.EventMaster>(
                        _parentKey,
                        Gs2.Gs2Schedule.Domain.Model.EventMasterDomain.CreateCacheKey(
                            this.EventName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Schedule.Model.EventMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Schedule.Model.EventMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Schedule.Model.EventMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Schedule.Model.EventMaster> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
