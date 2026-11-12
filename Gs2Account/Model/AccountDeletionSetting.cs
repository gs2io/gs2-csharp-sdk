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

namespace Gs2.Gs2Account.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class AccountDeletionSetting : IComparable
	{
        public int? GracePeriodMinutes { set; get; }
        public string LoginDuringGracePeriod { set; get; }
        public string CleanUserDataScope { set; get; }
        public AccountDeletionSetting WithGracePeriodMinutes(int? gracePeriodMinutes) {
            this.GracePeriodMinutes = gracePeriodMinutes;
            return this;
        }
        public AccountDeletionSetting WithLoginDuringGracePeriod(string loginDuringGracePeriod) {
            this.LoginDuringGracePeriod = loginDuringGracePeriod;
            return this;
        }
        public AccountDeletionSetting WithCleanUserDataScope(string cleanUserDataScope) {
            this.CleanUserDataScope = cleanUserDataScope;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AccountDeletionSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new AccountDeletionSetting()
                .WithGracePeriodMinutes(!data.Keys.Contains("gracePeriodMinutes") || data["gracePeriodMinutes"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["gracePeriodMinutes"].ToString()))
                .WithLoginDuringGracePeriod(!data.Keys.Contains("loginDuringGracePeriod") || data["loginDuringGracePeriod"] == null ? null : data["loginDuringGracePeriod"].ToString())
                .WithCleanUserDataScope(!data.Keys.Contains("cleanUserDataScope") || data["cleanUserDataScope"] == null ? null : data["cleanUserDataScope"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["gracePeriodMinutes"] = GracePeriodMinutes,
                ["loginDuringGracePeriod"] = LoginDuringGracePeriod,
                ["cleanUserDataScope"] = CleanUserDataScope,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (GracePeriodMinutes != null) {
                writer.WritePropertyName("gracePeriodMinutes");
                writer.Write((GracePeriodMinutes.ToString().Contains(".") ? (int)double.Parse(GracePeriodMinutes.ToString()) : int.Parse(GracePeriodMinutes.ToString())));
            }
            if (LoginDuringGracePeriod != null) {
                writer.WritePropertyName("loginDuringGracePeriod");
                writer.Write(LoginDuringGracePeriod.ToString());
            }
            if (CleanUserDataScope != null) {
                writer.WritePropertyName("cleanUserDataScope");
                writer.Write(CleanUserDataScope.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AccountDeletionSetting;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type AccountDeletionSetting.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(GracePeriodMinutes, other.GracePeriodMinutes);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(LoginDuringGracePeriod, other.LoginDuringGracePeriod);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CleanUserDataScope, other.CleanUserDataScope);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (GracePeriodMinutes < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accountDeletionSetting", "account.accountDeletionSetting.gracePeriodMinutes.error.invalid"),
                    });
                }
                if (GracePeriodMinutes > 131400) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("accountDeletionSetting", "account.accountDeletionSetting.gracePeriodMinutes.error.invalid"),
                    });
                }
            }
            {
                switch (LoginDuringGracePeriod) {
                    case "reject":
                    case "cancel":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("accountDeletionSetting", "account.accountDeletionSetting.loginDuringGracePeriod.error.invalid"),
                        });
                }
            }
            {
                switch (CleanUserDataScope) {
                    case "account":
                    case "project":
                    case "projectExceptMoney":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("accountDeletionSetting", "account.accountDeletionSetting.cleanUserDataScope.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new AccountDeletionSetting {
                GracePeriodMinutes = GracePeriodMinutes,
                LoginDuringGracePeriod = LoginDuringGracePeriod,
                CleanUserDataScope = CleanUserDataScope,
            };
        }
    }
}