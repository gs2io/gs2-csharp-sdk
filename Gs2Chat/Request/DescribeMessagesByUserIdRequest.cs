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
using Gs2.Gs2Chat.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Chat.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DescribeMessagesByUserIdRequest : Gs2Request<DescribeMessagesByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string RoomName { set; get; } = null!;
         public string Password { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public long? StartAt { set; get; } = null!;
         public int? Limit { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public DescribeMessagesByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DescribeMessagesByUserIdRequest WithRoomName(string roomName) {
            this.RoomName = roomName;
            return this;
        }
        public DescribeMessagesByUserIdRequest WithPassword(string password) {
            this.Password = password;
            return this;
        }
        public DescribeMessagesByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public DescribeMessagesByUserIdRequest WithStartAt(long? startAt) {
            this.StartAt = startAt;
            return this;
        }
        public DescribeMessagesByUserIdRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }
        public DescribeMessagesByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DescribeMessagesByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DescribeMessagesByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRoomName(!data.Keys.Contains("roomName") || data["roomName"] == null ? null : data["roomName"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithStartAt(!data.Keys.Contains("startAt") || data["startAt"] == null ? null : (long?)(data["startAt"].ToString().Contains(".") ? (long)double.Parse(data["startAt"].ToString()) : long.Parse(data["startAt"].ToString())))
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)(data["limit"].ToString().Contains(".") ? (int)double.Parse(data["limit"].ToString()) : int.Parse(data["limit"].ToString())))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["roomName"] = RoomName,
                ["password"] = Password,
                ["userId"] = UserId,
                ["startAt"] = StartAt,
                ["limit"] = Limit,
                ["timeOffsetToken"] = TimeOffsetToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (RoomName != null) {
                writer.WritePropertyName("roomName");
                writer.Write(RoomName.ToString());
            }
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (StartAt != null) {
                writer.WritePropertyName("startAt");
                writer.Write((StartAt.ToString().Contains(".") ? (long)double.Parse(StartAt.ToString()) : long.Parse(StartAt.ToString())));
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write((Limit.ToString().Contains(".") ? (int)double.Parse(Limit.ToString()) : int.Parse(Limit.ToString())));
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RoomName + ":";
            key += Password + ":";
            key += UserId + ":";
            key += StartAt + ":";
            key += Limit + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}