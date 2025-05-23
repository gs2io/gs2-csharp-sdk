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
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Auth.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class IssueTimeOffsetTokenByUserIdResult : IResult
	{
        public string Token { set; get; }
        public string UserId { set; get; }
        public long? Expire { set; get; }
        public ResultMetadata Metadata { set; get; }

        public IssueTimeOffsetTokenByUserIdResult WithToken(string token) {
            this.Token = token;
            return this;
        }

        public IssueTimeOffsetTokenByUserIdResult WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public IssueTimeOffsetTokenByUserIdResult WithExpire(long? expire) {
            this.Expire = expire;
            return this;
        }

        public IssueTimeOffsetTokenByUserIdResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static IssueTimeOffsetTokenByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new IssueTimeOffsetTokenByUserIdResult()
                .WithToken(!data.Keys.Contains("token") || data["token"] == null ? null : data["token"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithExpire(!data.Keys.Contains("expire") || data["expire"] == null ? null : (long?)(data["expire"].ToString().Contains(".") ? (long)double.Parse(data["expire"].ToString()) : long.Parse(data["expire"].ToString())))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["token"] = Token,
                ["userId"] = UserId,
                ["expire"] = Expire,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Token != null) {
                writer.WritePropertyName("token");
                writer.Write(Token.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Expire != null) {
                writer.WritePropertyName("expire");
                writer.Write((Expire.ToString().Contains(".") ? (long)double.Parse(Expire.ToString()) : long.Parse(Expire.ToString())));
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}