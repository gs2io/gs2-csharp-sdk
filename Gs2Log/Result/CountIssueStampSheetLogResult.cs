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

namespace Gs2.Gs2Log.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CountIssueStampSheetLogResult : IResult
	{
        public Gs2.Gs2Log.Model.IssueStampSheetLogCount[] Items { set; get; }
        public string NextPageToken { set; get; }
        public long? TotalCount { set; get; }
        public long? ScanSize { set; get; }
        public ResultMetadata Metadata { set; get; }

        public CountIssueStampSheetLogResult WithItems(Gs2.Gs2Log.Model.IssueStampSheetLogCount[] items) {
            this.Items = items;
            return this;
        }

        public CountIssueStampSheetLogResult WithNextPageToken(string nextPageToken) {
            this.NextPageToken = nextPageToken;
            return this;
        }

        public CountIssueStampSheetLogResult WithTotalCount(long? totalCount) {
            this.TotalCount = totalCount;
            return this;
        }

        public CountIssueStampSheetLogResult WithScanSize(long? scanSize) {
            this.ScanSize = scanSize;
            return this;
        }

        public CountIssueStampSheetLogResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CountIssueStampSheetLogResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CountIssueStampSheetLogResult()
                .WithItems(!data.Keys.Contains("items") || data["items"] == null || !data["items"].IsArray ? null : data["items"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Log.Model.IssueStampSheetLogCount.FromJson(v);
                }).ToArray())
                .WithNextPageToken(!data.Keys.Contains("nextPageToken") || data["nextPageToken"] == null ? null : data["nextPageToken"].ToString())
                .WithTotalCount(!data.Keys.Contains("totalCount") || data["totalCount"] == null ? null : (long?)(data["totalCount"].ToString().Contains(".") ? (long)double.Parse(data["totalCount"].ToString()) : long.Parse(data["totalCount"].ToString())))
                .WithScanSize(!data.Keys.Contains("scanSize") || data["scanSize"] == null ? null : (long?)(data["scanSize"].ToString().Contains(".") ? (long)double.Parse(data["scanSize"].ToString()) : long.Parse(data["scanSize"].ToString())))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            JsonData itemsJsonData = null;
            if (Items != null && Items.Length > 0)
            {
                itemsJsonData = new JsonData();
                foreach (var item in Items)
                {
                    itemsJsonData.Add(item.ToJson());
                }
            }
            return new JsonData {
                ["items"] = itemsJsonData,
                ["nextPageToken"] = NextPageToken,
                ["totalCount"] = TotalCount,
                ["scanSize"] = ScanSize,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Items != null) {
                writer.WritePropertyName("items");
                writer.WriteArrayStart();
                foreach (var item in Items)
                {
                    if (item != null) {
                        item.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (NextPageToken != null) {
                writer.WritePropertyName("nextPageToken");
                writer.Write(NextPageToken.ToString());
            }
            if (TotalCount != null) {
                writer.WritePropertyName("totalCount");
                writer.Write((TotalCount.ToString().Contains(".") ? (long)double.Parse(TotalCount.ToString()) : long.Parse(TotalCount.ToString())));
            }
            if (ScanSize != null) {
                writer.WritePropertyName("scanSize");
                writer.Write((ScanSize.ToString().Contains(".") ? (long)double.Parse(ScanSize.ToString()) : long.Parse(ScanSize.ToString())));
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}