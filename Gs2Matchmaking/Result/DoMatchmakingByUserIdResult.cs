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

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Matchmaking.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DoMatchmakingByUserIdResult : IResult
	{
        public Gs2.Gs2Matchmaking.Model.Gathering Item { set; get; }
        public string MatchmakingContextToken { set; get; }
        public ResultMetadata Metadata { set; get; }

        public DoMatchmakingByUserIdResult WithItem(Gs2.Gs2Matchmaking.Model.Gathering item) {
            this.Item = item;
            return this;
        }

        public DoMatchmakingByUserIdResult WithMatchmakingContextToken(string matchmakingContextToken) {
            this.MatchmakingContextToken = matchmakingContextToken;
            return this;
        }

        public DoMatchmakingByUserIdResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DoMatchmakingByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DoMatchmakingByUserIdResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Matchmaking.Model.Gathering.FromJson(data["item"]))
                .WithMatchmakingContextToken(!data.Keys.Contains("matchmakingContextToken") || data["matchmakingContextToken"] == null ? null : data["matchmakingContextToken"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["matchmakingContextToken"] = MatchmakingContextToken,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (MatchmakingContextToken != null) {
                writer.WritePropertyName("matchmakingContextToken");
                writer.Write(MatchmakingContextToken.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}