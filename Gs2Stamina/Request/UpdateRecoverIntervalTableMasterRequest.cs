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
using Gs2.Gs2Stamina.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Stamina.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateRecoverIntervalTableMasterRequest : Gs2Request<UpdateRecoverIntervalTableMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string RecoverIntervalTableName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string ExperienceModelId { set; get; } = null!;
         public int[] Values { set; get; } = null!;
        public UpdateRecoverIntervalTableMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateRecoverIntervalTableMasterRequest WithRecoverIntervalTableName(string recoverIntervalTableName) {
            this.RecoverIntervalTableName = recoverIntervalTableName;
            return this;
        }
        public UpdateRecoverIntervalTableMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateRecoverIntervalTableMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateRecoverIntervalTableMasterRequest WithExperienceModelId(string experienceModelId) {
            this.ExperienceModelId = experienceModelId;
            return this;
        }
        public UpdateRecoverIntervalTableMasterRequest WithValues(int[] values) {
            this.Values = values;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateRecoverIntervalTableMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateRecoverIntervalTableMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRecoverIntervalTableName(!data.Keys.Contains("recoverIntervalTableName") || data["recoverIntervalTableName"] == null ? null : data["recoverIntervalTableName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithExperienceModelId(!data.Keys.Contains("experienceModelId") || data["experienceModelId"] == null ? null : data["experienceModelId"].ToString())
                .WithValues(!data.Keys.Contains("values") || data["values"] == null || !data["values"].IsArray ? new int[]{} : data["values"].Cast<JsonData>().Select(v => {
                    return (v.ToString().Contains(".") ? (int)double.Parse(v.ToString()) : int.Parse(v.ToString()));
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData valuesJsonData = null;
            if (Values != null && Values.Length > 0)
            {
                valuesJsonData = new JsonData();
                foreach (var value in Values)
                {
                    valuesJsonData.Add(value);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["recoverIntervalTableName"] = RecoverIntervalTableName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["experienceModelId"] = ExperienceModelId,
                ["values"] = valuesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (RecoverIntervalTableName != null) {
                writer.WritePropertyName("recoverIntervalTableName");
                writer.Write(RecoverIntervalTableName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ExperienceModelId != null) {
                writer.WritePropertyName("experienceModelId");
                writer.Write(ExperienceModelId.ToString());
            }
            if (Values != null) {
                writer.WritePropertyName("values");
                writer.WriteArrayStart();
                foreach (var value in Values)
                {
                    writer.Write((value.ToString().Contains(".") ? (int)double.Parse(value.ToString()) : int.Parse(value.ToString())));
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RecoverIntervalTableName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += ExperienceModelId + ":";
            key += Values + ":";
            return key;
        }
    }
}