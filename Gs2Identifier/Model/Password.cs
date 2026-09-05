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

namespace Gs2.Gs2Identifier.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Password : IComparable
	{
        public string PasswordId { set; get; }
        public string UserId { set; get; }
        public string UserName { set; get; }
        public string EnableTwoFactorAuthentication { set; get; }
        public Gs2.Gs2Identifier.Model.TwoFactorAuthenticationSetting TwoFactorAuthenticationSetting { set; get; }
        public long? CreatedAt { set; get; }
        public long? Revision { set; get; }
        public Password WithPasswordId(string passwordId) {
            this.PasswordId = passwordId;
            return this;
        }
        public Password WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Password WithUserName(string userName) {
            this.UserName = userName;
            return this;
        }
        public Password WithEnableTwoFactorAuthentication(string enableTwoFactorAuthentication) {
            this.EnableTwoFactorAuthentication = enableTwoFactorAuthentication;
            return this;
        }
        public Password WithTwoFactorAuthenticationSetting(Gs2.Gs2Identifier.Model.TwoFactorAuthenticationSetting twoFactorAuthenticationSetting) {
            this.TwoFactorAuthenticationSetting = twoFactorAuthenticationSetting;
            return this;
        }
        public Password WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Password WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2::(?<ownerId>.+):identifier:user:(?<userName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _userNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2::(?<ownerId>.+):identifier:user:(?<userName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserNameFromGrn(
            string grn
        )
        {
            var match = _userNameRegex.Match(grn);
            if (!match.Success || !match.Groups["userName"].Success)
            {
                return null;
            }
            return match.Groups["userName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Password FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Password()
                .WithPasswordId(!data.Keys.Contains("passwordId") || data["passwordId"] == null ? null : data["passwordId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithUserName(!data.Keys.Contains("userName") || data["userName"] == null ? null : data["userName"].ToString())
                .WithEnableTwoFactorAuthentication(!data.Keys.Contains("enableTwoFactorAuthentication") || data["enableTwoFactorAuthentication"] == null ? null : data["enableTwoFactorAuthentication"].ToString())
                .WithTwoFactorAuthenticationSetting(!data.Keys.Contains("twoFactorAuthenticationSetting") || data["twoFactorAuthenticationSetting"] == null ? null : Gs2.Gs2Identifier.Model.TwoFactorAuthenticationSetting.FromJson(data["twoFactorAuthenticationSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["passwordId"] = PasswordId,
                ["userId"] = UserId,
                ["userName"] = UserName,
                ["enableTwoFactorAuthentication"] = EnableTwoFactorAuthentication,
                ["twoFactorAuthenticationSetting"] = TwoFactorAuthenticationSetting?.ToJson(),
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (PasswordId != null) {
                writer.WritePropertyName("passwordId");
                writer.Write(PasswordId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (UserName != null) {
                writer.WritePropertyName("userName");
                writer.Write(UserName.ToString());
            }
            if (EnableTwoFactorAuthentication != null) {
                writer.WritePropertyName("enableTwoFactorAuthentication");
                writer.Write(EnableTwoFactorAuthentication.ToString());
            }
            if (TwoFactorAuthenticationSetting != null) {
                writer.WritePropertyName("twoFactorAuthenticationSetting");
                TwoFactorAuthenticationSetting.WriteJson(writer);
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Password;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Password.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(PasswordId, other.PasswordId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UserName, other.UserName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(EnableTwoFactorAuthentication, other.EnableTwoFactorAuthentication);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TwoFactorAuthenticationSetting, other.TwoFactorAuthenticationSetting);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (PasswordId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("password", "identifier.password.passwordId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("password", "identifier.password.userId.error.tooLong"),
                    });
                }
            }
            {
                if (UserName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("password", "identifier.password.userName.error.tooLong"),
                    });
                }
            }
            {
                switch (EnableTwoFactorAuthentication) {
                    case "RFC6238":
                    case "Disable":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("password", "identifier.password.enableTwoFactorAuthentication.error.invalid"),
                        });
                }
            }
            {
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("password", "identifier.password.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("password", "identifier.password.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("password", "identifier.password.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("password", "identifier.password.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Password {
                PasswordId = PasswordId,
                UserId = UserId,
                UserName = UserName,
                EnableTwoFactorAuthentication = EnableTwoFactorAuthentication,
                TwoFactorAuthenticationSetting = TwoFactorAuthenticationSetting?.Clone() as Gs2.Gs2Identifier.Model.TwoFactorAuthenticationSetting,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}