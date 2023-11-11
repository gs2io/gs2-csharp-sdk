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

namespace Gs2.Gs2Datastore.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class DataObject : IComparable
	{
        public string DataObjectId { set; get; }
        public string Name { set; get; }
        public string UserId { set; get; }
        public string Scope { set; get; }
        public string[] AllowUserIds { set; get; }
        public string Status { set; get; }
        public string Generation { set; get; }
        public string PreviousGeneration { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }

        public DataObject WithDataObjectId(string dataObjectId) {
            this.DataObjectId = dataObjectId;
            return this;
        }

        public DataObject WithName(string name) {
            this.Name = name;
            return this;
        }

        public DataObject WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public DataObject WithScope(string scope) {
            this.Scope = scope;
            return this;
        }

        public DataObject WithAllowUserIds(string[] allowUserIds) {
            this.AllowUserIds = allowUserIds;
            return this;
        }

        public DataObject WithStatus(string status) {
            this.Status = status;
            return this;
        }

        public DataObject WithGeneration(string generation) {
            this.Generation = generation;
            return this;
        }

        public DataObject WithPreviousGeneration(string previousGeneration) {
            this.PreviousGeneration = previousGeneration;
            return this;
        }

        public DataObject WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public DataObject WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        public DataObject WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):datastore:(?<namespaceName>.+):user:(?<userId>.+):data:(?<dataObjectName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):datastore:(?<namespaceName>.+):user:(?<userId>.+):data:(?<dataObjectName>.+)",
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

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):datastore:(?<namespaceName>.+):user:(?<userId>.+):data:(?<dataObjectName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):datastore:(?<namespaceName>.+):user:(?<userId>.+):data:(?<dataObjectName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _dataObjectNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):datastore:(?<namespaceName>.+):user:(?<userId>.+):data:(?<dataObjectName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetDataObjectNameFromGrn(
            string grn
        )
        {
            var match = _dataObjectNameRegex.Match(grn);
            if (!match.Success || !match.Groups["dataObjectName"].Success)
            {
                return null;
            }
            return match.Groups["dataObjectName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DataObject FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DataObject()
                .WithDataObjectId(!data.Keys.Contains("dataObjectId") || data["dataObjectId"] == null ? null : data["dataObjectId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithScope(!data.Keys.Contains("scope") || data["scope"] == null ? null : data["scope"].ToString())
                .WithAllowUserIds(!data.Keys.Contains("allowUserIds") || data["allowUserIds"] == null || !data["allowUserIds"].IsArray ? new string[]{} : data["allowUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString())
                .WithGeneration(!data.Keys.Contains("generation") || data["generation"] == null ? null : data["generation"].ToString())
                .WithPreviousGeneration(!data.Keys.Contains("previousGeneration") || data["previousGeneration"] == null ? null : data["previousGeneration"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)long.Parse(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData allowUserIdsJsonData = null;
            if (AllowUserIds != null)
            {
                allowUserIdsJsonData = new JsonData();
                foreach (var allowUserId in AllowUserIds)
                {
                    allowUserIdsJsonData.Add(allowUserId);
                }
            }
            return new JsonData {
                ["dataObjectId"] = DataObjectId,
                ["name"] = Name,
                ["userId"] = UserId,
                ["scope"] = Scope,
                ["allowUserIds"] = allowUserIdsJsonData,
                ["status"] = Status,
                ["generation"] = Generation,
                ["previousGeneration"] = PreviousGeneration,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (DataObjectId != null) {
                writer.WritePropertyName("dataObjectId");
                writer.Write(DataObjectId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Scope != null) {
                writer.WritePropertyName("scope");
                writer.Write(Scope.ToString());
            }
            if (AllowUserIds != null) {
                writer.WritePropertyName("allowUserIds");
                writer.WriteArrayStart();
                foreach (var allowUserId in AllowUserIds)
                {
                    if (allowUserId != null) {
                        writer.Write(allowUserId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
            }
            if (Generation != null) {
                writer.WritePropertyName("generation");
                writer.Write(Generation.ToString());
            }
            if (PreviousGeneration != null) {
                writer.WritePropertyName("previousGeneration");
                writer.Write(PreviousGeneration.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write(long.Parse(Revision.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as DataObject;
            var diff = 0;
            if (DataObjectId == null && DataObjectId == other.DataObjectId)
            {
                // null and null
            }
            else
            {
                diff += DataObjectId.CompareTo(other.DataObjectId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Scope == null && Scope == other.Scope)
            {
                // null and null
            }
            else
            {
                diff += Scope.CompareTo(other.Scope);
            }
            if (AllowUserIds == null && AllowUserIds == other.AllowUserIds)
            {
                // null and null
            }
            else
            {
                diff += AllowUserIds.Length - other.AllowUserIds.Length;
                for (var i = 0; i < AllowUserIds.Length; i++)
                {
                    diff += AllowUserIds[i].CompareTo(other.AllowUserIds[i]);
                }
            }
            if (Status == null && Status == other.Status)
            {
                // null and null
            }
            else
            {
                diff += Status.CompareTo(other.Status);
            }
            if (Generation == null && Generation == other.Generation)
            {
                // null and null
            }
            else
            {
                diff += Generation.CompareTo(other.Generation);
            }
            if (PreviousGeneration == null && PreviousGeneration == other.PreviousGeneration)
            {
                // null and null
            }
            else
            {
                diff += PreviousGeneration.CompareTo(other.PreviousGeneration);
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
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }
    }
}