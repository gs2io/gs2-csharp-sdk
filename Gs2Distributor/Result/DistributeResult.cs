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

namespace Gs2.Gs2Distributor.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DistributeResult : IResult
	{
        public Gs2.Gs2Distributor.Model.DistributeResource DistributeResource { set; get; }
        public string InboxNamespaceId { set; get; }
        public string Result { set; get; }
        public ResultMetadata Metadata { set; get; }

        public DistributeResult WithDistributeResource(Gs2.Gs2Distributor.Model.DistributeResource distributeResource) {
            this.DistributeResource = distributeResource;
            return this;
        }

        public DistributeResult WithInboxNamespaceId(string inboxNamespaceId) {
            this.InboxNamespaceId = inboxNamespaceId;
            return this;
        }

        public DistributeResult WithResult(string result) {
            this.Result = result;
            return this;
        }

        public DistributeResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DistributeResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DistributeResult()
                .WithDistributeResource(!data.Keys.Contains("distributeResource") || data["distributeResource"] == null ? null : Gs2.Gs2Distributor.Model.DistributeResource.FromJson(data["distributeResource"]))
                .WithInboxNamespaceId(!data.Keys.Contains("inboxNamespaceId") || data["inboxNamespaceId"] == null ? null : data["inboxNamespaceId"].ToString())
                .WithResult(!data.Keys.Contains("result") || data["result"] == null ? null : data["result"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["distributeResource"] = DistributeResource?.ToJson(),
                ["inboxNamespaceId"] = InboxNamespaceId,
                ["result"] = Result,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (DistributeResource != null) {
                DistributeResource.WriteJson(writer);
            }
            if (InboxNamespaceId != null) {
                writer.WritePropertyName("inboxNamespaceId");
                writer.Write(InboxNamespaceId.ToString());
            }
            if (Result != null) {
                writer.WritePropertyName("result");
                writer.Write(Result.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}