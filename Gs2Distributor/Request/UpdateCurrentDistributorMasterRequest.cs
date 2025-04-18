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
using Gs2.Gs2Distributor.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Distributor.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateCurrentDistributorMasterRequest : Gs2Request<UpdateCurrentDistributorMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Settings { set; get; } = null!;
        public UpdateCurrentDistributorMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateCurrentDistributorMasterRequest WithSettings(string settings) {
            this.Settings = settings;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateCurrentDistributorMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateCurrentDistributorMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithSettings(!data.Keys.Contains("settings") || data["settings"] == null ? null : data["settings"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["settings"] = Settings,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Settings != null) {
                writer.WritePropertyName("settings");
                writer.Write(Settings.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Settings + ":";
            return key;
        }
    }
}