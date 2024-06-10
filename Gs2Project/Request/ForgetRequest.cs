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

namespace Gs2.Gs2Project.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ForgetRequest : Gs2Request<ForgetRequest>
	{
         public string Email { set; get; } = null!;
         public string Lang { set; get; } = null!;
        public ForgetRequest WithEmail(string email) {
            this.Email = email;
            return this;
        }
        public ForgetRequest WithLang(string lang) {
            this.Lang = lang;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ForgetRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ForgetRequest()
                .WithEmail(!data.Keys.Contains("email") || data["email"] == null ? null : data["email"].ToString())
                .WithLang(!data.Keys.Contains("lang") || data["lang"] == null ? null : data["lang"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["email"] = Email,
                ["lang"] = Lang,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Email != null) {
                writer.WritePropertyName("email");
                writer.Write(Email.ToString());
            }
            if (Lang != null) {
                writer.WritePropertyName("lang");
                writer.Write(Lang.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += Email + ":";
            key += Lang + ":";
            return key;
        }
    }
}