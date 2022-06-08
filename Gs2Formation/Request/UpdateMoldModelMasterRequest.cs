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
using Gs2.Gs2Formation.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Formation.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateMoldModelMasterRequest : Gs2Request<UpdateMoldModelMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string MoldName { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public string FormModelName { set; get; }
        public int? InitialMaxCapacity { set; get; }
        public int? MaxCapacity { set; get; }
        public UpdateMoldModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateMoldModelMasterRequest WithMoldName(string moldName) {
            this.MoldName = moldName;
            return this;
        }
        public UpdateMoldModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateMoldModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateMoldModelMasterRequest WithFormModelName(string formModelName) {
            this.FormModelName = formModelName;
            return this;
        }
        public UpdateMoldModelMasterRequest WithInitialMaxCapacity(int? initialMaxCapacity) {
            this.InitialMaxCapacity = initialMaxCapacity;
            return this;
        }
        public UpdateMoldModelMasterRequest WithMaxCapacity(int? maxCapacity) {
            this.MaxCapacity = maxCapacity;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateMoldModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateMoldModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithMoldName(!data.Keys.Contains("moldName") || data["moldName"] == null ? null : data["moldName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithFormModelName(!data.Keys.Contains("formModelName") || data["formModelName"] == null ? null : data["formModelName"].ToString())
                .WithInitialMaxCapacity(!data.Keys.Contains("initialMaxCapacity") || data["initialMaxCapacity"] == null ? null : (int?)int.Parse(data["initialMaxCapacity"].ToString()))
                .WithMaxCapacity(!data.Keys.Contains("maxCapacity") || data["maxCapacity"] == null ? null : (int?)int.Parse(data["maxCapacity"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["moldName"] = MoldName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["formModelName"] = FormModelName,
                ["initialMaxCapacity"] = InitialMaxCapacity,
                ["maxCapacity"] = MaxCapacity,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (MoldName != null) {
                writer.WritePropertyName("moldName");
                writer.Write(MoldName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (FormModelName != null) {
                writer.WritePropertyName("formModelName");
                writer.Write(FormModelName.ToString());
            }
            if (InitialMaxCapacity != null) {
                writer.WritePropertyName("initialMaxCapacity");
                writer.Write(int.Parse(InitialMaxCapacity.ToString()));
            }
            if (MaxCapacity != null) {
                writer.WritePropertyName("maxCapacity");
                writer.Write(int.Parse(MaxCapacity.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}