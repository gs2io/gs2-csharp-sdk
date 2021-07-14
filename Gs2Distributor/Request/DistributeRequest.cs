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
using UnityEngine.Scripting;

namespace Gs2.Gs2Distributor.Request
{
	[Preserve]
	[System.Serializable]
	public class DistributeRequest : Gs2Request<DistributeRequest>
	{
        public string NamespaceName { set; get; }
        public string DistributorName { set; get; }
        public string UserId { set; get; }
        public Gs2.Gs2Distributor.Model.DistributeResource DistributeResource { set; get; }

        public DistributeRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public DistributeRequest WithDistributorName(string distributorName) {
            this.DistributorName = distributorName;
            return this;
        }

        public DistributeRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public DistributeRequest WithDistributeResource(Gs2.Gs2Distributor.Model.DistributeResource distributeResource) {
            this.DistributeResource = distributeResource;
            return this;
        }

    	[Preserve]
        public static DistributeRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DistributeRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithDistributorName(!data.Keys.Contains("distributorName") || data["distributorName"] == null ? null : data["distributorName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithDistributeResource(!data.Keys.Contains("distributeResource") || data["distributeResource"] == null ? null : Gs2.Gs2Distributor.Model.DistributeResource.FromJson(data["distributeResource"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["distributorName"] = DistributorName,
                ["userId"] = UserId,
                ["distributeResource"] = DistributeResource?.ToJson(),
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (DistributeResource != null) {
                DistributeResource.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}