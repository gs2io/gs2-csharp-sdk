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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Ranking2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class SubscribeUser : IComparable
	{
        public string RankingName { set; get; }
        public string UserId { set; get; }
        public string TargetUserId { set; get; }
        public SubscribeUser WithRankingName(string rankingName) {
            this.RankingName = rankingName;
            return this;
        }
        public SubscribeUser WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SubscribeUser WithTargetUserId(string targetUserId) {
            this.TargetUserId = targetUserId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SubscribeUser FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SubscribeUser()
                .WithRankingName(!data.Keys.Contains("rankingName") || data["rankingName"] == null ? null : data["rankingName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTargetUserId(!data.Keys.Contains("targetUserId") || data["targetUserId"] == null ? null : data["targetUserId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["rankingName"] = RankingName,
                ["userId"] = UserId,
                ["targetUserId"] = TargetUserId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RankingName != null) {
                writer.WritePropertyName("rankingName");
                writer.Write(RankingName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (TargetUserId != null) {
                writer.WritePropertyName("targetUserId");
                writer.Write(TargetUserId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SubscribeUser;
            var diff = 0;
            if (RankingName == null && RankingName == other.RankingName)
            {
                // null and null
            }
            else
            {
                diff += RankingName.CompareTo(other.RankingName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (TargetUserId == null && TargetUserId == other.TargetUserId)
            {
                // null and null
            }
            else
            {
                diff += TargetUserId.CompareTo(other.TargetUserId);
            }
            return diff;
        }

        public void Validate() {
            {
                if (RankingName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeUser", "ranking2.subscribeUser.rankingName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeUser", "ranking2.subscribeUser.userId.error.tooLong"),
                    });
                }
            }
            {
                if (TargetUserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeUser", "ranking2.subscribeUser.targetUserId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new SubscribeUser {
                RankingName = RankingName,
                UserId = UserId,
                TargetUserId = TargetUserId,
            };
        }
    }
}