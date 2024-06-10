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
	public class DownloadSerialCodesRequest : Gs2Request<DownloadSerialCodesRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string CampaignModelName { set; get; } = null!;
         public string IssueJobName { set; get; } = null!;
        public DownloadSerialCodesRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DownloadSerialCodesRequest WithCampaignModelName(string campaignModelName) {
            this.CampaignModelName = campaignModelName;
            return this;
        }
        public DownloadSerialCodesRequest WithIssueJobName(string issueJobName) {
            this.IssueJobName = issueJobName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DownloadSerialCodesRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DownloadSerialCodesRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithCampaignModelName(!data.Keys.Contains("campaignModelName") || data["campaignModelName"] == null ? null : data["campaignModelName"].ToString())
                .WithIssueJobName(!data.Keys.Contains("issueJobName") || data["issueJobName"] == null ? null : data["issueJobName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["campaignModelName"] = CampaignModelName,
                ["issueJobName"] = IssueJobName,
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
            if (IssueJobName != null) {
                writer.WritePropertyName("issueJobName");
                writer.Write(IssueJobName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += CampaignModelName + ":";
            key += IssueJobName + ":";
            return key;
        }
    }
}