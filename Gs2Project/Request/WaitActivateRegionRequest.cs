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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Project.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Project.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class WaitActivateRegionRequest : Gs2Request<WaitActivateRegionRequest>
	{
         public string OwnerId { set; get; } = null!;
         public string ProjectName { set; get; } = null!;
         public string RegionName { set; get; } = null!;
        public WaitActivateRegionRequest WithOwnerId(string ownerId) {
            this.OwnerId = ownerId;
            return this;
        }
        public WaitActivateRegionRequest WithProjectName(string projectName) {
            this.ProjectName = projectName;
            return this;
        }
        public WaitActivateRegionRequest WithRegionName(string regionName) {
            this.RegionName = regionName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static WaitActivateRegionRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new WaitActivateRegionRequest()
                .WithOwnerId(!data.Keys.Contains("ownerId") || data["ownerId"] == null ? null : data["ownerId"].ToString())
                .WithProjectName(!data.Keys.Contains("projectName") || data["projectName"] == null ? null : data["projectName"].ToString())
                .WithRegionName(!data.Keys.Contains("regionName") || data["regionName"] == null ? null : data["regionName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["ownerId"] = OwnerId,
                ["projectName"] = ProjectName,
                ["regionName"] = RegionName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (OwnerId != null) {
                writer.WritePropertyName("ownerId");
                writer.Write(OwnerId.ToString());
            }
            if (ProjectName != null) {
                writer.WritePropertyName("projectName");
                writer.Write(ProjectName.ToString());
            }
            if (RegionName != null) {
                writer.WritePropertyName("regionName");
                writer.Write(RegionName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += OwnerId + ":";
            key += ProjectName + ":";
            key += RegionName + ":";
            return key;
        }
    }
}