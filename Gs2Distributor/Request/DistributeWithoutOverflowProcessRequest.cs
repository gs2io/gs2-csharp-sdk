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
	public class DistributeWithoutOverflowProcessRequest : Gs2Request<DistributeWithoutOverflowProcessRequest>
	{
        public string UserId { set; get; }
        public Gs2.Gs2Distributor.Model.DistributeResource DistributeResource { set; get; }

        public DistributeWithoutOverflowProcessRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public DistributeWithoutOverflowProcessRequest WithDistributeResource(Gs2.Gs2Distributor.Model.DistributeResource distributeResource) {
            this.DistributeResource = distributeResource;
            return this;
        }

    	[Preserve]
        public static DistributeWithoutOverflowProcessRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DistributeWithoutOverflowProcessRequest()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithDistributeResource(!data.Keys.Contains("distributeResource") || data["distributeResource"] == null ? null : Gs2.Gs2Distributor.Model.DistributeResource.FromJson(data["distributeResource"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["distributeResource"] = DistributeResource?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
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