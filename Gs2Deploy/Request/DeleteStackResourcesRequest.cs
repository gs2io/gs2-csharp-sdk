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
using Gs2.Gs2Deploy.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Deploy.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DeleteStackResourcesRequest : Gs2Request<DeleteStackResourcesRequest>
	{
         public string StackName { set; get; } = null!;
        public DeleteStackResourcesRequest WithStackName(string stackName) {
            this.StackName = stackName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DeleteStackResourcesRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DeleteStackResourcesRequest()
                .WithStackName(!data.Keys.Contains("stackName") || data["stackName"] == null ? null : data["stackName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["stackName"] = StackName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StackName != null) {
                writer.WritePropertyName("stackName");
                writer.Write(StackName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += StackName + ":";
            return key;
        }
    }
}