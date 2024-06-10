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
using Gs2.Gs2SeasonRating.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2SeasonRating.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetBallotRequest : Gs2Request<GetBallotRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string SeasonName { set; get; } = null!;
         public string SessionName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public int? NumberOfPlayer { set; get; } = null!;
         public string KeyId { set; get; } = null!;
        public GetBallotRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public GetBallotRequest WithSeasonName(string seasonName) {
            this.SeasonName = seasonName;
            return this;
        }
        public GetBallotRequest WithSessionName(string sessionName) {
            this.SessionName = sessionName;
            return this;
        }
        public GetBallotRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public GetBallotRequest WithNumberOfPlayer(int? numberOfPlayer) {
            this.NumberOfPlayer = numberOfPlayer;
            return this;
        }
        public GetBallotRequest WithKeyId(string keyId) {
            this.KeyId = keyId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetBallotRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetBallotRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithSeasonName(!data.Keys.Contains("seasonName") || data["seasonName"] == null ? null : data["seasonName"].ToString())
                .WithSessionName(!data.Keys.Contains("sessionName") || data["sessionName"] == null ? null : data["sessionName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithNumberOfPlayer(!data.Keys.Contains("numberOfPlayer") || data["numberOfPlayer"] == null ? null : (int?)(data["numberOfPlayer"].ToString().Contains(".") ? (int)double.Parse(data["numberOfPlayer"].ToString()) : int.Parse(data["numberOfPlayer"].ToString())))
                .WithKeyId(!data.Keys.Contains("keyId") || data["keyId"] == null ? null : data["keyId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["seasonName"] = SeasonName,
                ["sessionName"] = SessionName,
                ["accessToken"] = AccessToken,
                ["numberOfPlayer"] = NumberOfPlayer,
                ["keyId"] = KeyId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (SeasonName != null) {
                writer.WritePropertyName("seasonName");
                writer.Write(SeasonName.ToString());
            }
            if (SessionName != null) {
                writer.WritePropertyName("sessionName");
                writer.Write(SessionName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (NumberOfPlayer != null) {
                writer.WritePropertyName("numberOfPlayer");
                writer.Write((NumberOfPlayer.ToString().Contains(".") ? (int)double.Parse(NumberOfPlayer.ToString()) : int.Parse(NumberOfPlayer.ToString())));
            }
            if (KeyId != null) {
                writer.WritePropertyName("keyId");
                writer.Write(KeyId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += SeasonName + ":";
            key += SessionName + ":";
            key += AccessToken + ":";
            key += NumberOfPlayer + ":";
            key += KeyId + ":";
            return key;
        }
    }
}