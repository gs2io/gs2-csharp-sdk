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

namespace Gs2.Gs2Deploy.Model
{
	[Preserve]
	public class Stack : IComparable
	{

        /** スタック */
        public string stackId { set; get; }

        /**
         * スタックを設定
         *
         * @param stackId スタック
         * @return this
         */
        public Stack WithStackId(string stackId) {
            this.stackId = stackId;
            return this;
        }

        /** オーナーID */
        public string ownerId { set; get; }

        /**
         * オーナーIDを設定
         *
         * @param ownerId オーナーID
         * @return this
         */
        public Stack WithOwnerId(string ownerId) {
            this.ownerId = ownerId;
            return this;
        }

        /** スタック名 */
        public string name { set; get; }

        /**
         * スタック名を設定
         *
         * @param name スタック名
         * @return this
         */
        public Stack WithName(string name) {
            this.name = name;
            return this;
        }

        /** スタックの説明 */
        public string description { set; get; }

        /**
         * スタックの説明を設定
         *
         * @param description スタックの説明
         * @return this
         */
        public Stack WithDescription(string description) {
            this.description = description;
            return this;
        }

        /** テンプレートデータ */
        public string template { set; get; }

        /**
         * テンプレートデータを設定
         *
         * @param template テンプレートデータ
         * @return this
         */
        public Stack WithTemplate(string template) {
            this.template = template;
            return this;
        }

        /** 実行状態 */
        public string status { set; get; }

        /**
         * 実行状態を設定
         *
         * @param status 実行状態
         * @return this
         */
        public Stack WithStatus(string status) {
            this.status = status;
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
        public Stack WithCreatedAt(long? createdAt) {
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
        public Stack WithUpdatedAt(long? updatedAt) {
            this.updatedAt = updatedAt;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.stackId != null)
            {
                writer.WritePropertyName("stackId");
                writer.Write(this.stackId);
            }
            if(this.ownerId != null)
            {
                writer.WritePropertyName("ownerId");
                writer.Write(this.ownerId);
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
            if(this.template != null)
            {
                writer.WritePropertyName("template");
                writer.Write(this.template);
            }
            if(this.status != null)
            {
                writer.WritePropertyName("status");
                writer.Write(this.status);
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

    public static string GetStackNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):stack:(?<stackName>.*)");
        if (!match.Groups["stackName"].Success)
        {
            return null;
        }
        return match.Groups["stackName"].Value;
    }

    public static string GetOwnerIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):stack:(?<stackName>.*)");
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
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):stack:(?<stackName>.*)");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static Stack FromDict(JsonData data)
        {
            return new Stack()
                .WithStackId(data.Keys.Contains("stackId") && data["stackId"] != null ? data["stackId"].ToString() : null)
                .WithOwnerId(data.Keys.Contains("ownerId") && data["ownerId"] != null ? data["ownerId"].ToString() : null)
                .WithName(data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString() : null)
                .WithDescription(data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString() : null)
                .WithTemplate(data.Keys.Contains("template") && data["template"] != null ? data["template"].ToString() : null)
                .WithStatus(data.Keys.Contains("status") && data["status"] != null ? data["status"].ToString() : null)
                .WithCreatedAt(data.Keys.Contains("createdAt") && data["createdAt"] != null ? (long?)long.Parse(data["createdAt"].ToString()) : null)
                .WithUpdatedAt(data.Keys.Contains("updatedAt") && data["updatedAt"] != null ? (long?)long.Parse(data["updatedAt"].ToString()) : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Stack;
            var diff = 0;
            if (stackId == null && stackId == other.stackId)
            {
                // null and null
            }
            else
            {
                diff += stackId.CompareTo(other.stackId);
            }
            if (ownerId == null && ownerId == other.ownerId)
            {
                // null and null
            }
            else
            {
                diff += ownerId.CompareTo(other.ownerId);
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
            if (template == null && template == other.template)
            {
                // null and null
            }
            else
            {
                diff += template.CompareTo(other.template);
            }
            if (status == null && status == other.status)
            {
                // null and null
            }
            else
            {
                diff += status.CompareTo(other.status);
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
            data["stackId"] = stackId;
            data["ownerId"] = ownerId;
            data["name"] = name;
            data["description"] = description;
            data["template"] = template;
            data["status"] = status;
            data["createdAt"] = createdAt;
            data["updatedAt"] = updatedAt;
            return data;
        }
	}
}