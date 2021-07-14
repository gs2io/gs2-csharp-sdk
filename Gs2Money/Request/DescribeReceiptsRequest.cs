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
using Gs2.Gs2Money.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Money.Request
{
	[Preserve]
	[System.Serializable]
	public class DescribeReceiptsRequest : Gs2Request<DescribeReceiptsRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public int? Slot { set; get; }
        public long? Begin { set; get; }
        public long? End { set; get; }
        public string PageToken { set; get; }
        public int? Limit { set; get; }

        public DescribeReceiptsRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public DescribeReceiptsRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public DescribeReceiptsRequest WithSlot(int? slot) {
            this.Slot = slot;
            return this;
        }

        public DescribeReceiptsRequest WithBegin(long? begin) {
            this.Begin = begin;
            return this;
        }

        public DescribeReceiptsRequest WithEnd(long? end) {
            this.End = end;
            return this;
        }

        public DescribeReceiptsRequest WithPageToken(string pageToken) {
            this.PageToken = pageToken;
            return this;
        }

        public DescribeReceiptsRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }

    	[Preserve]
        public static DescribeReceiptsRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DescribeReceiptsRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithSlot(!data.Keys.Contains("slot") || data["slot"] == null ? null : (int?)int.Parse(data["slot"].ToString()))
                .WithBegin(!data.Keys.Contains("begin") || data["begin"] == null ? null : (long?)long.Parse(data["begin"].ToString()))
                .WithEnd(!data.Keys.Contains("end") || data["end"] == null ? null : (long?)long.Parse(data["end"].ToString()))
                .WithPageToken(!data.Keys.Contains("pageToken") || data["pageToken"] == null ? null : data["pageToken"].ToString())
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)int.Parse(data["limit"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["slot"] = Slot,
                ["begin"] = Begin,
                ["end"] = End,
                ["pageToken"] = PageToken,
                ["limit"] = Limit,
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
            if (Slot != null) {
                writer.WritePropertyName("slot");
                writer.Write(int.Parse(Slot.ToString()));
            }
            if (Begin != null) {
                writer.WritePropertyName("begin");
                writer.Write(long.Parse(Begin.ToString()));
            }
            if (End != null) {
                writer.WritePropertyName("end");
                writer.Write(long.Parse(End.ToString()));
            }
            if (PageToken != null) {
                writer.WritePropertyName("pageToken");
                writer.Write(PageToken.ToString());
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write(int.Parse(Limit.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}