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
using Gs2.Gs2Log.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Log.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class QueryInGameLogRequest : Gs2Request<QueryInGameLogRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public Gs2.Gs2Log.Model.InGameLogTag[] Tags { set; get; } = null!;
         public long? Begin { set; get; } = null!;
         public long? End { set; get; } = null!;
         public bool? LongTerm { set; get; } = null!;
         public string PageToken { set; get; } = null!;
         public int? Limit { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public QueryInGameLogRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public QueryInGameLogRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public QueryInGameLogRequest WithTags(Gs2.Gs2Log.Model.InGameLogTag[] tags) {
            this.Tags = tags;
            return this;
        }
        public QueryInGameLogRequest WithBegin(long? begin) {
            this.Begin = begin;
            return this;
        }
        public QueryInGameLogRequest WithEnd(long? end) {
            this.End = end;
            return this;
        }
        public QueryInGameLogRequest WithLongTerm(bool? longTerm) {
            this.LongTerm = longTerm;
            return this;
        }
        public QueryInGameLogRequest WithPageToken(string pageToken) {
            this.PageToken = pageToken;
            return this;
        }
        public QueryInGameLogRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }
        public QueryInGameLogRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public QueryInGameLogRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static QueryInGameLogRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new QueryInGameLogRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTags(!data.Keys.Contains("tags") || data["tags"] == null || !data["tags"].IsArray ? null : data["tags"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Log.Model.InGameLogTag.FromJson(v);
                }).ToArray())
                .WithBegin(!data.Keys.Contains("begin") || data["begin"] == null ? null : (long?)(data["begin"].ToString().Contains(".") ? (long)double.Parse(data["begin"].ToString()) : long.Parse(data["begin"].ToString())))
                .WithEnd(!data.Keys.Contains("end") || data["end"] == null ? null : (long?)(data["end"].ToString().Contains(".") ? (long)double.Parse(data["end"].ToString()) : long.Parse(data["end"].ToString())))
                .WithLongTerm(!data.Keys.Contains("longTerm") || data["longTerm"] == null ? null : (bool?)bool.Parse(data["longTerm"].ToString()))
                .WithPageToken(!data.Keys.Contains("pageToken") || data["pageToken"] == null ? null : data["pageToken"].ToString())
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)(data["limit"].ToString().Contains(".") ? (int)double.Parse(data["limit"].ToString()) : int.Parse(data["limit"].ToString())))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData tagsJsonData = null;
            if (Tags != null && Tags.Length > 0)
            {
                tagsJsonData = new JsonData();
                foreach (var tag in Tags)
                {
                    tagsJsonData.Add(tag.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["tags"] = tagsJsonData,
                ["begin"] = Begin,
                ["end"] = End,
                ["longTerm"] = LongTerm,
                ["pageToken"] = PageToken,
                ["limit"] = Limit,
                ["timeOffsetToken"] = TimeOffsetToken,
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
            if (Tags != null) {
                writer.WritePropertyName("tags");
                writer.WriteArrayStart();
                foreach (var tag in Tags)
                {
                    if (tag != null) {
                        tag.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Begin != null) {
                writer.WritePropertyName("begin");
                writer.Write((Begin.ToString().Contains(".") ? (long)double.Parse(Begin.ToString()) : long.Parse(Begin.ToString())));
            }
            if (End != null) {
                writer.WritePropertyName("end");
                writer.Write((End.ToString().Contains(".") ? (long)double.Parse(End.ToString()) : long.Parse(End.ToString())));
            }
            if (LongTerm != null) {
                writer.WritePropertyName("longTerm");
                writer.Write(bool.Parse(LongTerm.ToString()));
            }
            if (PageToken != null) {
                writer.WritePropertyName("pageToken");
                writer.Write(PageToken.ToString());
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write((Limit.ToString().Contains(".") ? (int)double.Parse(Limit.ToString()) : int.Parse(Limit.ToString())));
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += Tags + ":";
            key += Begin + ":";
            key += End + ":";
            key += LongTerm + ":";
            key += PageToken + ":";
            key += Limit + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}