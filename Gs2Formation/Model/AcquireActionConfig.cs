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
	public class AcquireActionConfig : IComparable
	{
        public string Name { set; get; }
        public Gs2.Gs2Formation.Model.Config[] Config { set; get; }
        public AcquireActionConfig WithName(string name) {
            this.Name = name;
            return this;
        }
        public AcquireActionConfig WithConfig(Gs2.Gs2Formation.Model.Config[] config) {
            this.Config = config;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AcquireActionConfig FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcquireActionConfig()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null || !data["config"].IsArray ? new Gs2.Gs2Formation.Model.Config[]{} : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Formation.Model.Config.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData configJsonData = null;
            if (Config != null && Config.Length > 0)
            {
                configJsonData = new JsonData();
                foreach (var confi in Config)
                {
                    configJsonData.Add(confi.ToJson());
                }
            }
            return new JsonData {
                ["name"] = Name,
                ["config"] = configJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Config != null) {
                writer.WritePropertyName("config");
                writer.WriteArrayStart();
                foreach (var confi in Config)
                {
                    if (confi != null) {
                        confi.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AcquireActionConfig;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Config == null && Config == other.Config)
            {
                // null and null
            }
            else
            {
                diff += Config.Length - other.Config.Length;
                for (var i = 0; i < Config.Length; i++)
                {
                    diff += Config[i].CompareTo(other.Config[i]);
                }
            }
            return diff;
        }
    }
}