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

namespace Gs2.Gs2Formation.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Form : IComparable
	{
        public string FormId { set; get; }
        public string Name { set; get; }
        public int? Index { set; get; }
        public Gs2.Gs2Formation.Model.Slot[] Slots { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public Form WithFormId(string formId) {
            this.FormId = formId;
            return this;
        }
        public Form WithName(string name) {
            this.Name = name;
            return this;
        }
        public Form WithIndex(int? index) {
            this.Index = index;
            return this;
        }
        public Form WithSlots(Gs2.Gs2Formation.Model.Slot[] slots) {
            this.Slots = slots;
            return this;
        }
        public Form WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Form WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public Form WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):user:(?<userId>.+):mold:(?<moldName>.+):form:(?<index>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):user:(?<userId>.+):mold:(?<moldName>.+):form:(?<index>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):user:(?<userId>.+):mold:(?<moldName>.+):form:(?<index>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):user:(?<userId>.+):mold:(?<moldName>.+):form:(?<index>.+)",
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

        private static System.Text.RegularExpressions.Regex _moldNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):user:(?<userId>.+):mold:(?<moldName>.+):form:(?<index>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetMoldNameFromGrn(
            string grn
        )
        {
            var match = _moldNameRegex.Match(grn);
            if (!match.Success || !match.Groups["moldName"].Success)
            {
                return null;
            }
            return match.Groups["moldName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _indexRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):formation:(?<namespaceName>.+):user:(?<userId>.+):mold:(?<moldName>.+):form:(?<index>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetIndexFromGrn(
            string grn
        )
        {
            var match = _indexRegex.Match(grn);
            if (!match.Success || !match.Groups["index"].Success)
            {
                return null;
            }
            return match.Groups["index"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Form FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Form()
                .WithFormId(!data.Keys.Contains("formId") || data["formId"] == null ? null : data["formId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithIndex(!data.Keys.Contains("index") || data["index"] == null ? null : (int?)int.Parse(data["index"].ToString()))
                .WithSlots(!data.Keys.Contains("slots") || data["slots"] == null ? new Gs2.Gs2Formation.Model.Slot[]{} : data["slots"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Formation.Model.Slot.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)long.Parse(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["formId"] = FormId,
                ["name"] = Name,
                ["index"] = Index,
                ["slots"] = Slots == null ? null : new JsonData(
                        Slots.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (FormId != null) {
                writer.WritePropertyName("formId");
                writer.Write(FormId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Index != null) {
                writer.WritePropertyName("index");
                writer.Write(int.Parse(Index.ToString()));
            }
            if (Slots != null) {
                writer.WritePropertyName("slots");
                writer.WriteArrayStart();
                foreach (var slot in Slots)
                {
                    if (slot != null) {
                        slot.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            var other = obj as Form;
            var diff = 0;
            if (FormId == null && FormId == other.FormId)
            {
                // null and null
            }
            else
            {
                diff += FormId.CompareTo(other.FormId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Index == null && Index == other.Index)
            {
                // null and null
            }
            else
            {
                diff += (int)(Index - other.Index);
            }
            if (Slots == null && Slots == other.Slots)
            {
                // null and null
            }
            else
            {
                diff += Slots.Length - other.Slots.Length;
                for (var i = 0; i < Slots.Length; i++)
                {
                    diff += Slots[i].CompareTo(other.Slots[i]);
                }
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