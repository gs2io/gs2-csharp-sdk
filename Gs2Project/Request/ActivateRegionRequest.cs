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
	public class ActivateRegionRequest : Gs2Request<ActivateRegionRequest>
	{
        public string AccountToken { set; get; }
        public string ProjectName { set; get; }
        public string RegionName { set; get; }

        public ActivateRegionRequest WithAccountToken(string accountToken) {
            this.AccountToken = accountToken;
            return this;
        }

        public ActivateRegionRequest WithProjectName(string projectName) {
            this.ProjectName = projectName;
            return this;
        }

        public ActivateRegionRequest WithRegionName(string regionName) {
            this.RegionName = regionName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ActivateRegionRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ActivateRegionRequest()
                .WithAccountToken(!data.Keys.Contains("accountToken") || data["accountToken"] == null ? null : data["accountToken"].ToString())
                .WithProjectName(!data.Keys.Contains("projectName") || data["projectName"] == null ? null : data["projectName"].ToString())
                .WithRegionName(!data.Keys.Contains("regionName") || data["regionName"] == null ? null : data["regionName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["accountToken"] = AccountToken,
                ["projectName"] = ProjectName,
                ["regionName"] = RegionName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AccountToken != null) {
                writer.WritePropertyName("accountToken");
                writer.Write(AccountToken.ToString());
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
            key += AccountToken + ":";
            key += ProjectName + ":";
            key += RegionName + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply ActivateRegionRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (ActivateRegionRequest)x;
            return this;
        }
    }
}