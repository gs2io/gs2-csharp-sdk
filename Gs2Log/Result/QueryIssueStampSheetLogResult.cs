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
using UnityEngine.Scripting;

namespace Gs2.Gs2Log.Result
{
	[Preserve]
	[System.Serializable]
	public class QueryIssueStampSheetLogResult : IResult
	{
        public Gs2.Gs2Log.Model.IssueStampSheetLog[] Items { set; get; }
        public string NextPageToken { set; get; }
        public long? TotalCount { set; get; }
        public long? ScanSize { set; get; }

        public QueryIssueStampSheetLogResult WithItems(Gs2.Gs2Log.Model.IssueStampSheetLog[] items) {
            this.Items = items;
            return this;
        }

        public QueryIssueStampSheetLogResult WithNextPageToken(string nextPageToken) {
            this.NextPageToken = nextPageToken;
            return this;
        }

        public QueryIssueStampSheetLogResult WithTotalCount(long? totalCount) {
            this.TotalCount = totalCount;
            return this;
        }

        public QueryIssueStampSheetLogResult WithScanSize(long? scanSize) {
            this.ScanSize = scanSize;
            return this;
        }

    	[Preserve]
        public static QueryIssueStampSheetLogResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new QueryIssueStampSheetLogResult()
                .WithItems(!data.Keys.Contains("items") || data["items"] == null ? new Gs2.Gs2Log.Model.IssueStampSheetLog[]{} : data["items"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Log.Model.IssueStampSheetLog.FromJson(v);
                }).ToArray())
                .WithNextPageToken(!data.Keys.Contains("nextPageToken") || data["nextPageToken"] == null ? null : data["nextPageToken"].ToString())
                .WithTotalCount(!data.Keys.Contains("totalCount") || data["totalCount"] == null ? null : (long?)long.Parse(data["totalCount"].ToString()))
                .WithScanSize(!data.Keys.Contains("scanSize") || data["scanSize"] == null ? null : (long?)long.Parse(data["scanSize"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["items"] = new JsonData(Items == null ? new JsonData[]{} :
                        Items.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["nextPageToken"] = NextPageToken,
                ["totalCount"] = TotalCount,
                ["scanSize"] = ScanSize,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            writer.WriteArrayStart();
            foreach (var item in Items)
            {
                if (item != null) {
                    item.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            if (NextPageToken != null) {
                writer.WritePropertyName("nextPageToken");
                writer.Write(NextPageToken.ToString());
            }
            if (TotalCount != null) {
                writer.WritePropertyName("totalCount");
                writer.Write(long.Parse(TotalCount.ToString()));
            }
            if (ScanSize != null) {
                writer.WritePropertyName("scanSize");
                writer.Write(long.Parse(ScanSize.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}