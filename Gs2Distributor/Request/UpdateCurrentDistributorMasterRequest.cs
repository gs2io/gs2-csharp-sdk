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
         public string Mode { set; get; } = null!;
         public string Settings { set; get; } = null!;
         public string UploadToken { set; get; } = null!;
        public UpdateCurrentDistributorMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateCurrentDistributorMasterRequest WithMode(string mode) {
            this.Mode = mode;
            return this;
        }
        public UpdateCurrentDistributorMasterRequest WithSettings(string settings) {
            this.Settings = settings;
            return this;
        }
        public UpdateCurrentDistributorMasterRequest WithUploadToken(string uploadToken) {
            this.UploadToken = uploadToken;
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
                .WithMode(!data.Keys.Contains("mode") || data["mode"] == null ? null : data["mode"].ToString())
                .WithSettings(!data.Keys.Contains("settings") || data["settings"] == null ? null : data["settings"].ToString())
                .WithUploadToken(!data.Keys.Contains("uploadToken") || data["uploadToken"] == null ? null : data["uploadToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["mode"] = Mode,
                ["settings"] = Settings,
                ["uploadToken"] = UploadToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Mode != null) {
                writer.WritePropertyName("mode");
                writer.Write(Mode.ToString());
            }
            if (Settings != null) {
                writer.WritePropertyName("settings");
                writer.Write(Settings.ToString());
            }
            if (UploadToken != null) {
                writer.WritePropertyName("uploadToken");
                writer.Write(UploadToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Mode + ":";
            key += Settings + ":";
            key += UploadToken + ":";
            return key;
        }
    }
}