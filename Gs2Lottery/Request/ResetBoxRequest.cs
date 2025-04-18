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
using Gs2.Gs2Lottery.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Lottery.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ResetBoxRequest : Gs2Request<ResetBoxRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string PrizeTableName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public ResetBoxRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public ResetBoxRequest WithPrizeTableName(string prizeTableName) {
            this.PrizeTableName = prizeTableName;
            return this;
        }
        public ResetBoxRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }

        public ResetBoxRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ResetBoxRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ResetBoxRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithPrizeTableName(!data.Keys.Contains("prizeTableName") || data["prizeTableName"] == null ? null : data["prizeTableName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["prizeTableName"] = PrizeTableName,
                ["accessToken"] = AccessToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (PrizeTableName != null) {
                writer.WritePropertyName("prizeTableName");
                writer.Write(PrizeTableName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += PrizeTableName + ":";
            key += AccessToken + ":";
            return key;
        }
    }
}