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
	public class UpdateStaminaModelMasterRequest : Gs2Request<UpdateStaminaModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string StaminaName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public int? RecoverIntervalMinutes { set; get; } = null!;
         public int? RecoverValue { set; get; } = null!;
         public int? InitialCapacity { set; get; } = null!;
         public bool? IsOverflow { set; get; } = null!;
         public int? MaxCapacity { set; get; } = null!;
         public string MaxStaminaTableName { set; get; } = null!;
         public string RecoverIntervalTableName { set; get; } = null!;
         public string RecoverValueTableName { set; get; } = null!;
        public UpdateStaminaModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateStaminaModelMasterRequest WithStaminaName(string staminaName) {
            this.StaminaName = staminaName;
            return this;
        }
        public UpdateStaminaModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateStaminaModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateStaminaModelMasterRequest WithRecoverIntervalMinutes(int? recoverIntervalMinutes) {
            this.RecoverIntervalMinutes = recoverIntervalMinutes;
            return this;
        }
        public UpdateStaminaModelMasterRequest WithRecoverValue(int? recoverValue) {
            this.RecoverValue = recoverValue;
            return this;
        }
        public UpdateStaminaModelMasterRequest WithInitialCapacity(int? initialCapacity) {
            this.InitialCapacity = initialCapacity;
            return this;
        }
        public UpdateStaminaModelMasterRequest WithIsOverflow(bool? isOverflow) {
            this.IsOverflow = isOverflow;
            return this;
        }
        public UpdateStaminaModelMasterRequest WithMaxCapacity(int? maxCapacity) {
            this.MaxCapacity = maxCapacity;
            return this;
        }
        public UpdateStaminaModelMasterRequest WithMaxStaminaTableName(string maxStaminaTableName) {
            this.MaxStaminaTableName = maxStaminaTableName;
            return this;
        }
        public UpdateStaminaModelMasterRequest WithRecoverIntervalTableName(string recoverIntervalTableName) {
            this.RecoverIntervalTableName = recoverIntervalTableName;
            return this;
        }
        public UpdateStaminaModelMasterRequest WithRecoverValueTableName(string recoverValueTableName) {
            this.RecoverValueTableName = recoverValueTableName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateStaminaModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateStaminaModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithStaminaName(!data.Keys.Contains("staminaName") || data["staminaName"] == null ? null : data["staminaName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithRecoverIntervalMinutes(!data.Keys.Contains("recoverIntervalMinutes") || data["recoverIntervalMinutes"] == null ? null : (int?)(data["recoverIntervalMinutes"].ToString().Contains(".") ? (int)double.Parse(data["recoverIntervalMinutes"].ToString()) : int.Parse(data["recoverIntervalMinutes"].ToString())))
                .WithRecoverValue(!data.Keys.Contains("recoverValue") || data["recoverValue"] == null ? null : (int?)(data["recoverValue"].ToString().Contains(".") ? (int)double.Parse(data["recoverValue"].ToString()) : int.Parse(data["recoverValue"].ToString())))
                .WithInitialCapacity(!data.Keys.Contains("initialCapacity") || data["initialCapacity"] == null ? null : (int?)(data["initialCapacity"].ToString().Contains(".") ? (int)double.Parse(data["initialCapacity"].ToString()) : int.Parse(data["initialCapacity"].ToString())))
                .WithIsOverflow(!data.Keys.Contains("isOverflow") || data["isOverflow"] == null ? null : (bool?)bool.Parse(data["isOverflow"].ToString()))
                .WithMaxCapacity(!data.Keys.Contains("maxCapacity") || data["maxCapacity"] == null ? null : (int?)(data["maxCapacity"].ToString().Contains(".") ? (int)double.Parse(data["maxCapacity"].ToString()) : int.Parse(data["maxCapacity"].ToString())))
                .WithMaxStaminaTableName(!data.Keys.Contains("maxStaminaTableName") || data["maxStaminaTableName"] == null ? null : data["maxStaminaTableName"].ToString())
                .WithRecoverIntervalTableName(!data.Keys.Contains("recoverIntervalTableName") || data["recoverIntervalTableName"] == null ? null : data["recoverIntervalTableName"].ToString())
                .WithRecoverValueTableName(!data.Keys.Contains("recoverValueTableName") || data["recoverValueTableName"] == null ? null : data["recoverValueTableName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["staminaName"] = StaminaName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["recoverIntervalMinutes"] = RecoverIntervalMinutes,
                ["recoverValue"] = RecoverValue,
                ["initialCapacity"] = InitialCapacity,
                ["isOverflow"] = IsOverflow,
                ["maxCapacity"] = MaxCapacity,
                ["maxStaminaTableName"] = MaxStaminaTableName,
                ["recoverIntervalTableName"] = RecoverIntervalTableName,
                ["recoverValueTableName"] = RecoverValueTableName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (StaminaName != null) {
                writer.WritePropertyName("staminaName");
                writer.Write(StaminaName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (RecoverIntervalMinutes != null) {
                writer.WritePropertyName("recoverIntervalMinutes");
                writer.Write((RecoverIntervalMinutes.ToString().Contains(".") ? (int)double.Parse(RecoverIntervalMinutes.ToString()) : int.Parse(RecoverIntervalMinutes.ToString())));
            }
            if (RecoverValue != null) {
                writer.WritePropertyName("recoverValue");
                writer.Write((RecoverValue.ToString().Contains(".") ? (int)double.Parse(RecoverValue.ToString()) : int.Parse(RecoverValue.ToString())));
            }
            if (InitialCapacity != null) {
                writer.WritePropertyName("initialCapacity");
                writer.Write((InitialCapacity.ToString().Contains(".") ? (int)double.Parse(InitialCapacity.ToString()) : int.Parse(InitialCapacity.ToString())));
            }
            if (IsOverflow != null) {
                writer.WritePropertyName("isOverflow");
                writer.Write(bool.Parse(IsOverflow.ToString()));
            }
            if (MaxCapacity != null) {
                writer.WritePropertyName("maxCapacity");
                writer.Write((MaxCapacity.ToString().Contains(".") ? (int)double.Parse(MaxCapacity.ToString()) : int.Parse(MaxCapacity.ToString())));
            }
            if (MaxStaminaTableName != null) {
                writer.WritePropertyName("maxStaminaTableName");
                writer.Write(MaxStaminaTableName.ToString());
            }
            if (RecoverIntervalTableName != null) {
                writer.WritePropertyName("recoverIntervalTableName");
                writer.Write(RecoverIntervalTableName.ToString());
            }
            if (RecoverValueTableName != null) {
                writer.WritePropertyName("recoverValueTableName");
                writer.Write(RecoverValueTableName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += StaminaName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += RecoverIntervalMinutes + ":";
            key += RecoverValue + ":";
            key += InitialCapacity + ":";
            key += IsOverflow + ":";
            key += MaxCapacity + ":";
            key += MaxStaminaTableName + ":";
            key += RecoverIntervalTableName + ":";
            key += RecoverValueTableName + ":";
            return key;
        }
    }
}