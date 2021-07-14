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

namespace Gs2.Gs2Experience.Model
{

	[Preserve]
	public class CurrentExperienceMaster : IComparable
	{
        public string NamespaceId { set; get; }
        public string Settings { set; get; }

        public CurrentExperienceMaster WithNamespaceId(string namespaceId) {
            this.NamespaceId = namespaceId;
            return this;
        }

        public CurrentExperienceMaster WithSettings(string settings) {
            this.Settings = settings;
            return this;
        }

    	[Preserve]
        public static CurrentExperienceMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CurrentExperienceMaster()
                .WithNamespaceId(!data.Keys.Contains("namespaceId") || data["namespaceId"] == null ? null : data["namespaceId"].ToString())
                .WithSettings(!data.Keys.Contains("settings") || data["settings"] == null ? null : data["settings"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["settings"] = Settings,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceId != null) {
                writer.WritePropertyName("namespaceId");
                writer.Write(NamespaceId.ToString());
            }
            if (Settings != null) {
                writer.WritePropertyName("settings");
                writer.Write(Settings.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as CurrentExperienceMaster;
            var diff = 0;
            if (NamespaceId == null && NamespaceId == other.NamespaceId)
            {
                // null and null
            }
            else
            {
                diff += NamespaceId.CompareTo(other.NamespaceId);
            }
            if (Settings == null && Settings == other.Settings)
            {
                // null and null
            }
            else
            {
                diff += Settings.CompareTo(other.Settings);
            }
            return diff;
        }
    }
}