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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2SeasonRating.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Ballot : IComparable
	{
        public string UserId { set; get; } = null!;
        public string SeasonName { set; get; } = null!;
        public string SessionName { set; get; } = null!;
        public int? NumberOfPlayer { set; get; } = null!;
        public Ballot WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Ballot WithSeasonName(string seasonName) {
            this.SeasonName = seasonName;
            return this;
        }
        public Ballot WithSessionName(string sessionName) {
            this.SessionName = sessionName;
            return this;
        }
        public Ballot WithNumberOfPlayer(int? numberOfPlayer) {
            this.NumberOfPlayer = numberOfPlayer;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Ballot FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Ballot()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithSeasonName(!data.Keys.Contains("seasonName") || data["seasonName"] == null ? null : data["seasonName"].ToString())
                .WithSessionName(!data.Keys.Contains("sessionName") || data["sessionName"] == null ? null : data["sessionName"].ToString())
                .WithNumberOfPlayer(!data.Keys.Contains("numberOfPlayer") || data["numberOfPlayer"] == null ? null : (int?)(data["numberOfPlayer"].ToString().Contains(".") ? (int)double.Parse(data["numberOfPlayer"].ToString()) : int.Parse(data["numberOfPlayer"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["seasonName"] = SeasonName,
                ["sessionName"] = SessionName,
                ["numberOfPlayer"] = NumberOfPlayer,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (SeasonName != null) {
                writer.WritePropertyName("seasonName");
                writer.Write(SeasonName.ToString());
            }
            if (SessionName != null) {
                writer.WritePropertyName("sessionName");
                writer.Write(SessionName.ToString());
            }
            if (NumberOfPlayer != null) {
                writer.WritePropertyName("numberOfPlayer");
                writer.Write((NumberOfPlayer.ToString().Contains(".") ? (int)double.Parse(NumberOfPlayer.ToString()) : int.Parse(NumberOfPlayer.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Ballot;
            var diff = 0;
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (SeasonName == null && SeasonName == other.SeasonName)
            {
                // null and null
            }
            else
            {
                diff += SeasonName.CompareTo(other.SeasonName);
            }
            if (SessionName == null && SessionName == other.SessionName)
            {
                // null and null
            }
            else
            {
                diff += SessionName.CompareTo(other.SessionName);
            }
            if (NumberOfPlayer == null && NumberOfPlayer == other.NumberOfPlayer)
            {
                // null and null
            }
            else
            {
                diff += (int)(NumberOfPlayer - other.NumberOfPlayer);
            }
            return diff;
        }

        public void Validate() {
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("ballot", "seasonRating.ballot.userId.error.tooLong"),
                    });
                }
            }
            {
                if (SeasonName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("ballot", "seasonRating.ballot.seasonName.error.tooLong"),
                    });
                }
            }
            {
                if (SessionName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("ballot", "seasonRating.ballot.sessionName.error.tooLong"),
                    });
                }
            }
            {
                if (NumberOfPlayer < 2) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("ballot", "seasonRating.ballot.numberOfPlayer.error.invalid"),
                    });
                }
                if (NumberOfPlayer > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("ballot", "seasonRating.ballot.numberOfPlayer.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Ballot {
                UserId = UserId,
                SeasonName = SeasonName,
                SessionName = SessionName,
                NumberOfPlayer = NumberOfPlayer,
            };
        }
    }
}