
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
using Gs2.Util.LitJson;
using Gs2.Gs2Chat.Model.Cache;
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

namespace Gs2.Gs2Chat.Domain.Iterator
{

    public class DescribeLatestMessagesByUserIdIterator :
    #if UNITY_2017_1_OR_NEWER
        Gs2Iterator<Gs2.Gs2Chat.Model.Message>,
    #endif
    #if GS2_ENABLE_UNITASK
        IUniTaskAsyncEnumerable<Gs2.Gs2Chat.Model.Message>
    #else
        IAsyncEnumerable<Gs2.Gs2Chat.Model.Message>
    #endif
    {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2ChatRestClient _client;
        public string NamespaceName { get; }
        public string RoomName { get; }
        public string Password { get; }
        public int? Category { get; }
        public string UserId { get; }
        public string TimeOffsetToken { get; }
        private long? _startAt;
        private bool _last;
        private Gs2.Gs2Chat.Model.Message[] _result;

        public static int? fetchSize;

        public DescribeLatestMessagesByUserIdIterator(
            Gs2.Core.Domain.Gs2 gs2,
            Gs2ChatRestClient client,
            string namespaceName,
            string userId,
            string roomName = null,
            string password = null,
            int? category = null,
            string timeOffsetToken = null
        ) {
            this._gs2 = gs2;
            this._client = client;
            this.NamespaceName = namespaceName;
            this.RoomName = roomName;
            this.Password = password;
            this.Category = category;
            this.UserId = userId;
            this.TimeOffsetToken = timeOffsetToken;
            this._startAt = null;
            this._last = false;
            this._result = new Gs2.Gs2Chat.Model.Message[]{};
        }

        #if GS2_ENABLE_UNITASK
        private async UniTask _load() {
        #else
        private async Task _load() {
        #endif
            var request = new Gs2.Gs2Chat.Request.DescribeLatestMessagesByUserIdRequest()
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRoomName(this.RoomName)
                .WithPassword(this.Password)
                .WithUserId(this.UserId)
                .WithLimit(fetchSize);
            var r = await this._client.DescribeLatestMessagesByUserIdAsync(
                request
            );
            this._result = r.Items
                .Where(item => this.Category == null || item.Category == this.Category)
                .ToArray();
            if (this._result.Length > 0) {
                this._startAt = this._result[this._result.Length-1].CreatedAt + 1;
            } else {
                this._last = true;
            }
            r.PutCache(
                this._gs2.Cache,
                UserId,
                null,
                request
            );
        }

        private bool _hasNext()
        {
            return true;
        }

        #if UNITY_2017_1_OR_NEWER
        public override bool HasNext()
        {
            if (Error != null) return false;
            return _hasNext();
        }

        protected override System.Collections.IEnumerator Next(
            Action<AsyncResult<Gs2.Gs2Chat.Model.Message>> callback
        )
        {
            if (this._result.Length == 0 && !this._last) {
                var future = this._load().ToGs2Future();
                yield return future;
                if (future.Error != null)
                {
                    Current = null;
                    Error = future.Error;
                    callback.Invoke(new AsyncResult<Gs2.Gs2Chat.Model.Message>(
                        Current,
                        Error
                    ));
                    yield break;
                }
            }
            if (this._result.Length == 0) {
                Current = null;
                callback.Invoke(new AsyncResult<Gs2.Gs2Chat.Model.Message>(
                    Current,
                    Error
                ));
                yield break;
            }
            var ret = this._result[0];
            this._result = this._result.ToList().GetRange(1, this._result.Length - 1).ToArray();
            Current = ret;
            callback.Invoke(new AsyncResult<Gs2.Gs2Chat.Model.Message>(
                Current,
                Error
            ));
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerator<Gs2.Gs2Chat.Model.Message> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
        ) => UniTaskAsyncEnumerable.Create<Gs2.Gs2Chat.Model.Message>(async (writer, token) =>
        #else
        public async IAsyncEnumerator<Gs2.Gs2Chat.Model.Message> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
        )
        #endif
        {
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
        #if GS2_ENABLE_UNITASK
                    await writer.YieldAsync(ret);
        #else
                    yield return ret;
        #endif
                }
        }
        #if GS2_ENABLE_UNITASK
        ).GetAsyncEnumerator();
        #endif
    }
}
