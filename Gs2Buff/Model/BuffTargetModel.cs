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

namespace Gs2.Gs2Buff.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class BuffTargetModel : IComparable
	{
        public string TargetModelName { set; get; }
        public string TargetFieldName { set; get; }
        public Gs2.Gs2Buff.Model.BuffTargetGrn[] ConditionGrns { set; get; }
        public float? Rate { set; get; }
        public BuffTargetModel WithTargetModelName(string targetModelName) {
            this.TargetModelName = targetModelName;
            return this;
        }
        public BuffTargetModel WithTargetFieldName(string targetFieldName) {
            this.TargetFieldName = targetFieldName;
            return this;
        }
        public BuffTargetModel WithConditionGrns(Gs2.Gs2Buff.Model.BuffTargetGrn[] conditionGrns) {
            this.ConditionGrns = conditionGrns;
            return this;
        }
        public BuffTargetModel WithRate(float? rate) {
            this.Rate = rate;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BuffTargetModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BuffTargetModel()
                .WithTargetModelName(!data.Keys.Contains("targetModelName") || data["targetModelName"] == null ? null : data["targetModelName"].ToString())
                .WithTargetFieldName(!data.Keys.Contains("targetFieldName") || data["targetFieldName"] == null ? null : data["targetFieldName"].ToString())
                .WithConditionGrns(!data.Keys.Contains("conditionGrns") || data["conditionGrns"] == null || !data["conditionGrns"].IsArray ? null : data["conditionGrns"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Buff.Model.BuffTargetGrn.FromJson(v);
                }).ToArray())
                .WithRate(!data.Keys.Contains("rate") || data["rate"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableFloat(data["rate"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData conditionGrnsJsonData = null;
            if (ConditionGrns != null && ConditionGrns.Length > 0)
            {
                conditionGrnsJsonData = new JsonData();
                foreach (var conditionGrn in ConditionGrns)
                {
                    conditionGrnsJsonData.Add(conditionGrn.ToJson());
                }
            }
            return new JsonData {
                ["targetModelName"] = TargetModelName,
                ["targetFieldName"] = TargetFieldName,
                ["conditionGrns"] = conditionGrnsJsonData,
                ["rate"] = Rate,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TargetModelName != null) {
                writer.WritePropertyName("targetModelName");
                writer.Write(TargetModelName.ToString());
            }
            if (TargetFieldName != null) {
                writer.WritePropertyName("targetFieldName");
                writer.Write(TargetFieldName.ToString());
            }
            if (ConditionGrns != null) {
                writer.WritePropertyName("conditionGrns");
                writer.WriteArrayStart();
                foreach (var conditionGrn in ConditionGrns)
                {
                    if (conditionGrn != null) {
                        conditionGrn.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Rate != null) {
                writer.WritePropertyName("rate");
                writer.Write(float.Parse(Rate.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BuffTargetModel;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type BuffTargetModel.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(TargetModelName, other.TargetModelName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TargetFieldName, other.TargetFieldName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(ConditionGrns, other.ConditionGrns);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Rate, other.Rate);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (TargetModelName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetModel", "buff.buffTargetModel.targetModelName.error.tooLong"),
                    });
                }
            }
            {
                if (TargetFieldName.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetModel", "buff.buffTargetModel.targetFieldName.error.tooLong"),
                    });
                }
            }
            {
                if (ConditionGrns.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetModel", "buff.buffTargetModel.conditionGrns.error.tooFew"),
                    });
                }
                if (ConditionGrns.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetModel", "buff.buffTargetModel.conditionGrns.error.tooMany"),
                    });
                }
            }
            {
                if (Rate < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetModel", "buff.buffTargetModel.rate.error.invalid"),
                    });
                }
                if (Rate > 1000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetModel", "buff.buffTargetModel.rate.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new BuffTargetModel {
                TargetModelName = TargetModelName,
                TargetFieldName = TargetFieldName,
                ConditionGrns = ConditionGrns?.Clone() as Gs2.Gs2Buff.Model.BuffTargetGrn[],
                Rate = Rate,
            };
        }
    }
}