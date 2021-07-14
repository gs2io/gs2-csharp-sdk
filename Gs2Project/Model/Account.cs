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
using UnityEngine.Scripting;

namespace Gs2.Gs2Project.Model
{

	[Preserve]
	public class Account : IComparable
	{
        public string AccountId { set; get; }
        public string OwnerId { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string FullName { set; get; }
        public string CompanyName { set; get; }
        public string Status { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Account WithAccountId(string accountId) {
            this.AccountId = accountId;
            return this;
        }

        public Account WithOwnerId(string ownerId) {
            this.OwnerId = ownerId;
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

    	[Preserve]
        public static Account FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Account()
                .WithAccountId(!data.Keys.Contains("accountId") || data["accountId"] == null ? null : data["accountId"].ToString())
                .WithOwnerId(!data.Keys.Contains("ownerId") || data["ownerId"] == null ? null : data["ownerId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithEmail(!data.Keys.Contains("email") || data["email"] == null ? null : data["email"].ToString())
                .WithFullName(!data.Keys.Contains("fullName") || data["fullName"] == null ? null : data["fullName"].ToString())
                .WithCompanyName(!data.Keys.Contains("companyName") || data["companyName"] == null ? null : data["companyName"].ToString())
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["accountId"] = AccountId,
                ["ownerId"] = OwnerId,
                ["name"] = Name,
                ["email"] = Email,
                ["fullName"] = FullName,
                ["companyName"] = CompanyName,
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
            if (OwnerId != null) {
                writer.WritePropertyName("ownerId");
                writer.Write(OwnerId.ToString());
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
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
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
            if (OwnerId == null && OwnerId == other.OwnerId)
            {
                // null and null
            }
            else
            {
                diff += OwnerId.CompareTo(other.OwnerId);
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
    }
}