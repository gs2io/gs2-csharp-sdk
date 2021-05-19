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
using Gs2.Gs2Mission.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Mission.Request
{
	[Preserve]
	[System.Serializable]
	public class DescribeMissionTaskModelsRequest : Gs2Request<DescribeMissionTaskModelsRequest>
	{

        /** ネームスペース名 */
		[UnityEngine.SerializeField]
        public string namespaceName;

        /**
         * ネームスペース名を設定
         *
         * @param namespaceName ネームスペース名
         * @return this
         */
        public DescribeMissionTaskModelsRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** グループ名 */
		[UnityEngine.SerializeField]
        public string missionGroupName;

        /**
         * グループ名を設定
         *
         * @param missionGroupName グループ名
         * @return this
         */
        public DescribeMissionTaskModelsRequest WithMissionGroupName(string missionGroupName) {
            this.missionGroupName = missionGroupName;
            return this;
        }


    	[Preserve]
        public static DescribeMissionTaskModelsRequest FromDict(JsonData data)
        {
            return new DescribeMissionTaskModelsRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                missionGroupName = data.Keys.Contains("missionGroupName") && data["missionGroupName"] != null ? data["missionGroupName"].ToString(): null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["missionGroupName"] = missionGroupName;
            return data;
        }
	}
}