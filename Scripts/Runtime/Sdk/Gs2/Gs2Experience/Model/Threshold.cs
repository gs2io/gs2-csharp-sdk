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
	public class Threshold : IComparable
	{

        /** ランクアップ閾値のメタデータ */
        public string metadata { set; get; }

        /**
         * ランクアップ閾値のメタデータを設定
         *
         * @param metadata ランクアップ閾値のメタデータ
         * @return this
         */
        public Threshold WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }

        /** ランクアップ経験値閾値リスト */
        public List<long?> values { set; get; }

        /**
         * ランクアップ経験値閾値リストを設定
         *
         * @param values ランクアップ経験値閾値リスト
         * @return this
         */
        public Threshold WithValues(List<long?> values) {
            this.values = values;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.metadata != null)
            {
                writer.WritePropertyName("metadata");
                writer.Write(this.metadata);
            }
            if(this.values != null)
            {
                writer.WritePropertyName("values");
                writer.WriteArrayStart();
                foreach(var item in this.values)
                {
                    writer.Write(item.Value);
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

    	[Preserve]
        public static Threshold FromDict(JsonData data)
        {
            return new Threshold()
                .WithMetadata(data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString() : null)
                .WithValues(data.Keys.Contains("values") && data["values"] != null ? data["values"].Cast<JsonData>().Select(value =>
                    {
                        return (long?)long.Parse(value.ToString());
                    }
                ).ToList() : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Threshold;
            var diff = 0;
            if (metadata == null && metadata == other.metadata)
            {
                // null and null
            }
            else
            {
                diff += metadata.CompareTo(other.metadata);
            }
            if (values == null && values == other.values)
            {
                // null and null
            }
            else
            {
                diff += values.Count - other.values.Count;
                for (var i = 0; i < values.Count; i++)
                {
                    diff += (int)(values[i] - other.values[i]);
                }
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["metadata"] = metadata;
            data["values"] = new JsonData(values);
            return data;
        }
	}
}