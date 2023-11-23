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
using Gs2.Gs2SerialKey.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2SerialKey.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateCampaignModelMasterRequest : Gs2Request<UpdateCampaignModelMasterRequest>
	{
         public string NamespaceName { set; get; }
         public string CampaignModelName { set; get; }
         public string Description { set; get; }
         public string Metadata { set; get; }
         public bool? EnableCampaignCode { set; get; }
        public UpdateCampaignModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateCampaignModelMasterRequest WithCampaignModelName(string campaignModelName) {
            this.CampaignModelName = campaignModelName;
            return this;
        }
        public UpdateCampaignModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateCampaignModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateCampaignModelMasterRequest WithEnableCampaignCode(bool? enableCampaignCode) {
            this.EnableCampaignCode = enableCampaignCode;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateCampaignModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateCampaignModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithCampaignModelName(!data.Keys.Contains("campaignModelName") || data["campaignModelName"] == null ? null : data["campaignModelName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithEnableCampaignCode(!data.Keys.Contains("enableCampaignCode") || data["enableCampaignCode"] == null ? null : (bool?)bool.Parse(data["enableCampaignCode"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["campaignModelName"] = CampaignModelName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["enableCampaignCode"] = EnableCampaignCode,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (CampaignModelName != null) {
                writer.WritePropertyName("campaignModelName");
                writer.Write(CampaignModelName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (EnableCampaignCode != null) {
                writer.WritePropertyName("enableCampaignCode");
                writer.Write(bool.Parse(EnableCampaignCode.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += CampaignModelName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += EnableCampaignCode + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply UpdateCampaignModelMasterRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (UpdateCampaignModelMasterRequest)x;
            return this;
        }
    }
}