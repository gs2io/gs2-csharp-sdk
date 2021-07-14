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
using Gs2.Gs2Datastore.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Datastore.Request
{
	[Preserve]
	[System.Serializable]
	public class PrepareDownloadByGenerationAndUserIdRequest : Gs2Request<PrepareDownloadByGenerationAndUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string DataObjectId { set; get; }
        public string Generation { set; get; }

        public PrepareDownloadByGenerationAndUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public PrepareDownloadByGenerationAndUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public PrepareDownloadByGenerationAndUserIdRequest WithDataObjectId(string dataObjectId) {
            this.DataObjectId = dataObjectId;
            return this;
        }

        public PrepareDownloadByGenerationAndUserIdRequest WithGeneration(string generation) {
            this.Generation = generation;
            return this;
        }

    	[Preserve]
        public static PrepareDownloadByGenerationAndUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PrepareDownloadByGenerationAndUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithDataObjectId(!data.Keys.Contains("dataObjectId") || data["dataObjectId"] == null ? null : data["dataObjectId"].ToString())
                .WithGeneration(!data.Keys.Contains("generation") || data["generation"] == null ? null : data["generation"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["dataObjectId"] = DataObjectId,
                ["generation"] = Generation,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (DataObjectId != null) {
                writer.WritePropertyName("dataObjectId");
                writer.Write(DataObjectId.ToString());
            }
            if (Generation != null) {
                writer.WritePropertyName("generation");
                writer.Write(Generation.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}