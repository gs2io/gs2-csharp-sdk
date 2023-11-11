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
using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Inbox.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Inbox.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class SendMessageByUserIdRequest : Gs2Request<SendMessageByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string Metadata { set; get; }
        public Gs2.Core.Model.AcquireAction[] ReadAcquireActions { set; get; }
        public long? ExpiresAt { set; get; }
        public Gs2.Gs2Inbox.Model.TimeSpan_ ExpiresTimeSpan { set; get; }
        public string DuplicationAvoider { set; get; }

        public SendMessageByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public SendMessageByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public SendMessageByUserIdRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public SendMessageByUserIdRequest WithReadAcquireActions(Gs2.Core.Model.AcquireAction[] readAcquireActions) {
            this.ReadAcquireActions = readAcquireActions;
            return this;
        }

        public SendMessageByUserIdRequest WithExpiresAt(long? expiresAt) {
            this.ExpiresAt = expiresAt;
            return this;
        }

        public SendMessageByUserIdRequest WithExpiresTimeSpan(Gs2.Gs2Inbox.Model.TimeSpan_ expiresTimeSpan) {
            this.ExpiresTimeSpan = expiresTimeSpan;
            return this;
        }

        public SendMessageByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SendMessageByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SendMessageByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithReadAcquireActions(!data.Keys.Contains("readAcquireActions") || data["readAcquireActions"] == null || !data["readAcquireActions"].IsArray ? new Gs2.Core.Model.AcquireAction[]{} : data["readAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)long.Parse(data["expiresAt"].ToString()))
                .WithExpiresTimeSpan(!data.Keys.Contains("expiresTimeSpan") || data["expiresTimeSpan"] == null ? null : Gs2.Gs2Inbox.Model.TimeSpan_.FromJson(data["expiresTimeSpan"]));
        }

        public override JsonData ToJson()
        {
            JsonData readAcquireActionsJsonData = null;
            if (ReadAcquireActions != null)
            {
                readAcquireActionsJsonData = new JsonData();
                foreach (var readAcquireAction in ReadAcquireActions)
                {
                    readAcquireActionsJsonData.Add(readAcquireAction.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["metadata"] = Metadata,
                ["readAcquireActions"] = readAcquireActionsJsonData,
                ["expiresAt"] = ExpiresAt,
                ["expiresTimeSpan"] = ExpiresTimeSpan?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            writer.WriteArrayStart();
            foreach (var readAcquireAction in ReadAcquireActions)
            {
                if (readAcquireAction != null) {
                    readAcquireAction.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write(long.Parse(ExpiresAt.ToString()));
            }
            if (ExpiresTimeSpan != null) {
                ExpiresTimeSpan.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += Metadata + ":";
            key += ReadAcquireActions + ":";
            key += ExpiresAt + ":";
            key += ExpiresTimeSpan + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new SendMessageByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                Metadata = Metadata,
                ReadAcquireActions = ReadAcquireActions,
                ExpiresAt = ExpiresAt,
                ExpiresTimeSpan = ExpiresTimeSpan,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (SendMessageByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values SendMessageByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values SendMessageByUserIdRequest::userId");
            }
            if (Metadata != y.Metadata) {
                throw new ArithmeticException("mismatch parameter values SendMessageByUserIdRequest::metadata");
            }
            if (ReadAcquireActions != y.ReadAcquireActions) {
                throw new ArithmeticException("mismatch parameter values SendMessageByUserIdRequest::readAcquireActions");
            }
            if (ExpiresAt != y.ExpiresAt) {
                throw new ArithmeticException("mismatch parameter values SendMessageByUserIdRequest::expiresAt");
            }
            if (ExpiresTimeSpan != y.ExpiresTimeSpan) {
                throw new ArithmeticException("mismatch parameter values SendMessageByUserIdRequest::expiresTimeSpan");
            }
            return new SendMessageByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                Metadata = Metadata,
                ReadAcquireActions = ReadAcquireActions,
                ExpiresAt = ExpiresAt,
                ExpiresTimeSpan = ExpiresTimeSpan,
            };
        }
    }
}