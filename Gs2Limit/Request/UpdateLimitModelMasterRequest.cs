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
using Gs2.Gs2Limit.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Limit.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateLimitModelMasterRequest : Gs2Request<UpdateLimitModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string LimitName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string ResetType { set; get; } = null!;
         public int? ResetDayOfMonth { set; get; } = null!;
         public string ResetDayOfWeek { set; get; } = null!;
         public int? ResetHour { set; get; } = null!;
        public UpdateLimitModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateLimitModelMasterRequest WithLimitName(string limitName) {
            this.LimitName = limitName;
            return this;
        }
        public UpdateLimitModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateLimitModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateLimitModelMasterRequest WithResetType(string resetType) {
            this.ResetType = resetType;
            return this;
        }
        public UpdateLimitModelMasterRequest WithResetDayOfMonth(int? resetDayOfMonth) {
            this.ResetDayOfMonth = resetDayOfMonth;
            return this;
        }
        public UpdateLimitModelMasterRequest WithResetDayOfWeek(string resetDayOfWeek) {
            this.ResetDayOfWeek = resetDayOfWeek;
            return this;
        }
        public UpdateLimitModelMasterRequest WithResetHour(int? resetHour) {
            this.ResetHour = resetHour;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateLimitModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateLimitModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithLimitName(!data.Keys.Contains("limitName") || data["limitName"] == null ? null : data["limitName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithResetType(!data.Keys.Contains("resetType") || data["resetType"] == null ? null : data["resetType"].ToString())
                .WithResetDayOfMonth(!data.Keys.Contains("resetDayOfMonth") || data["resetDayOfMonth"] == null ? null : (int?)(data["resetDayOfMonth"].ToString().Contains(".") ? (int)double.Parse(data["resetDayOfMonth"].ToString()) : int.Parse(data["resetDayOfMonth"].ToString())))
                .WithResetDayOfWeek(!data.Keys.Contains("resetDayOfWeek") || data["resetDayOfWeek"] == null ? null : data["resetDayOfWeek"].ToString())
                .WithResetHour(!data.Keys.Contains("resetHour") || data["resetHour"] == null ? null : (int?)(data["resetHour"].ToString().Contains(".") ? (int)double.Parse(data["resetHour"].ToString()) : int.Parse(data["resetHour"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["limitName"] = LimitName,
                ["description"] = Description,
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
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (LimitName != null) {
                writer.WritePropertyName("limitName");
                writer.Write(LimitName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
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
                writer.Write((ResetDayOfMonth.ToString().Contains(".") ? (int)double.Parse(ResetDayOfMonth.ToString()) : int.Parse(ResetDayOfMonth.ToString())));
            }
            if (ResetDayOfWeek != null) {
                writer.WritePropertyName("resetDayOfWeek");
                writer.Write(ResetDayOfWeek.ToString());
            }
            if (ResetHour != null) {
                writer.WritePropertyName("resetHour");
                writer.Write((ResetHour.ToString().Contains(".") ? (int)double.Parse(ResetHour.ToString()) : int.Parse(ResetHour.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += LimitName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += ResetType + ":";
            key += ResetDayOfMonth + ":";
            key += ResetDayOfWeek + ":";
            key += ResetHour + ":";
            return key;
        }
    }
}