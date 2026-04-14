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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Log.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Facet : IComparable
	{
        public string Field { set; get; }
        public Gs2.Gs2Log.Model.FacetValueCount[] Values { set; get; }
        public Gs2.Gs2Log.Model.NumericRange Range { set; get; }
        public Gs2.Gs2Log.Model.NumericRange GlobalRange { set; get; }
        public Facet WithField(string field) {
            this.Field = field;
            return this;
        }
        public Facet WithValues(Gs2.Gs2Log.Model.FacetValueCount[] values) {
            this.Values = values;
            return this;
        }
        public Facet WithRange(Gs2.Gs2Log.Model.NumericRange range) {
            this.Range = range;
            return this;
        }
        public Facet WithGlobalRange(Gs2.Gs2Log.Model.NumericRange globalRange) {
            this.GlobalRange = globalRange;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Facet FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Facet()
                .WithField(!data.Keys.Contains("field") || data["field"] == null ? null : data["field"].ToString())
                .WithValues(!data.Keys.Contains("values") || data["values"] == null || !data["values"].IsArray ? null : data["values"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Log.Model.FacetValueCount.FromJson(v);
                }).ToArray())
                .WithRange(!data.Keys.Contains("range") || data["range"] == null ? null : Gs2.Gs2Log.Model.NumericRange.FromJson(data["range"]))
                .WithGlobalRange(!data.Keys.Contains("globalRange") || data["globalRange"] == null ? null : Gs2.Gs2Log.Model.NumericRange.FromJson(data["globalRange"]));
        }

        public JsonData ToJson()
        {
            JsonData valuesJsonData = null;
            if (Values != null && Values.Length > 0)
            {
                valuesJsonData = new JsonData();
                foreach (var value in Values)
                {
                    valuesJsonData.Add(value.ToJson());
                }
            }
            return new JsonData {
                ["field"] = Field,
                ["values"] = valuesJsonData,
                ["range"] = Range?.ToJson(),
                ["globalRange"] = GlobalRange?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Field != null) {
                writer.WritePropertyName("field");
                writer.Write(Field.ToString());
            }
            if (Values != null) {
                writer.WritePropertyName("values");
                writer.WriteArrayStart();
                foreach (var value in Values)
                {
                    if (value != null) {
                        value.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Range != null) {
                writer.WritePropertyName("range");
                Range.WriteJson(writer);
            }
            if (GlobalRange != null) {
                writer.WritePropertyName("globalRange");
                GlobalRange.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Facet;
            var diff = 0;
            if (Field == null && Field == other.Field)
            {
                // null and null
            }
            else
            {
                diff += Field.CompareTo(other.Field);
            }
            if (Values == null && Values == other.Values)
            {
                // null and null
            }
            else
            {
                diff += Values.Length - other.Values.Length;
                for (var i = 0; i < Values.Length; i++)
                {
                    diff += Values[i].CompareTo(other.Values[i]);
                }
            }
            if (Range == null && Range == other.Range)
            {
                // null and null
            }
            else
            {
                diff += Range.CompareTo(other.Range);
            }
            if (GlobalRange == null && GlobalRange == other.GlobalRange)
            {
                // null and null
            }
            else
            {
                diff += GlobalRange.CompareTo(other.GlobalRange);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Field.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("facet", "log.facet.field.error.tooLong"),
                    });
                }
            }
            {
                if (Values.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("facet", "log.facet.values.error.tooMany"),
                    });
                }
            }
            {
            }
            {
            }
        }

        public object Clone() {
            return new Facet {
                Field = Field,
                Values = Values?.Clone() as Gs2.Gs2Log.Model.FacetValueCount[],
                Range = Range?.Clone() as Gs2.Gs2Log.Model.NumericRange,
                GlobalRange = GlobalRange?.Clone() as Gs2.Gs2Log.Model.NumericRange,
            };
        }
    }
}