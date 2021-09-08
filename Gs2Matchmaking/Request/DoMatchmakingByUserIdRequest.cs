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
using Gs2.Gs2Matchmaking.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Matchmaking.Request
{
	[Preserve]
	[System.Serializable]
	public class DoMatchmakingByUserIdRequest : Gs2Request<DoMatchmakingByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public Gs2.Gs2Matchmaking.Model.Player Player { set; get; }
        public string MatchmakingContextToken { set; get; }

        public DoMatchmakingByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public DoMatchmakingByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public DoMatchmakingByUserIdRequest WithPlayer(Gs2.Gs2Matchmaking.Model.Player player) {
            this.Player = player;
            return this;
        }

        public DoMatchmakingByUserIdRequest WithMatchmakingContextToken(string matchmakingContextToken) {
            this.MatchmakingContextToken = matchmakingContextToken;
            return this;
        }

    	[Preserve]
        public static DoMatchmakingByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DoMatchmakingByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithPlayer(!data.Keys.Contains("player") || data["player"] == null ? null : Gs2.Gs2Matchmaking.Model.Player.FromJson(data["player"]))
                .WithMatchmakingContextToken(!data.Keys.Contains("matchmakingContextToken") || data["matchmakingContextToken"] == null ? null : data["matchmakingContextToken"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["player"] = Player?.ToJson(),
                ["matchmakingContextToken"] = MatchmakingContextToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Player != null) {
                Player.WriteJson(writer);
            }
            if (MatchmakingContextToken != null) {
                writer.WritePropertyName("matchmakingContextToken");
                writer.Write(MatchmakingContextToken.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}