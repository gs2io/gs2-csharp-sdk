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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Showcase.Model
{
	[Preserve]
	public class Showcase : IComparable
	{

        /** 陳列棚 */
        public string showcaseId { set; get; }

        /**
         * 陳列棚を設定
         *
         * @param showcaseId 陳列棚
         * @return this
         */
        public Showcase WithShowcaseId(string showcaseId) {
            this.showcaseId = showcaseId;
            return this;
        }

        /** 商品名 */
        public string name { set; get; }

        /**
         * 商品名を設定
         *
         * @param name 商品名
         * @return this
         */
        public Showcase WithName(string name) {
            this.name = name;
            return this;
        }

        /** 商品のメタデータ */
        public string metadata { set; get; }

        /**
         * 商品のメタデータを設定
         *
         * @param metadata 商品のメタデータ
         * @return this
         */
        public Showcase WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }

        /** 販売期間とするイベントマスター のGRN */
        public string salesPeriodEventId { set; get; }

        /**
         * 販売期間とするイベントマスター のGRNを設定
         *
         * @param salesPeriodEventId 販売期間とするイベントマスター のGRN
         * @return this
         */
        public Showcase WithSalesPeriodEventId(string salesPeriodEventId) {
            this.salesPeriodEventId = salesPeriodEventId;
            return this;
        }

        /** インベントリに格納可能なアイテムモデル一覧 */
        public List<DisplayItem> displayItems { set; get; }

        /**
         * インベントリに格納可能なアイテムモデル一覧を設定
         *
         * @param displayItems インベントリに格納可能なアイテムモデル一覧
         * @return this
         */
        public Showcase WithDisplayItems(List<DisplayItem> displayItems) {
            this.displayItems = displayItems;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.showcaseId != null)
            {
                writer.WritePropertyName("showcaseId");
                writer.Write(this.showcaseId);
            }
            if(this.name != null)
            {
                writer.WritePropertyName("name");
                writer.Write(this.name);
            }
            if(this.metadata != null)
            {
                writer.WritePropertyName("metadata");
                writer.Write(this.metadata);
            }
            if(this.salesPeriodEventId != null)
            {
                writer.WritePropertyName("salesPeriodEventId");
                writer.Write(this.salesPeriodEventId);
            }
            if(this.displayItems != null)
            {
                writer.WritePropertyName("displayItems");
                writer.WriteArrayStart();
                foreach(var item in this.displayItems)
                {
                    item.WriteJson(writer);
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

    public static string GetShowcaseNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):showcase:(?<namespaceName>.*):showcase:(?<showcaseName>.*)");
        if (!match.Groups["showcaseName"].Success)
        {
            return null;
        }
        return match.Groups["showcaseName"].Value;
    }

    public static string GetNamespaceNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):showcase:(?<namespaceName>.*):showcase:(?<showcaseName>.*)");
        if (!match.Groups["namespaceName"].Success)
        {
            return null;
        }
        return match.Groups["namespaceName"].Value;
    }

    public static string GetOwnerIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):showcase:(?<namespaceName>.*):showcase:(?<showcaseName>.*)");
        if (!match.Groups["ownerId"].Success)
        {
            return null;
        }
        return match.Groups["ownerId"].Value;
    }

    public static string GetRegionFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):showcase:(?<namespaceName>.*):showcase:(?<showcaseName>.*)");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static Showcase FromDict(JsonData data)
        {
            return new Showcase()
                .WithShowcaseId(data.Keys.Contains("showcaseId") && data["showcaseId"] != null ? data["showcaseId"].ToString() : null)
                .WithName(data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString() : null)
                .WithMetadata(data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString() : null)
                .WithSalesPeriodEventId(data.Keys.Contains("salesPeriodEventId") && data["salesPeriodEventId"] != null ? data["salesPeriodEventId"].ToString() : null)
                .WithDisplayItems(data.Keys.Contains("displayItems") && data["displayItems"] != null ? data["displayItems"].Cast<JsonData>().Select(value =>
                    {
                        return Gs2.Gs2Showcase.Model.DisplayItem.FromDict(value);
                    }
                ).ToList() : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Showcase;
            var diff = 0;
            if (showcaseId == null && showcaseId == other.showcaseId)
            {
                // null and null
            }
            else
            {
                diff += showcaseId.CompareTo(other.showcaseId);
            }
            if (name == null && name == other.name)
            {
                // null and null
            }
            else
            {
                diff += name.CompareTo(other.name);
            }
            if (metadata == null && metadata == other.metadata)
            {
                // null and null
            }
            else
            {
                diff += metadata.CompareTo(other.metadata);
            }
            if (salesPeriodEventId == null && salesPeriodEventId == other.salesPeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += salesPeriodEventId.CompareTo(other.salesPeriodEventId);
            }
            if (displayItems == null && displayItems == other.displayItems)
            {
                // null and null
            }
            else
            {
                diff += displayItems.Count - other.displayItems.Count;
                for (var i = 0; i < displayItems.Count; i++)
                {
                    diff += displayItems[i].CompareTo(other.displayItems[i]);
                }
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["showcaseId"] = showcaseId;
            data["name"] = name;
            data["metadata"] = metadata;
            data["salesPeriodEventId"] = salesPeriodEventId;
            data["displayItems"] = new JsonData(displayItems.Select(item => item.ToDict()));
            return data;
        }
	}
}