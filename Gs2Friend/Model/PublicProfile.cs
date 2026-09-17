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

namespace Gs2.Gs2Friend.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class PublicProfile : IComparable
	{
        public string UserId { set; get; }
        public string Value { set; get; }
        public PublicProfile WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public PublicProfile WithValue(string value) {
            this.Value = value;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PublicProfile FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new PublicProfile()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithValue(!data.Keys.Contains("publicProfile") || data["publicProfile"] == null ? null : data["publicProfile"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["publicProfile"] = Value,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write(Value.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as PublicProfile;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type PublicProfile.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Value, other.Value);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("publicProfile", "friend.publicProfile.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Value.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("publicProfile", "friend.publicProfile.publicProfile.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new PublicProfile {
                UserId = UserId,
                Value = Value,
            };
        }
    }
}