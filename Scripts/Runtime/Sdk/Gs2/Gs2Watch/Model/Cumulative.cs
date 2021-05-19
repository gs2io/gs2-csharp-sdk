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

namespace Gs2.Gs2Watch.Model
{
	[Preserve]
	public class Cumulative : IComparable
	{

        /** 累積値 */
        public string cumulativeId { set; get; }

        /**
         * 累積値を設定
         *
         * @param cumulativeId 累積値
         * @return this
         */
        public Cumulative WithCumulativeId(string cumulativeId) {
            this.cumulativeId = cumulativeId;
            return this;
        }

        /** オーナーID */
        public string ownerId { set; get; }

        /**
         * オーナーIDを設定
         *
         * @param ownerId オーナーID
         * @return this
         */
        public Cumulative WithOwnerId(string ownerId) {
            this.ownerId = ownerId;
            return this;
        }

        /** リソースのGRN */
        public string resourceGrn { set; get; }

        /**
         * リソースのGRNを設定
         *
         * @param resourceGrn リソースのGRN
         * @return this
         */
        public Cumulative WithResourceGrn(string resourceGrn) {
            this.resourceGrn = resourceGrn;
            return this;
        }

        /** 名前 */
        public string name { set; get; }

        /**
         * 名前を設定
         *
         * @param name 名前
         * @return this
         */
        public Cumulative WithName(string name) {
            this.name = name;
            return this;
        }

        /** 累積値 */
        public long? value { set; get; }

        /**
         * 累積値を設定
         *
         * @param value 累積値
         * @return this
         */
        public Cumulative WithValue(long? value) {
            this.value = value;
            return this;
        }

        /** 最終更新日時 */
        public long? updatedAt { set; get; }

        /**
         * 最終更新日時を設定
         *
         * @param updatedAt 最終更新日時
         * @return this
         */
        public Cumulative WithUpdatedAt(long? updatedAt) {
            this.updatedAt = updatedAt;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.cumulativeId != null)
            {
                writer.WritePropertyName("cumulativeId");
                writer.Write(this.cumulativeId);
            }
            if(this.ownerId != null)
            {
                writer.WritePropertyName("ownerId");
                writer.Write(this.ownerId);
            }
            if(this.resourceGrn != null)
            {
                writer.WritePropertyName("resourceGrn");
                writer.Write(this.resourceGrn);
            }
            if(this.name != null)
            {
                writer.WritePropertyName("name");
                writer.Write(this.name);
            }
            if(this.value.HasValue)
            {
                writer.WritePropertyName("value");
                writer.Write(this.value.Value);
            }
            if(this.updatedAt.HasValue)
            {
                writer.WritePropertyName("updatedAt");
                writer.Write(this.updatedAt.Value);
            }
            writer.WriteObjectEnd();
        }

    public static string GetNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):watch:cumulative:(?<resourceGrn>.*):(?<name>.*)");
        if (!match.Groups["name"].Success)
        {
            return null;
        }
        return match.Groups["name"].Value;
    }

    public static string GetResourceGrnFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):watch:cumulative:(?<resourceGrn>.*):(?<name>.*)");
        if (!match.Groups["resourceGrn"].Success)
        {
            return null;
        }
        return match.Groups["resourceGrn"].Value;
    }

    public static string GetOwnerIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):watch:cumulative:(?<resourceGrn>.*):(?<name>.*)");
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
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):watch:cumulative:(?<resourceGrn>.*):(?<name>.*)");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static Cumulative FromDict(JsonData data)
        {
            return new Cumulative()
                .WithCumulativeId(data.Keys.Contains("cumulativeId") && data["cumulativeId"] != null ? data["cumulativeId"].ToString() : null)
                .WithOwnerId(data.Keys.Contains("ownerId") && data["ownerId"] != null ? data["ownerId"].ToString() : null)
                .WithResourceGrn(data.Keys.Contains("resourceGrn") && data["resourceGrn"] != null ? data["resourceGrn"].ToString() : null)
                .WithName(data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString() : null)
                .WithValue(data.Keys.Contains("value") && data["value"] != null ? (long?)long.Parse(data["value"].ToString()) : null)
                .WithUpdatedAt(data.Keys.Contains("updatedAt") && data["updatedAt"] != null ? (long?)long.Parse(data["updatedAt"].ToString()) : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Cumulative;
            var diff = 0;
            if (cumulativeId == null && cumulativeId == other.cumulativeId)
            {
                // null and null
            }
            else
            {
                diff += cumulativeId.CompareTo(other.cumulativeId);
            }
            if (ownerId == null && ownerId == other.ownerId)
            {
                // null and null
            }
            else
            {
                diff += ownerId.CompareTo(other.ownerId);
            }
            if (resourceGrn == null && resourceGrn == other.resourceGrn)
            {
                // null and null
            }
            else
            {
                diff += resourceGrn.CompareTo(other.resourceGrn);
            }
            if (name == null && name == other.name)
            {
                // null and null
            }
            else
            {
                diff += name.CompareTo(other.name);
            }
            if (value == null && value == other.value)
            {
                // null and null
            }
            else
            {
                diff += (int)(value - other.value);
            }
            if (updatedAt == null && updatedAt == other.updatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(updatedAt - other.updatedAt);
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["cumulativeId"] = cumulativeId;
            data["ownerId"] = ownerId;
            data["resourceGrn"] = resourceGrn;
            data["name"] = name;
            data["value"] = value;
            data["updatedAt"] = updatedAt;
            return data;
        }
	}
}