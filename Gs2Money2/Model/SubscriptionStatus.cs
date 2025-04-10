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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class SubscriptionStatus : IComparable
	{
        public string UserId { set; get; }
        public string ContentName { set; get; }
        public string Status { set; get; }
        public long? ExpiresAt { set; get; }
        public Gs2.Gs2Money2.Model.SubscribeTransaction[] Detail { set; get; }
        public SubscriptionStatus WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SubscriptionStatus WithContentName(string contentName) {
            this.ContentName = contentName;
            return this;
        }
        public SubscriptionStatus WithStatus(string status) {
            this.Status = status;
            return this;
        }
        public SubscriptionStatus WithExpiresAt(long? expiresAt) {
            this.ExpiresAt = expiresAt;
            return this;
        }
        public SubscriptionStatus WithDetail(Gs2.Gs2Money2.Model.SubscribeTransaction[] detail) {
            this.Detail = detail;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SubscriptionStatus FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SubscriptionStatus()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithContentName(!data.Keys.Contains("contentName") || data["contentName"] == null ? null : data["contentName"].ToString())
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString())
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)(data["expiresAt"].ToString().Contains(".") ? (long)double.Parse(data["expiresAt"].ToString()) : long.Parse(data["expiresAt"].ToString())))
                .WithDetail(!data.Keys.Contains("detail") || data["detail"] == null || !data["detail"].IsArray ? null : data["detail"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Money2.Model.SubscribeTransaction.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData detailJsonData = null;
            if (Detail != null && Detail.Length > 0)
            {
                detailJsonData = new JsonData();
                foreach (var detai in Detail)
                {
                    detailJsonData.Add(detai.ToJson());
                }
            }
            return new JsonData {
                ["userId"] = UserId,
                ["contentName"] = ContentName,
                ["status"] = Status,
                ["expiresAt"] = ExpiresAt,
                ["detail"] = detailJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ContentName != null) {
                writer.WritePropertyName("contentName");
                writer.Write(ContentName.ToString());
            }
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
            }
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write((ExpiresAt.ToString().Contains(".") ? (long)double.Parse(ExpiresAt.ToString()) : long.Parse(ExpiresAt.ToString())));
            }
            if (Detail != null) {
                writer.WritePropertyName("detail");
                writer.WriteArrayStart();
                foreach (var detai in Detail)
                {
                    if (detai != null) {
                        detai.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SubscriptionStatus;
            var diff = 0;
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (ContentName == null && ContentName == other.ContentName)
            {
                // null and null
            }
            else
            {
                diff += ContentName.CompareTo(other.ContentName);
            }
            if (Status == null && Status == other.Status)
            {
                // null and null
            }
            else
            {
                diff += Status.CompareTo(other.Status);
            }
            if (ExpiresAt == null && ExpiresAt == other.ExpiresAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ExpiresAt - other.ExpiresAt);
            }
            if (Detail == null && Detail == other.Detail)
            {
                // null and null
            }
            else
            {
                diff += Detail.Length - other.Detail.Length;
                for (var i = 0; i < Detail.Length; i++)
                {
                    diff += Detail[i].CompareTo(other.Detail[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscriptionStatus", "money2.subscriptionStatus.userId.error.tooLong"),
                    });
                }
            }
            {
                if (ContentName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscriptionStatus", "money2.subscriptionStatus.contentName.error.tooLong"),
                    });
                }
            }
            {
                switch (Status) {
                    case "active":
                    case "inactive":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("subscriptionStatus", "money2.subscriptionStatus.status.error.invalid"),
                        });
                }
            }
            {
                if (ExpiresAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscriptionStatus", "money2.subscriptionStatus.expiresAt.error.invalid"),
                    });
                }
                if (ExpiresAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscriptionStatus", "money2.subscriptionStatus.expiresAt.error.invalid"),
                    });
                }
            }
            {
                if (Detail.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscriptionStatus", "money2.subscriptionStatus.detail.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new SubscriptionStatus {
                UserId = UserId,
                ContentName = ContentName,
                Status = Status,
                ExpiresAt = ExpiresAt,
                Detail = Detail?.Clone() as Gs2.Gs2Money2.Model.SubscribeTransaction[],
            };
        }
    }
}