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
using Gs2.Gs2Identifier.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Identifier.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ChallengeMfaRequest : Gs2Request<ChallengeMfaRequest>
	{
         public string UserName { set; get; } = null!;
         public string Passcode { set; get; } = null!;
        public ChallengeMfaRequest WithUserName(string userName) {
            this.UserName = userName;
            return this;
        }
        public ChallengeMfaRequest WithPasscode(string passcode) {
            this.Passcode = passcode;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ChallengeMfaRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ChallengeMfaRequest()
                .WithUserName(!data.Keys.Contains("userName") || data["userName"] == null ? null : data["userName"].ToString())
                .WithPasscode(!data.Keys.Contains("passcode") || data["passcode"] == null ? null : data["passcode"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["userName"] = UserName,
                ["passcode"] = Passcode,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserName != null) {
                writer.WritePropertyName("userName");
                writer.Write(UserName.ToString());
            }
            if (Passcode != null) {
                writer.WritePropertyName("passcode");
                writer.Write(Passcode.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += UserName + ":";
            key += Passcode + ":";
            return key;
        }
    }
}