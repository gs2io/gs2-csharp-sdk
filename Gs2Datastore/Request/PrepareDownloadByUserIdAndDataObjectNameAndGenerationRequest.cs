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

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Datastore.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest : Gs2Request<PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest>
	{
         public string NamespaceName { set; get; }
         public string UserId { set; get; }
         public string DataObjectName { set; get; }
         public string Generation { set; get; }
         public string TimeOffsetToken { set; get; }
        public string DuplicationAvoider { set; get; }
        public PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest WithDataObjectName(string dataObjectName) {
            this.DataObjectName = dataObjectName;
            return this;
        }
        public PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest WithGeneration(string generation) {
            this.Generation = generation;
            return this;
        }
        public PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PrepareDownloadByUserIdAndDataObjectNameAndGenerationRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithDataObjectName(!data.Keys.Contains("dataObjectName") || data["dataObjectName"] == null ? null : data["dataObjectName"].ToString())
                .WithGeneration(!data.Keys.Contains("generation") || data["generation"] == null ? null : data["generation"].ToString())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["dataObjectName"] = DataObjectName,
                ["generation"] = Generation,
                ["timeOffsetToken"] = TimeOffsetToken,
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
            if (DataObjectName != null) {
                writer.WritePropertyName("dataObjectName");
                writer.Write(DataObjectName.ToString());
            }
            if (Generation != null) {
                writer.WritePropertyName("generation");
                writer.Write(Generation.ToString());
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += DataObjectName + ":";
            key += Generation + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}