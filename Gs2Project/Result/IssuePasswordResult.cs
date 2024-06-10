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
using Gs2.Gs2Project.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Project.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class IssuePasswordResult : IResult
	{
        public string NewPassword { set; get; } = null!;

        public IssuePasswordResult WithNewPassword(string newPassword) {
            this.NewPassword = newPassword;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static IssuePasswordResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new IssuePasswordResult()
                .WithNewPassword(!data.Keys.Contains("newPassword") || data["newPassword"] == null ? null : data["newPassword"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["newPassword"] = NewPassword,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NewPassword != null) {
                writer.WritePropertyName("newPassword");
                writer.Write(NewPassword.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}