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
using Gs2.Gs2Account.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Account.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateTakeOverByUserIdRequest : Gs2Request<UpdateTakeOverByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public int? Type { set; get; } = null!;
         public string OldPassword { set; get; } = null!;
         public string Password { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public UpdateTakeOverByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateTakeOverByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public UpdateTakeOverByUserIdRequest WithType(int? type) {
            this.Type = type;
            return this;
        }
        public UpdateTakeOverByUserIdRequest WithOldPassword(string oldPassword) {
            this.OldPassword = oldPassword;
            return this;
        }
        public UpdateTakeOverByUserIdRequest WithPassword(string password) {
            this.Password = password;
            return this;
        }
        public UpdateTakeOverByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public UpdateTakeOverByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateTakeOverByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateTakeOverByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : (int?)(data["type"].ToString().Contains(".") ? (int)double.Parse(data["type"].ToString()) : int.Parse(data["type"].ToString())))
                .WithOldPassword(!data.Keys.Contains("oldPassword") || data["oldPassword"] == null ? null : data["oldPassword"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["type"] = Type,
                ["oldPassword"] = OldPassword,
                ["password"] = Password,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write((Type.ToString().Contains(".") ? (int)double.Parse(Type.ToString()) : int.Parse(Type.ToString())));
            }
            if (OldPassword != null) {
                writer.WritePropertyName("oldPassword");
                writer.Write(OldPassword.ToString());
            }
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
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
            key += UserId + ":";
            key += Type + ":";
            key += OldPassword + ":";
            key += Password + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}