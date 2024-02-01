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
using Gs2.Gs2Script.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Script.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DebugInvokeRequest : Gs2Request<DebugInvokeRequest>
	{
         public string Script { set; get; }
         public string Args { set; get; }
         public Gs2.Gs2Script.Model.RandomStatus RandomStatus { set; get; }
        public DebugInvokeRequest WithScript(string script) {
            this.Script = script;
            return this;
        }
        public DebugInvokeRequest WithArgs(string args) {
            this.Args = args;
            return this;
        }
        public DebugInvokeRequest WithRandomStatus(Gs2.Gs2Script.Model.RandomStatus randomStatus) {
            this.RandomStatus = randomStatus;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DebugInvokeRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DebugInvokeRequest()
                .WithScript(!data.Keys.Contains("script") || data["script"] == null ? null : data["script"].ToString())
                .WithArgs(!data.Keys.Contains("args") || data["args"] == null ? null : data["args"].ToString())
                .WithRandomStatus(!data.Keys.Contains("randomStatus") || data["randomStatus"] == null ? null : Gs2.Gs2Script.Model.RandomStatus.FromJson(data["randomStatus"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["script"] = Script,
                ["args"] = Args,
                ["randomStatus"] = RandomStatus?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Script != null) {
                writer.WritePropertyName("script");
                writer.Write(Script.ToString());
            }
            if (Args != null) {
                writer.WritePropertyName("args");
                writer.Write(Args.ToString());
            }
            if (RandomStatus != null) {
                RandomStatus.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += Script + ":";
            key += Args + ":";
            key += RandomStatus + ":";
            return key;
        }
    }
}