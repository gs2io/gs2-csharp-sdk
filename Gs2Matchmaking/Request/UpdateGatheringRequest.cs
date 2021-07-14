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
using Gs2.Gs2Matchmaking.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Matchmaking.Request
{
	[Preserve]
	[System.Serializable]
	public class UpdateGatheringRequest : Gs2Request<UpdateGatheringRequest>
	{
        public string NamespaceName { set; get; }
        public string GatheringName { set; get; }
        public string AccessToken { set; get; }
        public Gs2.Gs2Matchmaking.Model.AttributeRange[] AttributeRanges { set; get; }

        public UpdateGatheringRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public UpdateGatheringRequest WithGatheringName(string gatheringName) {
            this.GatheringName = gatheringName;
            return this;
        }

        public UpdateGatheringRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }

        public UpdateGatheringRequest WithAttributeRanges(Gs2.Gs2Matchmaking.Model.AttributeRange[] attributeRanges) {
            this.AttributeRanges = attributeRanges;
            return this;
        }

    	[Preserve]
        public static UpdateGatheringRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateGatheringRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithGatheringName(!data.Keys.Contains("gatheringName") || data["gatheringName"] == null ? null : data["gatheringName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithAttributeRanges(!data.Keys.Contains("attributeRanges") || data["attributeRanges"] == null ? new Gs2.Gs2Matchmaking.Model.AttributeRange[]{} : data["attributeRanges"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Matchmaking.Model.AttributeRange.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["gatheringName"] = GatheringName,
                ["accessToken"] = AccessToken,
                ["attributeRanges"] = new JsonData(AttributeRanges == null ? new JsonData[]{} :
                        AttributeRanges.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (GatheringName != null) {
                writer.WritePropertyName("gatheringName");
                writer.Write(GatheringName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            writer.WriteArrayStart();
            foreach (var attributeRange in AttributeRanges)
            {
                if (attributeRange != null) {
                    attributeRange.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}