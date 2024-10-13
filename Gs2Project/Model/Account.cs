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

namespace Gs2.Gs2Project.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Account : IComparable
	{
        public string AccountId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Email { set; get; } = null!;
        public string FullName { set; get; } = null!;
        public string CompanyName { set; get; } = null!;
        public string EnableTwoFactorAuthentication { set; get; } = null!;
        public Gs2.Gs2Project.Model.TwoFactorAuthenticationSetting TwoFactorAuthenticationSetting { set; get; } = null!;
        public string Status { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public Account WithAccountId(string accountId) {
            this.AccountId = accountId;
            return this;
        }
        public Account WithName(string name) {
            this.Name = name;
            return this;
        }
        public Account WithEmail(string email) {
            this.Email = email;
            return this;
        }
        public Account WithFullName(string fullName) {
            this.FullName = fullName;
            return this;
        }
        public Account WithCompanyName(string companyName) {
            this.CompanyName = companyName;
            return this;
        }
        public Account WithEnableTwoFactorAuthentication(string enableTwoFactorAuthentication) {
            this.EnableTwoFactorAuthentication = enableTwoFactorAuthentication;
            return this;
        }
        public Account WithTwoFactorAuthenticationSetting(Gs2.Gs2Project.Model.TwoFactorAuthenticationSetting twoFactorAuthenticationSetting) {
            this.TwoFactorAuthenticationSetting = twoFactorAuthenticationSetting;
            return this;
        }
        public Account WithStatus(string status) {
            this.Status = status;
            return this;
        }
        public Account WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Account WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _accountNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetAccountNameFromGrn(
            string grn
        )
        {
            var match = _accountNameRegex.Match(grn);
            if (!match.Success || !match.Groups["accountName"].Success)
            {
                return null;
            }
            return match.Groups["accountName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Account FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Account()
                .WithAccountId(!data.Keys.Contains("accountId") || data["accountId"] == null ? null : data["accountId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithEmail(!data.Keys.Contains("email") || data["email"] == null ? null : data["email"].ToString())
                .WithFullName(!data.Keys.Contains("fullName") || data["fullName"] == null ? null : data["fullName"].ToString())
                .WithCompanyName(!data.Keys.Contains("companyName") || data["companyName"] == null ? null : data["companyName"].ToString())
                .WithEnableTwoFactorAuthentication(!data.Keys.Contains("enableTwoFactorAuthentication") || data["enableTwoFactorAuthentication"] == null ? null : data["enableTwoFactorAuthentication"].ToString())
                .WithTwoFactorAuthenticationSetting(!data.Keys.Contains("twoFactorAuthenticationSetting") || data["twoFactorAuthenticationSetting"] == null ? null : Gs2.Gs2Project.Model.TwoFactorAuthenticationSetting.FromJson(data["twoFactorAuthenticationSetting"]))
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["accountId"] = AccountId,
                ["name"] = Name,
                ["email"] = Email,
                ["fullName"] = FullName,
                ["companyName"] = CompanyName,
                ["enableTwoFactorAuthentication"] = EnableTwoFactorAuthentication,
                ["twoFactorAuthenticationSetting"] = TwoFactorAuthenticationSetting?.ToJson(),
                ["status"] = Status,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AccountId != null) {
                writer.WritePropertyName("accountId");
                writer.Write(AccountId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Email != null) {
                writer.WritePropertyName("email");
                writer.Write(Email.ToString());
            }
            if (FullName != null) {
                writer.WritePropertyName("fullName");
                writer.Write(FullName.ToString());
            }
            if (CompanyName != null) {
                writer.WritePropertyName("companyName");
                writer.Write(CompanyName.ToString());
            }
            if (EnableTwoFactorAuthentication != null) {
                writer.WritePropertyName("enableTwoFactorAuthentication");
                writer.Write(EnableTwoFactorAuthentication.ToString());
            }
            if (TwoFactorAuthenticationSetting != null) {
                writer.WritePropertyName("twoFactorAuthenticationSetting");
                TwoFactorAuthenticationSetting.WriteJson(writer);
            }
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Account;
            var diff = 0;
            if (AccountId == null && AccountId == other.AccountId)
            {
                // null and null
            }
            else
            {
                diff += AccountId.CompareTo(other.AccountId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Email == null && Email == other.Email)
            {
                // null and null
            }
            else
            {
                diff += Email.CompareTo(other.Email);
            }
            if (FullName == null && FullName == other.FullName)
            {
                // null and null
            }
            else
            {
                diff += FullName.CompareTo(other.FullName);
            }
            if (CompanyName == null && CompanyName == other.CompanyName)
            {
                // null and null
            }
            else
            {
                diff += CompanyName.CompareTo(other.CompanyName);
            }
            if (EnableTwoFactorAuthentication == null && EnableTwoFactorAuthentication == other.EnableTwoFactorAuthentication)
            {
                // null and null
            }
            else
            {
                diff += EnableTwoFactorAuthentication.CompareTo(other.EnableTwoFactorAuthentication);
            }
            if (TwoFactorAuthenticationSetting == null && TwoFactorAuthenticationSetting == other.TwoFactorAuthenticationSetting)
            {
                // null and null
            }
            else
            {
                diff += TwoFactorAuthenticationSetting.CompareTo(other.TwoFactorAuthenticationSetting);
            }
            if (Status == null && Status == other.Status)
            {
                // null and null
            }
            else
            {
                diff += Status.CompareTo(other.Status);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            return diff;
        }

        public void Validate() {
            {
                if (AccountId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "project.account.accountId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length < 8) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "project.account.name.error.tooShort"),
                    });
                }
                if (Name.Length > 8) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "project.account.name.error.tooLong"),
                    });
                }
            }
            {
                if (Email.Length > 256) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "project.account.email.error.tooLong"),
                    });
                }
            }
            {
                if (FullName.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "project.account.fullName.error.tooLong"),
                    });
                }
            }
            {
                if (CompanyName.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "project.account.companyName.error.tooLong"),
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
                            new RequestError("account", "project.account.enableTwoFactorAuthentication.error.invalid"),
                        });
                }
            }
            {
            }
            {
                switch (Status) {
                    case "VERIFYING":
                    case "ACTIVE":
                    case "DELETED":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("account", "project.account.status.error.invalid"),
                        });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "project.account.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "project.account.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "project.account.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("account", "project.account.updatedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Account {
                AccountId = AccountId,
                Name = Name,
                Email = Email,
                FullName = FullName,
                CompanyName = CompanyName,
                EnableTwoFactorAuthentication = EnableTwoFactorAuthentication,
                TwoFactorAuthenticationSetting = TwoFactorAuthenticationSetting.Clone() as Gs2.Gs2Project.Model.TwoFactorAuthenticationSetting,
                Status = Status,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
            };
        }
    }
}