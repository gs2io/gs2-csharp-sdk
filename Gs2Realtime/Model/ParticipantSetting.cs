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

namespace Gs2.Gs2Realtime.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class ParticipantSetting : IComparable
	{
        public string UserId { set; get; }
        public string ScriptId { set; get; }
        public ParticipantSetting WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ParticipantSetting WithScriptId(string scriptId) {
            this.ScriptId = scriptId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ParticipantSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new ParticipantSetting()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithScriptId(!data.Keys.Contains("scriptId") || data["scriptId"] == null ? null : data["scriptId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["scriptId"] = ScriptId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ScriptId != null) {
                writer.WritePropertyName("scriptId");
                writer.Write(ScriptId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ParticipantSetting;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type ParticipantSetting.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ScriptId, other.ScriptId);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (UserId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("participantSetting", "realtime.participantSetting.userId.error.tooLong"),
                    });
                }
            }
            {
                if (ScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("participantSetting", "realtime.participantSetting.scriptId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new ParticipantSetting {
                UserId = UserId,
                ScriptId = ScriptId,
            };
        }
    }
}