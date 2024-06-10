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
	public class VerifyRequest : Gs2Request<VerifyRequest>
	{
         public string VerifyToken { set; get; } = null!;
        public VerifyRequest WithVerifyToken(string verifyToken) {
            this.VerifyToken = verifyToken;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyRequest()
                .WithVerifyToken(!data.Keys.Contains("verifyToken") || data["verifyToken"] == null ? null : data["verifyToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["verifyToken"] = VerifyToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (VerifyToken != null) {
                writer.WritePropertyName("verifyToken");
                writer.Write(VerifyToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += VerifyToken + ":";
            return key;
        }
    }
}