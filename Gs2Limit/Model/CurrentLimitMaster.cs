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

namespace Gs2.Gs2Limit.Model
{
	[Preserve]
	public class CurrentLimitMaster : IComparable
	{

        /** 現在有効な回数制限設定 */
        public string namespaceId { set; get; }

        /**
         * 現在有効な回数制限設定を設定
         *
         * @param namespaceId 現在有効な回数制限設定
         * @return this
         */
        public CurrentLimitMaster WithNamespaceId(string namespaceId) {
            this.namespaceId = namespaceId;
            return this;
        }

        /** マスターデータ */
        public string settings { set; get; }

        /**
         * マスターデータを設定
         *
         * @param settings マスターデータ
         * @return this
         */
        public CurrentLimitMaster WithSettings(string settings) {
            this.settings = settings;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.namespaceId != null)
            {
                writer.WritePropertyName("namespaceId");
                writer.Write(this.namespaceId);
            }
            if(this.settings != null)
            {
                writer.WritePropertyName("settings");
                writer.Write(this.settings);
            }
            writer.WriteObjectEnd();
        }

    public static string GetNamespaceNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):limit:(?<namespaceName>.*):counterLimit");
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
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):limit:(?<namespaceName>.*):counterLimit");
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
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):limit:(?<namespaceName>.*):counterLimit");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static CurrentLimitMaster FromDict(JsonData data)
        {
            return new CurrentLimitMaster()
                .WithNamespaceId(data.Keys.Contains("namespaceId") && data["namespaceId"] != null ? data["namespaceId"].ToString() : null)
                .WithSettings(data.Keys.Contains("settings") && data["settings"] != null ? data["settings"].ToString() : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as CurrentLimitMaster;
            var diff = 0;
            if (namespaceId == null && namespaceId == other.namespaceId)
            {
                // null and null
            }
            else
            {
                diff += namespaceId.CompareTo(other.namespaceId);
            }
            if (settings == null && settings == other.settings)
            {
                // null and null
            }
            else
            {
                diff += settings.CompareTo(other.settings);
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceId"] = namespaceId;
            data["settings"] = settings;
            return data;
        }
	}
}