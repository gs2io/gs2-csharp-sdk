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
using Gs2.Gs2Distributor.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Distributor.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateDistributorModelMasterRequest : Gs2Request<UpdateDistributorModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string DistributorName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string InboxNamespaceId { set; get; } = null!;
         public string[] WhiteListTargetIds { set; get; } = null!;
        public UpdateDistributorModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateDistributorModelMasterRequest WithDistributorName(string distributorName) {
            this.DistributorName = distributorName;
            return this;
        }
        public UpdateDistributorModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateDistributorModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateDistributorModelMasterRequest WithInboxNamespaceId(string inboxNamespaceId) {
            this.InboxNamespaceId = inboxNamespaceId;
            return this;
        }
        public UpdateDistributorModelMasterRequest WithWhiteListTargetIds(string[] whiteListTargetIds) {
            this.WhiteListTargetIds = whiteListTargetIds;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateDistributorModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateDistributorModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithDistributorName(!data.Keys.Contains("distributorName") || data["distributorName"] == null ? null : data["distributorName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithInboxNamespaceId(!data.Keys.Contains("inboxNamespaceId") || data["inboxNamespaceId"] == null ? null : data["inboxNamespaceId"].ToString())
                .WithWhiteListTargetIds(!data.Keys.Contains("whiteListTargetIds") || data["whiteListTargetIds"] == null || !data["whiteListTargetIds"].IsArray ? null : data["whiteListTargetIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData whiteListTargetIdsJsonData = null;
            if (WhiteListTargetIds != null && WhiteListTargetIds.Length > 0)
            {
                whiteListTargetIdsJsonData = new JsonData();
                foreach (var whiteListTargetId in WhiteListTargetIds)
                {
                    whiteListTargetIdsJsonData.Add(whiteListTargetId);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["distributorName"] = DistributorName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["inboxNamespaceId"] = InboxNamespaceId,
                ["whiteListTargetIds"] = whiteListTargetIdsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (DistributorName != null) {
                writer.WritePropertyName("distributorName");
                writer.Write(DistributorName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (InboxNamespaceId != null) {
                writer.WritePropertyName("inboxNamespaceId");
                writer.Write(InboxNamespaceId.ToString());
            }
            if (WhiteListTargetIds != null) {
                writer.WritePropertyName("whiteListTargetIds");
                writer.WriteArrayStart();
                foreach (var whiteListTargetId in WhiteListTargetIds)
                {
                    writer.Write(whiteListTargetId.ToString());
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += DistributorName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += InboxNamespaceId + ":";
            key += WhiteListTargetIds + ":";
            return key;
        }
    }
}