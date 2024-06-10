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
using Gs2.Gs2Version.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Version.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class AcceptByUserIdRequest : Gs2Request<AcceptByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string VersionName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public Gs2.Gs2Version.Model.Version_ Version { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public AcceptByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public AcceptByUserIdRequest WithVersionName(string versionName) {
            this.VersionName = versionName;
            return this;
        }
        public AcceptByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public AcceptByUserIdRequest WithVersion(Gs2.Gs2Version.Model.Version_ version) {
            this.Version = version;
            return this;
        }
        public AcceptByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public AcceptByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AcceptByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcceptByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithVersionName(!data.Keys.Contains("versionName") || data["versionName"] == null ? null : data["versionName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithVersion(!data.Keys.Contains("version") || data["version"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["version"]))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["versionName"] = VersionName,
                ["userId"] = UserId,
                ["version"] = Version?.ToJson(),
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
            if (VersionName != null) {
                writer.WritePropertyName("versionName");
                writer.Write(VersionName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Version != null) {
                Version.WriteJson(writer);
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
            key += VersionName + ":";
            key += UserId + ":";
            key += Version + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}