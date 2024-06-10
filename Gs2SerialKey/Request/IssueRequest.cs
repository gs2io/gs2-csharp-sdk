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
	public class IssueRequest : Gs2Request<IssueRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string CampaignModelName { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public int? IssueRequestCount { set; get; } = null!;
        public IssueRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public IssueRequest WithCampaignModelName(string campaignModelName) {
            this.CampaignModelName = campaignModelName;
            return this;
        }
        public IssueRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public IssueRequest WithIssueRequestCount(int? issueRequestCount) {
            this.IssueRequestCount = issueRequestCount;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static IssueRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new IssueRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithCampaignModelName(!data.Keys.Contains("campaignModelName") || data["campaignModelName"] == null ? null : data["campaignModelName"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithIssueRequestCount(!data.Keys.Contains("issueRequestCount") || data["issueRequestCount"] == null ? null : (int?)(data["issueRequestCount"].ToString().Contains(".") ? (int)double.Parse(data["issueRequestCount"].ToString()) : int.Parse(data["issueRequestCount"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["campaignModelName"] = CampaignModelName,
                ["metadata"] = Metadata,
                ["issueRequestCount"] = IssueRequestCount,
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
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (IssueRequestCount != null) {
                writer.WritePropertyName("issueRequestCount");
                writer.Write((IssueRequestCount.ToString().Contains(".") ? (int)double.Parse(IssueRequestCount.ToString()) : int.Parse(IssueRequestCount.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += CampaignModelName + ":";
            key += Metadata + ":";
            key += IssueRequestCount + ":";
            return key;
        }
    }
}