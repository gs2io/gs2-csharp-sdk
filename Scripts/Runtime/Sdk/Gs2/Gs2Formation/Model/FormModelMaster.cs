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

namespace Gs2.Gs2Formation.Model
{
	[Preserve]
	public class FormModelMaster : IComparable
	{

        /** フォームマスター */
        public string formModelId { set; get; }

        /**
         * フォームマスターを設定
         *
         * @param formModelId フォームマスター
         * @return this
         */
        public FormModelMaster WithFormModelId(string formModelId) {
            this.formModelId = formModelId;
            return this;
        }

        /** フォーム名 */
        public string name { set; get; }

        /**
         * フォーム名を設定
         *
         * @param name フォーム名
         * @return this
         */
        public FormModelMaster WithName(string name) {
            this.name = name;
            return this;
        }

        /** フォームマスターの説明 */
        public string description { set; get; }

        /**
         * フォームマスターの説明を設定
         *
         * @param description フォームマスターの説明
         * @return this
         */
        public FormModelMaster WithDescription(string description) {
            this.description = description;
            return this;
        }

        /** フォームのメタデータ */
        public string metadata { set; get; }

        /**
         * フォームのメタデータを設定
         *
         * @param metadata フォームのメタデータ
         * @return this
         */
        public FormModelMaster WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }

        /** スロットリスト */
        public List<SlotModel> slots { set; get; }

        /**
         * スロットリストを設定
         *
         * @param slots スロットリスト
         * @return this
         */
        public FormModelMaster WithSlots(List<SlotModel> slots) {
            this.slots = slots;
            return this;
        }

        /** 作成日時 */
        public long? createdAt { set; get; }

        /**
         * 作成日時を設定
         *
         * @param createdAt 作成日時
         * @return this
         */
        public FormModelMaster WithCreatedAt(long? createdAt) {
            this.createdAt = createdAt;
            return this;
        }

        /** 最終更新日時 */
        public long? updatedAt { set; get; }

        /**
         * 最終更新日時を設定
         *
         * @param updatedAt 最終更新日時
         * @return this
         */
        public FormModelMaster WithUpdatedAt(long? updatedAt) {
            this.updatedAt = updatedAt;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.formModelId != null)
            {
                writer.WritePropertyName("formModelId");
                writer.Write(this.formModelId);
            }
            if(this.name != null)
            {
                writer.WritePropertyName("name");
                writer.Write(this.name);
            }
            if(this.description != null)
            {
                writer.WritePropertyName("description");
                writer.Write(this.description);
            }
            if(this.metadata != null)
            {
                writer.WritePropertyName("metadata");
                writer.Write(this.metadata);
            }
            if(this.slots != null)
            {
                writer.WritePropertyName("slots");
                writer.WriteArrayStart();
                foreach(var item in this.slots)
                {
                    item.WriteJson(writer);
                }
                writer.WriteArrayEnd();
            }
            if(this.createdAt.HasValue)
            {
                writer.WritePropertyName("createdAt");
                writer.Write(this.createdAt.Value);
            }
            if(this.updatedAt.HasValue)
            {
                writer.WritePropertyName("updatedAt");
                writer.Write(this.updatedAt.Value);
            }
            writer.WriteObjectEnd();
        }

    public static string GetFormModelNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):formation:(?<namespaceName>.*):model:(?<formModelName>.*)");
        if (!match.Groups["formModelName"].Success)
        {
            return null;
        }
        return match.Groups["formModelName"].Value;
    }

    public static string GetNamespaceNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):formation:(?<namespaceName>.*):model:(?<formModelName>.*)");
        if (!match.Groups["namespaceName"].Success)
        {
            return null;
        }
        return match.Groups["namespaceName"].Value;
    }

    public static string GetOwnerIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):formation:(?<namespaceName>.*):model:(?<formModelName>.*)");
        if (!match.Groups["ownerId"].Success)
        {
            return null;
        }
        return match.Groups["ownerId"].Value;
    }

    public static string GetRegionFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):formation:(?<namespaceName>.*):model:(?<formModelName>.*)");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static FormModelMaster FromDict(JsonData data)
        {
            return new FormModelMaster()
                .WithFormModelId(data.Keys.Contains("formModelId") && data["formModelId"] != null ? data["formModelId"].ToString() : null)
                .WithName(data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString() : null)
                .WithDescription(data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString() : null)
                .WithMetadata(data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString() : null)
                .WithSlots(data.Keys.Contains("slots") && data["slots"] != null ? data["slots"].Cast<JsonData>().Select(value =>
                    {
                        return Gs2.Gs2Formation.Model.SlotModel.FromDict(value);
                    }
                ).ToList() : null)
                .WithCreatedAt(data.Keys.Contains("createdAt") && data["createdAt"] != null ? (long?)long.Parse(data["createdAt"].ToString()) : null)
                .WithUpdatedAt(data.Keys.Contains("updatedAt") && data["updatedAt"] != null ? (long?)long.Parse(data["updatedAt"].ToString()) : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as FormModelMaster;
            var diff = 0;
            if (formModelId == null && formModelId == other.formModelId)
            {
                // null and null
            }
            else
            {
                diff += formModelId.CompareTo(other.formModelId);
            }
            if (name == null && name == other.name)
            {
                // null and null
            }
            else
            {
                diff += name.CompareTo(other.name);
            }
            if (description == null && description == other.description)
            {
                // null and null
            }
            else
            {
                diff += description.CompareTo(other.description);
            }
            if (metadata == null && metadata == other.metadata)
            {
                // null and null
            }
            else
            {
                diff += metadata.CompareTo(other.metadata);
            }
            if (slots == null && slots == other.slots)
            {
                // null and null
            }
            else
            {
                diff += slots.Count - other.slots.Count;
                for (var i = 0; i < slots.Count; i++)
                {
                    diff += slots[i].CompareTo(other.slots[i]);
                }
            }
            if (createdAt == null && createdAt == other.createdAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(createdAt - other.createdAt);
            }
            if (updatedAt == null && updatedAt == other.updatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(updatedAt - other.updatedAt);
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["formModelId"] = formModelId;
            data["name"] = name;
            data["description"] = description;
            data["metadata"] = metadata;
            data["slots"] = new JsonData(slots.Select(item => item.ToDict()));
            data["createdAt"] = createdAt;
            data["updatedAt"] = updatedAt;
            return data;
        }
	}
}