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

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Inbox.Request;
using Gs2.Core.Model;
using Gs2.Gs2Inbox.Model.Cache;
using Gs2.Gs2Inbox.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inbox.Domain.Transaction.SpeculativeExecutor
{
    public static class ReadMessageByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inbox:ReadMessageByUserId";
        }

        public static ConsumeAction[] BuildConsumeActions(
            ReadMessageByUserIdRequest request
        ) {
            return new [] {
                new ConsumeAction {
                    Action = "Gs2Inbox:OpenMessageByUserId",
                    Request = new OpenMessageByUserIdRequest {
                        NamespaceName = request.NamespaceName,
                        UserId = request.UserId,
                        MessageName = request.MessageName,
                    }.ToJson().ToJson(),
                },
            };
        }

        public static AcquireAction[] BuildAcquireActions(
            Gs2.Gs2Inbox.Model.Message item,
            Gs2.Gs2Inbox.Model.Config[] configs,
            string userId
        ) {
            return (item?.ReadAcquireActions ?? Array.Empty<AcquireAction>())
                .Where(action => action?.Action != null && action.Request != null)
                .Select(action => {
                var transformed = ApplyConfig(action, "userId", userId, false);
                foreach (var config in configs ?? Array.Empty<Gs2.Gs2Inbox.Model.Config>()) {
                    if (config?.Key != null && config.Value != null) {
                        transformed = ApplyConfig(
                            transformed,
                            config.Key,
                            config.Value,
                            true
                        );
                    }
                }
                return transformed;
            }).ToArray();
        }

        private static AcquireAction ApplyConfig(
            AcquireAction action,
            string key,
            string value,
            bool escapeJsonObject
        ) {
            var replacement = value;
            if (escapeJsonObject) {
                try {
                    var parsed = Gs2.Util.LitJson.JsonMapper.ToObject(value);
                    if (parsed != null && parsed.IsObject) {
                        replacement = EscapeJsonStringContent(value);
                    }
                }
                catch (System.Exception) {
                    // The server inserts non-object and invalid JSON values raw.
                }
            }
            return new AcquireAction()
                .WithAction(action.Action)
                .WithRequest(action.Request.Replace($"#{{{key}}}", replacement));
        }

        private static string EscapeJsonStringContent(string value) {
            var builder = new StringBuilder(value.Length);
            foreach (var character in value) {
                switch (character) {
                    case '\"': builder.Append("\\\""); break;
                    case '\\': builder.Append("\\\\"); break;
                    case '\b': builder.Append("\\b"); break;
                    case '\f': builder.Append("\\f"); break;
                    case '\n': builder.Append("\\n"); break;
                    case '\r': builder.Append("\\r"); break;
                    case '\t': builder.Append("\\t"); break;
                    case '<': builder.Append("\\u003c"); break;
                    case '>': builder.Append("\\u003e"); break;
                    case '&': builder.Append("\\u0026"); break;
                    case '\u2028': builder.Append("\\u2028"); break;
                    case '\u2029': builder.Append("\\u2029"); break;
                    default:
                        if (character < 0x20) {
                            builder.Append("\\u");
                            builder.Append(((int)character).ToString("x4"));
                        }
                        else {
                            builder.Append(character);
                        }
                        break;
                }
            }
            return builder.ToString();
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ReadMessageByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ReadMessageByUserIdRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            var prepared = request == null ? null :
                new ReadMessageByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithUserId(request.UserId)
                    .WithMessageName(request.MessageName)
                    .WithConfig(request.Config?
                        .Where(config => config != null)
                        .Select(config => config.Clone() as
                            Gs2.Gs2Inbox.Model.Config)
                        .ToArray())
                    .WithTimeOffsetToken(request.TimeOffsetToken);
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = token?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(token?.UserId) ||
                prepared?.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.MessageName)) {
                return null;
            }
            var userId = token.UserId;
            var cached = ((Gs2.Gs2Inbox.Model.Message)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                userId,
                prepared.MessageName,
                token.TimeOffset
            );
            var item = cached.Item1;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inbox:{prepared.NamespaceName}:" +
                $"user:{userId}:message:{prepared.MessageName}";
            if (!cached.Item2 || item == null ||
                item.MessageId != expectedId ||
                item.Name != prepared.MessageName || item.UserId != userId) {
                return null;
            }

            var openRequest = new OpenMessageByUserIdRequest()
                .WithNamespaceName(prepared.NamespaceName)
                .WithUserId(userId)
                .WithMessageName(prepared.MessageName);
            return await Gs2.Gs2Inbox.Domain.SpeculativeExecutor
                .OpenMessageByUserIdSpeculativeExecutor.ExecuteAsync(
                domain,
                token,
                openRequest
            );
        }
    }
}
