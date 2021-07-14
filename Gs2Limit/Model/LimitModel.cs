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

namespace Gs2.Gs2Limit.Model
{

	[Preserve]
	public class LimitModel : IComparable
	{
        public string LimitModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string ResetType { set; get; }
        public int? ResetDayOfMonth { set; get; }
        public string ResetDayOfWeek { set; get; }
        public int? ResetHour { set; get; }

        public LimitModel WithLimitModelId(string limitModelId) {
            this.LimitModelId = limitModelId;
            return this;
        }

        public LimitModel WithName(string name) {
            this.Name = name;
            return this;
        }

        public LimitModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public LimitModel WithResetType(string resetType) {
            this.ResetType = resetType;
            return this;
        }

        public LimitModel WithResetDayOfMonth(int? resetDayOfMonth) {
            this.ResetDayOfMonth = resetDayOfMonth;
            return this;
        }

        public LimitModel WithResetDayOfWeek(string resetDayOfWeek) {
            this.ResetDayOfWeek = resetDayOfWeek;
            return this;
        }

        public LimitModel WithResetHour(int? resetHour) {
            this.ResetHour = resetHour;
            return this;
        }

    	[Preserve]
        public static LimitModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LimitModel()
                .WithLimitModelId(!data.Keys.Contains("limitModelId") || data["limitModelId"] == null ? null : data["limitModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithResetType(!data.Keys.Contains("resetType") || data["resetType"] == null ? null : data["resetType"].ToString())
                .WithResetDayOfMonth(!data.Keys.Contains("resetDayOfMonth") || data["resetDayOfMonth"] == null ? null : (int?)int.Parse(data["resetDayOfMonth"].ToString()))
                .WithResetDayOfWeek(!data.Keys.Contains("resetDayOfWeek") || data["resetDayOfWeek"] == null ? null : data["resetDayOfWeek"].ToString())
                .WithResetHour(!data.Keys.Contains("resetHour") || data["resetHour"] == null ? null : (int?)int.Parse(data["resetHour"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["limitModelId"] = LimitModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["resetType"] = ResetType,
                ["resetDayOfMonth"] = ResetDayOfMonth,
                ["resetDayOfWeek"] = ResetDayOfWeek,
                ["resetHour"] = ResetHour,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (LimitModelId != null) {
                writer.WritePropertyName("limitModelId");
                writer.Write(LimitModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ResetType != null) {
                writer.WritePropertyName("resetType");
                writer.Write(ResetType.ToString());
            }
            if (ResetDayOfMonth != null) {
                writer.WritePropertyName("resetDayOfMonth");
                writer.Write(int.Parse(ResetDayOfMonth.ToString()));
            }
            if (ResetDayOfWeek != null) {
                writer.WritePropertyName("resetDayOfWeek");
                writer.Write(ResetDayOfWeek.ToString());
            }
            if (ResetHour != null) {
                writer.WritePropertyName("resetHour");
                writer.Write(int.Parse(ResetHour.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as LimitModel;
            var diff = 0;
            if (LimitModelId == null && LimitModelId == other.LimitModelId)
            {
                // null and null
            }
            else
            {
                diff += LimitModelId.CompareTo(other.LimitModelId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (ResetType == null && ResetType == other.ResetType)
            {
                // null and null
            }
            else
            {
                diff += ResetType.CompareTo(other.ResetType);
            }
            if (ResetDayOfMonth == null && ResetDayOfMonth == other.ResetDayOfMonth)
            {
                // null and null
            }
            else
            {
                diff += (int)(ResetDayOfMonth - other.ResetDayOfMonth);
            }
            if (ResetDayOfWeek == null && ResetDayOfWeek == other.ResetDayOfWeek)
            {
                // null and null
            }
            else
            {
                diff += ResetDayOfWeek.CompareTo(other.ResetDayOfWeek);
            }
            if (ResetHour == null && ResetHour == other.ResetHour)
            {
                // null and null
            }
            else
            {
                diff += (int)(ResetHour - other.ResetHour);
            }
            return diff;
        }
    }
}